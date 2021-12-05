using FurniturePlus.Data;
using FurniturePlus.Infrastructure;
using FurniturePlus.Models.Items;
using FurniturePlus.Services.Items;
using FurniturePlus.Services.Salesmen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemService items;
        private readonly ISalesmanService salesmen;

        public ItemsController(IItemService items, ISalesmanService salesmen)
        {

            this.items = items;
            this.salesmen = salesmen;
        }

        public IActionResult All([FromQuery] ItemSearchModel query)
        {
            return View(this.items.All(query));
        }

        [Authorize]
        public IActionResult AddItem()
        {
            if (!this.IsSalesman(this.User.GetId()) && !User.IsAdmin())
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
            if (!this.items.DoesCategoryExist(item.CategoryId))
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
            if (!this.IsSalesman(this.User.GetId()) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(SalesmenController.RegisterSalesman), "Salesmen");
            }

            var item = this.items.EditItem(id);

            return View(item);
        }



        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        //Model binding: ASP.NET core ще попълни модела (AddItemFormModel item) с данните от request-a и ще върне view
        public IActionResult EditItem(EditItemFormModel item)
        {

            if (!this.items.DoesCategoryExist(item.CategoryId))
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


        public IActionResult Details(int id)    //parameter named "id" --> "id" in "asp-route-id" 
        {
            var currentUserId = "";
            if (IsAuthenticated())
            {
                currentUserId = this.User.GetId();
            }
            var item = this.items.Details(id, currentUserId, this.IsSalesman(currentUserId));
            return View(item);
        }

        public bool IsSalesman(string userId)
        {
            return this.salesmen.IsUserASalesman(userId) ? this.salesmen.IsSalesmanApproved(userId) : false;
        }
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }


    }
}
