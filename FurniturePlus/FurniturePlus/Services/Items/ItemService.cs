using FurniturePlus.Data;
using FurniturePlus.Models.Items;
using System.Linq;

namespace FurniturePlus.Services.Items
{
    public class ItemService : IItemService
    {
        private readonly FurniturePlusDbContext data;

        public ItemService(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        public DetailsViewModel Details(int itemId)
        {
            return this.data
                    .Items
                    .Where(i => i.Id == itemId)
                    .Select(i => new DetailsViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        PurchaseCode = i.PurchaseCode,
                        ImageUrl = i.ImageUrl,
                        Description = i.Description,
                        Price = i.Price
                    })
                    .FirstOrDefault();
        }
    }
}
