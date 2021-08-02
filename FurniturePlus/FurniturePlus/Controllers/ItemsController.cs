using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Models.Items;
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

        public IActionResult Add()
        {
            return View(new AddItemFormModel
            {
                ItemCategories = this.GetItemCategories()
            });
        }

        [HttpPost]
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
                PurchaseCode = "VEN001",
                Category = item.Category,
                CategoryId = item.CategoryId,
                Vendor = this.data.Vendors.FirstOrDefault(),
                ImageUrl = item.ImageUrl,
                Description = item.Description,
                Price = item.Price
            };
            this.data.Items.Add(newItem);
            this.data.SaveChanges();
            return RedirectToAction("Index", "Home");
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
