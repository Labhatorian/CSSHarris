using System.ComponentModel.DataAnnotations;

namespace CijferRegistratie.Models
{
    public class Vak
    {
        [Key]
        public int VakId { get; set; }

        [Required]
        [MinLength(6)]
        public string? Naam { get; set; }

        [Required]
        public int EC { get; set; }

        public ICollection<Poging> Pogingen { get; set; }
    }
}