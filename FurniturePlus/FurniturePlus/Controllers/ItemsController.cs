﻿using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class ItemsController : Controller
    {
        private readonly FurniturePlusDbContext data;

        public ItemsController(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery] ItemSearchModel query)
        {
            var itemQuery = this.data.Items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CategoryId))
            {
                itemQuery = itemQuery.Where(i => i.CategoryId.ToString() == query.CategoryId);
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
        public IActionResult Add()
        {
            return View(new AddItemFormModel
            {
                ItemCategories = this.GetItemCategories()
            });
        }

        [HttpPost]
        [Authorize]
        //Model binding: ASP.NET core ще попълни модела (AddItemFormModel item) с данните от request-a и ще върне view
        public IActionResult Add(AddItemFormModel item)
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

            var newItem = new Item
            {
                Id = item.Id,
                Name = item.Name,
                PurchaseCode = "VEN001",        //TODO
                Category = item.Category,
                CategoryId = item.CategoryId,
                Vendor = this.data.Vendors.FirstOrDefault(),        //TODO
                ImageUrl = item.ImageUrl,
                Description = item.Description,
                Price = item.Price
            };
            this.data.Items.Add(newItem);
            this.data.SaveChanges();
            return RedirectToAction(nameof(All));
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