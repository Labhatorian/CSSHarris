using Microsoft.AspNetCore.Mvc;
using Setup.Models.DeveloperModels;

namespace Setup.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly string PrivateKey = "6LedxnkkAAAAAB7HjANzV8h9lB08kJYYedyaJ3da";
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
                var req = new HttpRequestMessage(HttpMethod.Post, GoogleCaptchaUrl);
                req.Headers.Add("Accept", "application/x-www-form-urlencoded");

                req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret", PrivateKey },
                    { "response", ResponseUser }
                });

                HttpResponseMessage resp = await client.SendAsync(req);
                content = await resp.Content.ReadAsStringAsync();
            }
        }
    }
}