using System.ComponentModel.DataAnnotations.Schema;

namespace NerdyGadgets.Models
{
    public class ProductCategory
    {
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}