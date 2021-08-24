using FurniturePlus.Services.Salesmen;
using FurniturePlus.Services.Vendors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Areas.Admin.Models.Requests;

namespace FurniturePlus.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestsController : Controller
    {
        private readonly IVendorService vendors;
        private readonly ISalesmanService salesmen;

        public RequestsController(IVendorService vendors, ISalesmanService salesmen)
        {
            this.vendors = vendors;
            this.salesmen = salesmen;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Requests()
        {
            var pendingVendors = this.vendors.RequestVendors();
            var pendingSalesmen = this.salesmen.RequestSalesmen();

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
