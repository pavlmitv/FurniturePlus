

using FurniturePlus.Data.Models;

namespace FurniturePlus.Models.Items
{
    public class ItemListingViewModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string PurchaseCode { get; init; }
        public Category Category { get; init; }
        public string ImageUrl { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
