using FurniturePlus.Data.Models;
using static FurniturePlus.Data.Models.Constants.User;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FurniturePlus.Models.Salesmen
{
    public class RegisterSalesmanFormModel
    {
        [Required]
        [MinLength(FirstLastNameMinLength)]
        [MaxLength(FirstLastNameMaxLength)]
        [Display(Name ="First name")]
        public string FirstName { get; init; }
        [Required]
        [MinLength(FirstLastNameMinLength)]
        [MaxLength(FirstLastNameMaxLength)]
        [Display(Name = "Last name")]
        public string LastName { get; init; }
        [Required]
        [Display(Name ="Phone number")]
        [RegularExpression(@"\+[0-9]{1,3}[0-9]{10}", ErrorMessage = "The format should be: \" + \" + country code + phone number.")]
        public string PhoneNumber { get; init; }
        [Display(Name ="Vendor")]
        public int VendorId { get; init; }
        public Vendor Vendor { get; init; }
        public IEnumerable<SalesmanVendorViewModel> SalesmanVendors { get; set; }
    }
}