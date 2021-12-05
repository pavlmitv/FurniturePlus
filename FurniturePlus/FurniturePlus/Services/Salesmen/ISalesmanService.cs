using FurniturePlus.Data.Models;
using FurniturePlus.Models.Salesmen;
using System.Collections.Generic;

namespace FurniturePlus.Services.Salesmen
{
    public interface ISalesmanService
    {
        void ApproveSalesman(int salesmanId);
        void DeclineSalesman(int salesmanId);
        List<SalesmanDetailsModel> RequestSalesmen();
        IEnumerable<SalesmanVendorViewModel> GetVendors();
        bool IsUserASalesman(string userId);
        bool IsSalesmanApproved(string userId);
    }
}
