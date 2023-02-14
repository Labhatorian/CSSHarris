using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Setup.Models.DeveloperModels;

namespace Setup.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly string CaptchaPrivateKey = "6LedxnkkAAAAAB7HjANzV8h9lB08kJYYedyaJ3da";
        private readonly string EmailPrivateKey = "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";

        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        private bool AcceptCaptcha = false;

        [HttpPost("Validate")]
        public IActionResult ValidatePost([FromBody] Email email)
        {
            Task captchatask = VerifyCaptcha(email.Response);
            captchatask.Wait();

            if (!AcceptCaptcha)
            {
                //TODO use forbid?
                return Unauthorized();
            }
            //TODO put data in database (mongodb?)
            //TODO send email and check response unauthorize too then(or forbid)

            return Ok("Message: " + email.Message + ", Subject: " + email.Subject);
        }

        private async Task VerifyCaptcha(string ResponseUser)
        {
            //UNDONE
            string content = null;
            AcceptCaptcha = false;
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, GoogleCaptchaUrl);
                req.Headers.Add("Accept", "application/x-www-form-urlencoded");

                req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret", CaptchaPrivateKey },
                    { "response", ResponseUser }
                });

                HttpResponseMessage resp = await client.SendAsync(req);
                content = await resp.Content.ReadAsStringAsync();

                //TODO check code
                if (content != null)
                {
                    AcceptCaptcha = true;
                }
            }
        }

        private async Task SendEmail(Email email)
        {
            var client = new SendGridClient(EmailPrivateKey);
            var from = new EmailAddress(email.EmailAddress);
            var to = new EmailAddress("", "");
            var msg = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Message, "");
            var response = await client.SendEmailAsync(msg);
            //TODO check respones for OK
        }
    }
}