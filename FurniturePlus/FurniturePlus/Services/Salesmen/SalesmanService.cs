using FurniturePlus.Data;
using FurniturePlus.Services.Salesmen;

namespace FurniturePlus.Services.Vendors
{
    public class SalesmanService : ISalesmanService
    {
        private readonly FurniturePlusDbContext data;

        public SalesmanService(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        public void Approve(int salesmanId)
        {
            var salesman = this.data.Salesmen.Find(salesmanId);
            salesman.IsApproved = true;
            this.data.SaveChanges();
        }

        public void Decline(int salesmanId)
        {
            var salesman = this.data.Salesmen.Find(salesmanId);
            this.data.Salesmen.Remove(salesman);

            this.data.SaveChanges();
        }
    }
}
