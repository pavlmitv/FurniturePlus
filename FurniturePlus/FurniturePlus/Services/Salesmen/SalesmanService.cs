using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Models.Salesmen;
using FurniturePlus.Services.Salesmen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FurniturePlus.Infrastructure;
namespace FurniturePlus.Services.Vendors
{
    public class SalesmanService : Controller, ISalesmanService
    {
        private readonly FurniturePlusDbContext data;

        public SalesmanService(FurniturePlusDbContext data)
        {
            this.data = data;
        }
        public List<SalesmanDetailsModel> RequestSalesmen()
        {
            return this.data
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
        }

        public void ApproveSalesman(int salesmanId)
        {
            var salesman = this.data.Salesmen.Find(salesmanId);
            salesman.IsApproved = true;
            this.data.SaveChanges();
        }

        public void DeclineSalesman(int salesmanId)
        {
            var salesman = this.data.Salesmen.Find(salesmanId);
            this.data.Salesmen.Remove(salesman);

            this.data.SaveChanges();
        }

        public IEnumerable<SalesmanVendorViewModel> GetVendors()
        {
            return this.data
                .Vendors
                .Where(v => v.IsApproved == true)
                .Select(v => new SalesmanVendorViewModel
                {
                    Id = v.Id,
                    Name = v.Name
                })
                .ToList();
        }

        public bool IsUserASalesman(string userId)
        {
            return this.data
                .Salesmen
                .Any(s => s.UserId == userId);
        }

    }
}
