using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using Setup.Models;
using Setup.Models.DeveloperModels;

namespace Setup.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly string CaptchaPrivateKey = "";
        private readonly string EmailPrivateKey = "";

        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        private bool AcceptCaptcha = false;

        private EmailContext db;

        public DevContactController(EmailContext db)
        {
            this.db = db;
        }

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

            //todo secure database
            var allEmails = db.Email.Where(e => e.EmailAddress != null).ToList();

            //TODO put data in database (mongodb?)
            //TODO send email and check response unauthorize too then(or forbid)

            return Ok("Message: " + email.Message + ", Subject: " + email.Subject);
        }

        private async Task VerifyCaptcha(string ResponseUser)
        {
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
                content = content.Replace("\n", "").Replace("\r", "");
                dynamic json = JsonConvert.DeserializeObject(content);

                if ((bool)json["success"])
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