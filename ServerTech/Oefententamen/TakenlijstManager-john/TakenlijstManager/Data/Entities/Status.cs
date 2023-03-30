using System.ComponentModel.DataAnnotations;

namespace TakenlijstManager.Data.Entities
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        public int VolgendeStatus { get; set; }
    }
}
