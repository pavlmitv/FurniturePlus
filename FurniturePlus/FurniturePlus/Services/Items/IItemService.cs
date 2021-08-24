using FurniturePlus.Models.Home;
using FurniturePlus.Models.Items;
using System.Collections.Generic;

namespace FurniturePlus.Services.Items
{
    public interface IItemService
    {
        DetailsViewModel Details(int itemId, bool isEditable);
        int ItemsCount();
        List<ItemIndexViewModel> GetAllItemsForHomePage();
    }
}
