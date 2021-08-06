
using FurniturePlus.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurniturePlus.Models.Items
{
    public class ItemSearchModel
    {
        public const int ItemsPerPage = 2;
        public int CurrentPage { get; init; } = 1;
        [Display(Name = "Search by category:")]
        public string CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        [Display(Name = "Search by keyword:")]
        public string SearchTerm { get; set; }
        [Display(Name = "Sort by:")]
        public ItemSorting Sorting { get; init; }
        public int ItemsCount { get; set; }
        public IEnumerable<ItemListingViewModel> Items { get; set; }
    }

}
