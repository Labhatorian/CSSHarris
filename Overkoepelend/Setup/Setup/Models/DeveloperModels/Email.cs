using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Setup.Models.DeveloperModels
{
    [Keyless]
    public class Email
    {
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
