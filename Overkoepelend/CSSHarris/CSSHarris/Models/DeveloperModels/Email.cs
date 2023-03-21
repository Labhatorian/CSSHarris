using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSSHarris.Models.DeveloperModels
{
    /// <summary>
    /// Has annontations for Database and validation
    /// </summary>
    public class Email
    {
        [Key]
        public int idEmail { get; set; }

        [NotMapped]
        public string? Response { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        public string? Subject { get; set; }

        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string? EmailAddress { get; set; }

        [MaxLength(600)]
        [StringLength(600)]
        public string? Message { get; set; }
    }
}