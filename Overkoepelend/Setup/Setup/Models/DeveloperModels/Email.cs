using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Setup.Models.DeveloperModels
{
    public class Email
    {
        public string Response { get; set; }

        [MaxLength(200)]
        public string Subject { get; set; }

        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string EmailAddress { get; set; }

        [MaxLength(600)]
        public string Message { get; set; }
    }
}
