using FurniturePlus.Data;
using FurniturePlus.Models.Vendors;
using FurniturePlus.Services.Vendors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class RequestsController : Controller
    {
        private readonly FurniturePlusDbContext data;
        private readonly IVendorService vendors;

        public RequestsController(FurniturePlusDbContext data, IVendorService vendors)
        {
            this.data = data;
            this.vendors = vendors;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Requests()
        {
            var pendingVendors = this.data
                 .Vendors
                 .Where(v => v.IsApproved == false)
                 .Select(v => new VendorDetailsModel
                 {
                     Id = v.Id,
                     Name = v.Name,
                     Address = v.Address,
                     Phone = v.Phone,
                     Email = v.Email,
                     VATNumber = v.VATNumber,
                     IsApproved = v.IsApproved
                 })
                 .ToList();

            return View(pendingVendors);
        }

        public IActionResult Approve(int id)
        {
            this.vendors.Approve(id);
            return RedirectToAction(nameof(Requests), nameof(Requests));
        }

        //[HttpPost]
        //[Authorize(Roles = "Administrator")]
        //[AutoValidateAntiforgeryToken]
        //public IActionResult Approve(VendorDetailsModel vendor, int id)
        //{
        //    var currentVendor = this.data
        //         .Vendors
        //         .FirstOrDefault(v => v.Id == id);

        //    currentVendor.IsApproved = vendor.IsApproved;

        //    this.data.SaveChanges();

        //    return RedirectToAction("Requests", "Requests");

        //}

        //public IActionResult Decline()
        //{

        //}
    }
}
