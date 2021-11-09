using FurniturePlus.Data;
using FurniturePlus.Infrastructure;
using FurniturePlus.Models.Items;
using FurniturePlus.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View(this.items.All(query));
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
                ItemCategories = this.items.GetItemCategories()
            });
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddItem(AddItemFormModel item)
        {
            if (!this.items.DoesCategoryExist(item.Id))
            {
                this.ModelState.AddModelError(nameof(item.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                item.ItemCategories = this.items.GetItemCategories();
                return View(item);
            }

            var currentUserId = IsAuthenticated() ? this.User.GetId() : "";

            this.items.AddItem(item, currentUserId);
            return RedirectToAction(nameof(All));
        }


        [Authorize]
        public IActionResult EditItem(int id)
        {
            if (!this.IsSalesman() && !User.IsAdmin())
            {
                return RedirectToAction(nameof(SalesmenController.RegisterSalesman), "Salesmen");
            }

            var item = this.items.EditItem(id);

            return View(item);
        }

        //[Authorize]
        //public IActionResult EditItem(int id)
        //{
        //    if (!this.IsSalesman() && !User.IsAdmin())
        //    {
        //        return RedirectToAction(nameof(SalesmenController.RegisterSalesman), "Salesmen");
        //    }

        //   var item= this.data
        //        .Items
        //        .Where(i => i.Id == id)
        //        .Select(i => new EditItemFormModel
        //        {
        //            Id = i.Id,
        //            Name = i.Name,
        //            Category = i.Category,
        //            CategoryId = i.CategoryId,
        //            Vendor = i.Vendor,
        //            ImageUrl = i.ImageUrl,
        //            Description = i.Description,
        //            Price = i.Price,
        //            ItemCategories = this.GetItemCategories()
        //        })
        //        .FirstOrDefault();

        //    return View(item);
        //}

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        //Model binding: ASP.NET core ще попълни модела (AddItemFormModel item) с данните от request-a и ще върне view
        public IActionResult EditItem(EditItemFormModel item)
        {

            if (!this.items.DoesCategoryExist(item.Id))
            {
                this.ModelState.AddModelError(nameof(item.CategoryId), "Category does not exist.");
            }
            if (!ModelState.IsValid)
            {
                item.ItemCategories = this.items.GetItemCategories();
                return View(item);
            }

            this.items.EditItem(item);

            return RedirectToAction(nameof(Details), new { id = item.Id });
        }

        //public IActionResult EditItem(EditItemFormModel item, int id)
        //{

        //    //if (!this.items.DoesCategoryExist(item.Id))
        //    //{
        //    //    this.ModelState.AddModelError(nameof(item.CategoryId), "Category does not exist.");
        //    //}
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    item.ItemCategories = this.items.GetItemCategories();
        //    //    return View(item);
        //    //}

        //    if (!this.data.Categories.Any(c => c.Id == item.CategoryId))
        //    {
        //        this.ModelState.AddModelError(nameof(item.CategoryId), "Category does not exist.");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        item.ItemCategories = this.GetItemCategories();
        //        return View(item);
        //    }

        //    var currentItem = this.data
        //        .Items
        //        .FirstOrDefault(i => i.Id == id);

        //    // currentItem.Category = item.Category;
        //    currentItem.Name = item.Name;
        //    currentItem.Description = item.Description;
        //    currentItem.ImageUrl = item.ImageUrl;
        //    currentItem.Price = item.Price;

        //    this.data.SaveChanges();

        //    return RedirectToAction(nameof(Details), new { id });
        //}

        public IActionResult Details(int id)    //parameter named "id" --> "id" in "asp-route-id" 
        {
            var currentUserId = "";
            if (IsAuthenticated())
            {
                currentUserId = this.User.GetId();
            }
            var item = this.items.Details(id, currentUserId, this.IsSalesman());
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

        //private IEnumerable<ItemCategoryViewModel> GetItemCategories()
        //{
        //    return data
        //        .Categories
        //        .Select(c => new ItemCategoryViewModel
        //        {
        //            Id = c.Id,
        //            Name = c.Name
        //        })
        //        .ToList();
        //}
    }
}
