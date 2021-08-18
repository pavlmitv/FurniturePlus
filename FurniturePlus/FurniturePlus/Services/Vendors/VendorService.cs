using FurniturePlus.Data;

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
    }
}
