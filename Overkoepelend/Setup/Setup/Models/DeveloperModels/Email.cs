using System.Text.Json;
using System.Text.Json.Serialization;

namespace Setup.Models.DeveloperModels
{
    public class Email
    {
        public string Response { get; set; }

        public string Subject { get; set; }

        public string EmailAddress { get; set; }

        public string Message { get; set; }
    }
}
