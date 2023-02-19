using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opdracht_op_eindniveau_les_4.Models
{
    public class Product
    {
        public int ArticleNumber { get; set; }

        [ForeignKey("Id")]
        public int CategoryId { get; set; }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}