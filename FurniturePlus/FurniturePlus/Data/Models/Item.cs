using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FurniturePlus.Data.Models.Constants.Item;

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
        
        public string PurchaseCode { get; set; }
        [Required]
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public Vendor Vendor { get; init; }
        [Required]
        public int VendorId { get; init; }
        public string ImageUrl { get; set; }
        [MaxLength(ItemDescriptionMaxLength)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName ="decimal(7,2)")]
        public decimal Price { get; set; }
       
    }
}
