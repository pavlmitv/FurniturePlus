using FurniturePlus.Data;
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
            return View();
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
