using FurniturePlus.Models.Salesmen;
using FurniturePlus.Models.Vendors;
using System.Collections.Generic;

namespace FurniturePlus.Areas.Admin.Models.Requests
{
    public class RequestsViewModel
    {
        public IEnumerable<SalesmanDetailsModel> Salesmen { get; set; }
        public IEnumerable<VendorDetailsModel> Vendors { get; set; }
    }
}
