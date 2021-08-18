using FurniturePlus.Data;
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

        public void Approve(int vendorId)
        {
            var vendor = this.data.Vendors.Find(vendorId);
            vendor.IsApproved = true;
            this.data.SaveChanges();
        }

        public void Decline(int vendorId)
        {
            var vendor = this.data.Vendors.Find(vendorId);
            this.data.Vendors.Remove(vendor);

            this.data.SaveChanges();
        }
    }
}
