using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakenlijstManager.Data.Entities
{
    public class Taak
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength =3)]
        public string Naam { get; set; }

        public int Omvang { get; set; }


        [Range(0, 10)]
        public int Prioriteit { get; set; }


        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        public Status? Status { get; set; }
    }
}
