using FurniturePlus.Data;
using FurniturePlus.Models.Vendors;
using System.Collections.Generic;
using System.Linq;

namespace FurniturePlus.Services.Vendors
{
    public class VendorService : IVendorService
    {
        private readonly FurniturePlusDbContext data;

        public VendorService(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        public List<VendorDetailsModel> RequestVendors()
        {
           return this.data
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
        }
        public void ApproveVendor(int vendorId)
        {
            var vendor = this.data.Vendors.Find(vendorId);
            vendor.IsApproved = true;
            this.data.SaveChanges();
        }

        public void DeclineVendor(int vendorId)
        {
            var vendor = this.data.Vendors.Find(vendorId);
            this.data.Vendors.Remove(vendor);

            this.data.SaveChanges();
        }

        public int VendorsCount()
        {
            return this.data.Vendors.Where(v=>v.IsApproved==true).Count();
        }

        public bool DoesVendorExist(int salesmanVendorId)
        {
            return this.data.Vendors.Any(v => v.Id == salesmanVendorId);
        }
    }
}
