using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants;

namespace FurniturePlus.Data.Models
{
    //Name of the company who sells the products
    public class Vendor
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(VendorNameMaxLength)]
        public string Name { get; set; }
        [Required]
        [MaxLength(VendorAddressMaxLength)]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Salesman Salesman { get; set; }

    }
}
