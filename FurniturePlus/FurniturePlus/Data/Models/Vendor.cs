using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants.Vendor;

namespace FurniturePlus.Data.Models
{
    //Vendors are the companies who sell furniture via the website
    public class Vendor
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(VendorNameMaxLength)]
        public string Name { get; init; }
        [Required]
        [MaxLength(VendorAddressMaxLength)]
        public string Address { get; init; }
        [Required]
        public string Phone { get; init; }
        [Required]
        public string Email { get; init; }
        public string VATNumber { get; init; }
        public IEnumerable<Item> Items { get; init; }
        public IEnumerable<Salesman> Salesmen { get; init; }


    }
}
