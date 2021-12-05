using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Models.Vendors;
using FurniturePlus.Services.Vendors;

namespace FurniturePlus.Controllers
{
    public class VendorsController : Controller
    {
        private readonly IVendorService vendors;
        public VendorsController(IVendorService vendors)
        {
            this.vendors = vendors;
        }

        [Authorize]
        public IActionResult AddVendor()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        //Model binding: ASP.NET core ще попълни модела (AddItemFormModel item) с данните от request-a и ще върне view
        public IActionResult AddVendor(AddVendorFormModel vendor)
        {
            this.vendors.AddVendor(vendor);
            return RedirectToAction("","");
        }
    }
}
