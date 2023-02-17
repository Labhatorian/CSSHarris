using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Setup.Models.DeveloperModels
{
    public class Email
    {
        [Key]
        public int idEmail { get; set; }

        [NotMapped]
        public string Response { get; set; }

        [MaxLength(200)]
        public string Subject { get; set; }

        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string EmailAddress { get; set; }

        [MaxLength(600)]
        public string Message { get; set; }
    }
}