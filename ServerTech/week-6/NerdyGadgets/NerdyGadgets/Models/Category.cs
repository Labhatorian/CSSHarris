using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NerdyGadgets.Models
{
    public class Category
    {
        [Key]
        public int CategoryNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<ProductCategory> CategoryProducts { get; set; } //VEEL OP VEEL

        public Category() { 
            CategoryProducts = new List<ProductCategory>();
        }
    }
}