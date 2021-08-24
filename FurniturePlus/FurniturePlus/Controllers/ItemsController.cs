using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Infrastructure;
using FurniturePlus.Models.Items;
using FurniturePlus.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class ItemsController : Controller
    {
        private readonly FurniturePlusDbContext data;
        private readonly IItemService items;

        public ItemsController(FurniturePlusDbContext data, IItemService items)
        {
            this.data = data;
            this.items = items;
        }

        public IActionResult All([FromQuery] ItemSearchModel query)
        {
            var itemQuery = this.data.Items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CategoryId))
            {
                itemQuery = itemQuery.Where(i => i.CategoryId.ToString() == query.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(query.VendorId))
            {
                itemQuery = itemQuery.Where(i => i.VendorId.ToString() == query.VendorId);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                itemQuery = itemQuery
                    .Where(i =>
                    i.Category.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    i.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    i.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            switch (query.Sorting)
            {
                case ItemSorting.Category:
                    itemQuery = itemQuery.OrderBy(i => i.Category.Name);
                    break;
                case ItemSorting.Name:
                    itemQuery = itemQuery.OrderBy(i => i.Name);
                    break;
                case ItemSorting.PriceAscending:
                    itemQuery = itemQuery.OrderBy(i => i.Price);
                    break;
                case ItemSorting.PriceDescending:
                    itemQuery = itemQuery.OrderByDescending(i => i.Price);
                    break;
            }

            var items = itemQuery
                .Skip((query.CurrentPage - 1) * ItemSearchModel.ItemsPerPage)
                .Take(ItemSearchModel.ItemsPerPage)
                .Select(i => new ItemListingViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    PurchaseCode = i.PurchaseCode,
                    Category = i.Category,
                    ImageUrl = i.ImageUrl,
                    Description = i.Description,
                    Price = i.Price
                })
                .ToList();

            var itemCategories = this.data
                .Items
                .Select(i => i.Category)
                .Distinct()
                .ToList();

            query.Categories = itemCategories;
            query.Items = items;
            query.ItemsCount = itemQuery.Count();

            return View(query);
        }

        [Authorize]
        public IActionResult AddItem()
        {
            if (!this.IsSalesman() && !User.IsAdmin())
            {
                return RedirectToAction(nameof(SalesmenController.RegisterSalesman), "Salesmen");
            }

            return View(new AddItemFormModel
            {
                ItemCategories = this.GetItemCategories()
            });
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        //Model binding: ASP.NET core ще попълни модела (AddItemFormModel item) с данните от request-a и ще върне view
        public IActionResult AddItem(AddItemFormModel item)
        {
            if (!this.data.Categories.Any(c => c.Id == item.CategoryId))
            {
                this.ModelState.AddModelError(nameof(item.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                item.ItemCategories = this.GetItemCategories();
                return View(item);
            }

            var itemVendor = this.data
                .Vendors
                .Where(v => v.Id == this.data
                .Salesmen
                .Where(s => s.UserId == this.User.GetId())
                .FirstOrDefault()
                .VendorId)
                .FirstOrDefault();

            //Purchase Code = first 3 letters from Vendor's name + random 6 digits number;
            var rnd = new Random();
            var purchaseCode = itemVendor.Name.Substring(0, 3).ToUpper() + (rnd.Next(0, 1000000).ToString("D6"));

            var newItem = new Item
            {
                Id = item.Id,
                Name = item.Name,
                PurchaseCode = purchaseCode,
                Category = item.Category,
                CategoryId = item.CategoryId,
                Vendor = itemVendor,
                ImageUrl = item.ImageUrl,
                Description = item.Description,
                Price = item.Price
            };
            this.data.Items.Add(newItem);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult EditItem(int id)
        {
            if (!this.IsSalesman() && !User.IsAdmin())
            {
                return RedirectToAction(nameof(SalesmenController.RegisterSalesman), "Salesmen");
            }

            var item = this.data
                .Items
                .FirstOrDefault(i => i.Id == id);

            return View(new EditItemFormModel
            {
                Id = item.Id,
                Name = item.Name,
                Category = item.Category,
                CategoryId = item.CategoryId,
                Vendor = item.Vendor,
                ImageUrl = item.ImageUrl,
                Description = item.Description,
                Price = item.Price,
                ItemCategories = this.GetItemCategories()
            });
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        //Model binding: ASP.NET core ще попълни модела (AddItemFormModel item) с данните от request-a и ще върне view
        public IActionResult EditItem(EditItemFormModel item, int id)
        {
            if (!this.data.Categories.Any(c => c.Id == item.CategoryId))
            {
                this.ModelState.AddModelError(nameof(item.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                item.ItemCategories = this.GetItemCategories();
                return View(item);
            }

            var currentItem = this.data
                .Items
                .FirstOrDefault(i => i.Id == id);

            // currentItem.Category = item.Category;
            currentItem.Name = item.Name;
            currentItem.Description = item.Description;
            currentItem.ImageUrl = item.ImageUrl;
            currentItem.Price = item.Price;

            this.data.SaveChanges();

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Details(int id)    //parameter named "id" --> "id" in "asp-route-id" 
        {
            var isEditable = false;

            if (IsAuthenticated())
            {
                isEditable = IsSalesman()
                    && (this.data
                          .Salesmen
                          .FirstOrDefault(s => s.UserId == this.User.GetId())
                          .VendorId
                          == this.data
                          .Items
                          .FirstOrDefault(i => i.Id == id)
                          .VendorId);
            }

            var item = this.items.Details(id, isEditable);
            return View(item);
        }

        public bool IsSalesman()
        {
            return this.data.Salesmen.Any(s => s.UserId == this.User.GetId())
                    ? this.data.Salesmen.FirstOrDefault(s => s.UserId == this.User.GetId()).IsApproved
                    : false;
        }
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        private IEnumerable<ItemCategoryViewModel> GetItemCategories()
        {
            return data
                .Categories
                .Select(c => new ItemCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
