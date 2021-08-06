using System.Collections.Generic;

namespace FurniturePlus.Models.Home
{
    public class IndexViewModel
    {
        public int TotalVendors { get; init; }
        public int TotalItems { get; init; }
        public int TotalPurchases { get; init; }

        public IEnumerable<ItemIndexViewModel> Items { get; init; }
    }
}
