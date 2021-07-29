
using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants;

namespace FurniturePlus.Data.Models
{
    public class User
    {
        public int Id { get; init; }
        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; }
        [Required]
        [MaxLength(PasswordMaxLength)]
        [MinLength(PasswordMinLength)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

    }
}