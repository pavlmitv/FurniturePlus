
using FurniturePlus.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FurniturePlus.Data.Models.Constants.Item;

namespace FurniturePlus.Models.Items
{
    public class AddItemFormModel
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(ItemNameMaxLength)]
        public string Name { get; init; }
        [Display(Name = "Purchase code")]
        public string PurchaseCode { get; init; }
        public Category Category { get; init; }
        [Display(Name = "Category")]
        public int CategoryId { get; init; }
        public Vendor Vendor { get; init; }
        [Display(Name = "Preview image URL")]
        [Url]
        public string ImageUrl { get; init; }
        [MaxLength(ItemDescriptionMaxLength)]
        public string Description { get; init; }
        [Required]
        [Column(TypeName = "decimal(7,2)")]     //is this applicable here or only for the database validations?
        public decimal Price { get; init; }
        public IEnumerable<ItemCategoryViewModel> ItemCategories { get; set; }
        public bool DoesCategoryExist { get; set; }


    }
}
