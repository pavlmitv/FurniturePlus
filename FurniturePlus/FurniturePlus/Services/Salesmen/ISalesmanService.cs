namespace FurniturePlus.Services.Salesmen
{
    public interface ISalesmanService
    {
        void ApproveSalesman(int salesmanId);
        void DeclineSalesman(int salesmanId);
    }
}
