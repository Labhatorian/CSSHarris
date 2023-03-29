using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CijferRegistratie.Models
{
    public class Poging
    {
        [Key]
        public int PogingId { get; set; }

        [Required]
        public int Jaar { get; set; }

        [Required]
        public int Resultaat { get; set; }


        public int VakId { get; set; }
        public Vak? Vak { get; set; }

        public string? StudentType { get; set; }
    }
}