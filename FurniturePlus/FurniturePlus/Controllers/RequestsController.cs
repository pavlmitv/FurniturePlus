using FurniturePlus.Data;
using FurniturePlus.Models.Requests;
using FurniturePlus.Models.Salesmen;
using FurniturePlus.Models.Vendors;
using FurniturePlus.Services.Salesmen;
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
        private readonly ISalesmanService salesmen;

        public RequestsController(FurniturePlusDbContext data, IVendorService vendors, ISalesmanService salesmen)
        {
            this.data = data;
            this.vendors = vendors;
            this.salesmen = salesmen;
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

            var pendingSalesmen = this.data
                .Salesmen
                .Where(s => s.IsApproved == false)
                .Select(s => new SalesmanDetailsModel
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    PhoneNumber = s.PhoneNumber,
                    VendorName = s.Vendor.Name,
                    IsApproved = s.IsApproved
                })
                .ToList();

            var request = new RequestsViewModel
            {
                Salesmen = pendingSalesmen,
                Vendors = pendingVendors
            };

            return View(request);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ApproveVendor(int id)
        {
            this.vendors.ApproveVendor(id);
            return RedirectToAction(nameof(Requests), nameof(Requests));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult DeclineVendor(int id)
        {
            this.vendors.DeclineVendor(id);
            return RedirectToAction(nameof(Requests), nameof(Requests));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ApproveSalesman(int id)
        {
            this.salesmen.ApproveSalesman(id);
            return RedirectToAction(nameof(Requests), nameof(Requests));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult DeclineSalesman(int id)
        {
            this.salesmen.DeclineSalesman(id);
            return RedirectToAction(nameof(Requests), nameof(Requests));
        }
    }
}
