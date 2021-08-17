using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Data;
using FurniturePlus.Models.Salesmen;
using System.Linq;
using FurniturePlus.Infrastructure;
using FurniturePlus.Data.Models;
using System.Collections.Generic;

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
            return View(new RegisterSalesmanFormModel
            {
                SalesmanVendors = this.GetVendors()
            });
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult RegisterSalesman(RegisterSalesmanFormModel salesman)
        {
            if (!this.data.Vendors.Any(v => v.Id == salesman.VendorId))
            {
                this.ModelState.AddModelError(nameof(salesman.VendorId), "Vendor does not exist.");
            }
            if (!ModelState.IsValid)
            {
                salesman.SalesmanVendors = this.GetVendors();
                return View(salesman);
            }
           

            var userId = this.User.GetId();
            var userIsAlreadySalesman = this.data
                .Salesmen
                .Any(s => s.UserId == userId);

            if (userIsAlreadySalesman)
            {
                return BadRequest();
            }

            var newSalesman = new Salesman
            {
                FirstName = salesman.FirstName,
                LastName = salesman.LastName,
                PhoneNumber = salesman.PhoneNumber,
                UserId = userId,
                VendorId = salesman.VendorId,
                Vendor = this.data
                .Vendors
                .Where(v => v.Id == salesman.VendorId)
                .FirstOrDefault()
            };

            this.data.Salesmen.Add(newSalesman);
            this.data.SaveChanges();

            return RedirectToAction("All", "Items");
        }

        private IEnumerable<SalesmanVendorViewModel> GetVendors()
        {

            return data
                .Vendors
                .Where(v=>v.IsApproved==true)
                .Select(v => new SalesmanVendorViewModel
                {
                    Id = v.Id,
                    Name = v.Name
                })
                .ToList();
        }
    }
}
