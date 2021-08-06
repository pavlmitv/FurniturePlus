using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants.User;

namespace FurniturePlus.Data.Models
{
    public class Salesman    //:IdentityUser   //TODO: does it need to inherit the IdentityUser props?
    {
        public int Id { get; init; }
        [Required]
        [MinLength(FirstLastNameMinLength)]
        [MaxLength(FirstLastNameMaxLength)]
        public string FirstName { get; init; }
        [Required]
        [MinLength(FirstLastNameMinLength)]
        [MaxLength(FirstLastNameMaxLength)]
        public string LastName { get; init; }
        [Required]
        public string PhoneNumber { get; init; }
        [Required]
        public string UserId { get; init; }
        [Required]
        public Vendor Vendor { get; init; }
        [Required]
        public int VendorId { get; init; }


    }
}