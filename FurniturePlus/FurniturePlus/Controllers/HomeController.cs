using FurniturePlus.Data;
using FurniturePlus.Models;
using FurniturePlus.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class HomeController : Controller
    {
        private readonly FurniturePlusDbContext data;

        public HomeController(FurniturePlusDbContext data)
        {
            this.data = data;
        }
        public IActionResult Index()
        {
            var totalItems = this.data.Items.Count();

            var items = this.data
                    .Items
                    .Select(i => new ItemIndexViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        PurchaseCode = i.PurchaseCode,
                        ImageUrl = i.ImageUrl,
                        Description = i.Description,
                        Price = i.Price
                    })
                    .Take(3)
                    .ToList();

            return View(new IndexViewModel
            {
                TotalItems = totalItems,
                Items = items
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
