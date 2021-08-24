using FurniturePlus.Models;
using FurniturePlus.Models.Home;
using FurniturePlus.Services.Items;
using FurniturePlus.Services.Vendors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FurniturePlus.Controllers
{
    public class HomeController : Controller
    {
        private readonly IItemService items;
        private readonly IVendorService vendors;

        public HomeController(IItemService items, IVendorService vendors)
        {
            this.items = items;
            this.vendors = vendors;
        }
        public IActionResult Index()
        {
            var totalItems = this.items.ItemsCount();
            var totalVendors = this.vendors.VendorsCount();

            var items = this.items.GetAllItemsForHomePage();

            return View(new IndexViewModel
            {
                TotalItems = totalItems,
                Items = items,
                TotalVendors = totalVendors
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
