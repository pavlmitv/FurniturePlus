using FurniturePlus.Models.Home;
using FurniturePlus.Models.Items;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FurniturePlus.Services.Items
{
    public interface IItemService
    {
        DetailsViewModel Details(int itemId, string currentUserId, bool isSalesman);
        ItemSearchModel All([FromQuery] ItemSearchModel query);
        int ItemsCount();
        List<ItemIndexViewModel> GetAllItemsForHomePage();
    }
}
