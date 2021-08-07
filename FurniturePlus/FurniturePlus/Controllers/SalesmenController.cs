using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Data;
using FurniturePlus.Models.Salesmen;
using System.Linq;
using FurniturePlus.Infrastructure;
using FurniturePlus.Data.Models;

namespace FurniturePlus.Controllers
{
    public class SalesmenController : Controller
    {
        private readonly FurniturePlusDbContext data;

        public SalesmenController(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult RegisterSalesman()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult RegisterSalesman(RegisterSalesmanFormModel salesman)
        {
            var userId = this.User.GetId();
            var userIsAlreadySalesman = this.data
                .Salesmen
                .Any(s => s.UserId == userId);

            if (userIsAlreadySalesman)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                return View(salesman);
            }

            var newSalesman = new Salesman
            {
                FirstName = salesman.FirstName,
                LastName = salesman.LastName,
                PhoneNumber = salesman.PhoneNumber,
                UserId = userId,
                Vendor = this.data
                .Vendors
                .FirstOrDefault(v => v.Id == salesman.VendorId)
            };

            this.data.Salesmen.Add(newSalesman);
            this.data.SaveChanges();

            return RedirectToAction("All", "Items");
        }
    }
}
