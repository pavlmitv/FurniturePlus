using FurniturePlus.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurniturePlus.Services.Items
{
    public interface IItemService
    {
        DetailsViewModel Details(int itemId, bool isEditable);
    }
}
