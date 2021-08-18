using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Models.Vendors;


namespace FurniturePlus.Controllers
{
    public class VendorsController : Controller
    {
        private readonly FurniturePlusDbContext data;
        public VendorsController(FurniturePlusDbContext data)
        {
            this.data = data;
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

            var newVendor = new Vendor
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Address = vendor.Address,
                Phone = vendor.Phone,
                Email = vendor.Email,
                VATNumber = vendor.VATNumber,
                IsApproved = false
            };
            this.data.Vendors.Add(newVendor);
            this.data.SaveChanges();
            return RedirectToAction("","");
        }
    }
}
