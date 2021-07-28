using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants;

namespace FurniturePlus.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
