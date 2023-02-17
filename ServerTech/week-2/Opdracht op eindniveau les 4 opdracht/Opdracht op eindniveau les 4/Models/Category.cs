using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Opdracht_op_eindniveau_les_4.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}