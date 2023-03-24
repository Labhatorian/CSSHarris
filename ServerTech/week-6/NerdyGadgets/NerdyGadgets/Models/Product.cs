using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NerdyGadgets.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(100, ErrorMessage = ValidationMessages.MaxLenghtMessage)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        //[Column("Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } //VEEL OP VEEL
    }
}