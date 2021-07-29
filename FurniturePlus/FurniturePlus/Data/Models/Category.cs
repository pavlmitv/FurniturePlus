using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FurniturePlus.Data.Models.Constants;

namespace FurniturePlus.Data.Models
{
    public class Category
    {
        public int Id { get; init; }
        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
