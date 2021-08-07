
using FurniturePlus.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants.Vendor;

namespace FurniturePlus.Models.Vendors
{
    public class AddVendorFormModel
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(VendorNameMaxLength)]
        [Display(Name="Company name")]
        public string Name { get; init; }
        [Required]
        [MaxLength(VendorAddressMaxLength)]
        public string Address { get; init; }
        [Required]
        public string Phone { get; init; }
        [Required]
        public string Email { get; init; }
        [Display(Name = "VAT/Registration No.")]
        public string VATNumber { get; init; }
        public IEnumerable<Item> Items { get; init; }
        public IEnumerable<Salesman> Salesmen { get; init; }

    }
}
