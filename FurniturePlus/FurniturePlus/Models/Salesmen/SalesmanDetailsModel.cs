namespace FurniturePlus.Models.Salesmen
{
    public class SalesmanDetailsModel
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string VendorName { get; init; }
        public bool IsApproved { get; set; }
    }
}
