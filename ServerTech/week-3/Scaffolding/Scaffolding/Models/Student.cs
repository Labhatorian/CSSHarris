using System.ComponentModel.DataAnnotations;

namespace Scaffolding.Models
{
    public class Student
    {
        [Key]
        public int StudentNummer { get; set; }

        public string Voornaam { get; set; }

        public string Achternaam { get; set; }
    }
}
