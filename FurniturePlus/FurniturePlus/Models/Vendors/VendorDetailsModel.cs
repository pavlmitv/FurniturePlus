namespace FurniturePlus.Models.Vendors
{
    public class VendorDetailsModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Address { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
        public string VATNumber { get; init; }
        public bool IsApproved { get; init; }
    }
}
