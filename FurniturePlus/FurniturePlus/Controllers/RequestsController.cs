using FurniturePlus.Data;
using FurniturePlus.Models.Vendors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FurniturePlus.Controllers
{
    public class RequestsController : Controller
    {
        private readonly FurniturePlusDbContext data;

        public RequestsController(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        [Authorize(Roles ="Administrator")]
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

        [HttpPost]
        [Authorize(Roles ="Administrator")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Approve( int id)
        {
            this.data
                .Vendors
                .FirstOrDefault(v => v.Id == id)
                .IsApproved = true;

            this.data.SaveChanges();

            return RedirectToAction("Requests","Requests");

        }
        //public IActionResult Decline()
        //{

        //}
    }
}
