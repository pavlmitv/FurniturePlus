namespace FurniturePlus.Services.Salesmen
{
    public interface ISalesmanService
    {
        void Approve(int salesmanId);
        void Decline(int salesmanId);
    }
}
