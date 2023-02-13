using System.Text.Json;
using System.Text.Json.Serialization;

namespace Setup.Models.DeveloperModels
{
    public class Email
    {
        [JsonPropertyName("Response")]
        public string Response { get; set; }

        [JsonPropertyName("Subject")]
        public string Subject { get; set; }

        [JsonPropertyName("EmailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("Message")]
        [Json]
        public string Message { get; set; }
    }
}
