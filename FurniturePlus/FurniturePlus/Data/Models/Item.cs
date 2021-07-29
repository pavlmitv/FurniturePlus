using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants;

namespace FurniturePlus.Data.Models
{
    public class Item
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(ItemNameMaxLength)]
        public string Name { get; set; }
        //automatically generated: first 3 letters from Vendor + sequence number;
        [Required]
        public int PurchaseCode { get; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public Vendor Vendor { get; set; }
        public string ImageUrl { get; set; }
        [MaxLength(ItemDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
