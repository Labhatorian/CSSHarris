using Microsoft.AspNetCore.Mvc;
using Setup.Models.DeveloperModels;
using System.Text;

namespace Setup.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly string PrivateKey = "OBFUSCATED";
        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        //todo verify captcha in html and js too
        [HttpPost("Validate")]
        public IActionResult ValidatePost([FromBody] Email email)
        {
            email.Subject += " dit tekst is toegevoegd door de server";
            email.Message += " dit tekst is toegevoegd door de server";
            VerifyCaptcha(email.Response);

            return Ok("Message: " + email.Message + ", Subject: " + email.Subject);
        }

        public async void VerifyCaptcha(string ResponseUser)
        {
            //UNDONE
            string content = null;
            using (var client = new HttpClient())
            {
                string requestJson = "{secret: " + PrivateKey + ", response: " + ResponseUser + "}";
                HttpContent httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(GoogleCaptchaUrl, httpContent);

                content = await response.Content.ReadAsStringAsync();
            }
        }
    }
}