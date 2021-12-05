using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Data;
using FurniturePlus.Models.Salesmen;
using FurniturePlus.Infrastructure;
using FurniturePlus.Services.Salesmen;
using FurniturePlus.Services.Vendors;
using FurniturePlus.Data.Models;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class SalesmenController : Controller
    {
        private readonly ISalesmanService salesmen;
        private readonly IVendorService vendors;

        public SalesmenController(ISalesmanService salesmen, IVendorService vendors)
        {
            this.salesmen = salesmen;
            this.vendors = vendors;
        }

        [Authorize]
        public IActionResult RegisterSalesman()
        {
            return View(new RegisterSalesmanFormModel
            {
                SalesmanVendors = this.salesmen.GetVendors()
            });
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult RegisterSalesman(RegisterSalesmanFormModel salesman)
        {
            if (!this.vendors.DoesVendorExist(salesman.VendorId))
            {
                this.ModelState.AddModelError(nameof(salesman.VendorId), "Vendor does not exist.");
            }
            if (!ModelState.IsValid)
            {
                salesman.SalesmanVendors = this.salesmen.GetVendors();
                return View(salesman);
            }
            var userId = this.User.GetId();

            if (this.salesmen.IsUserASalesman(userId))
            {
                return BadRequest("You either have already registered as a salesman or you've sent a request and should wait for an approval from Administrator.");
            }

            this.salesmen.RegisterSalesman(salesman, userId);

            return RedirectToAction("All", "Items");
        }

    }
}
