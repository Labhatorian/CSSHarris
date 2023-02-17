using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Mvc;
using Setup.Models;
using Setup.Models.DeveloperModels;

namespace Setup.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly string CaptchaPrivateKey = "";
        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        private bool AcceptCaptcha = false;
        private bool AcceptEmail = false;

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

            if (!AcceptCaptcha) return Unauthorized(); //TODO use forbid?

            //todo secure database

            db.Add(email);
            db.SaveChanges();

            SendEmail(email).Wait();

            if (!AcceptEmail) return Unauthorized(); //TODO use forbid?

            return Ok("Message: " + email.Message + ", Subject: " + email.Subject);
        }

        private async Task VerifyCaptcha(string ResponseUser)
        {
            AcceptCaptcha = false;

            using HttpClient client = new();
            var req = new HttpRequestMessage(HttpMethod.Post, GoogleCaptchaUrl);
            req.Headers.Add("Accept", "application/x-www-form-urlencoded");

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret", CaptchaPrivateKey },
                    { "response", ResponseUser }
                });

            HttpResponseMessage resp = await client.SendAsync(req);
            AcceptCaptcha = (bool)resp.IsSuccessStatusCode;
        }

        private async Task SendEmail(Email email)
        {
            AcceptEmail = false;

            MailjetClient client = new MailjetClient("36be74da033eb5679d2a9d9901c79dbd", "");

            // construct your email with builder
            var emailtosend = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("s1168716@student.windesheim.nl"))
                   .WithSubject(email.Subject)
                   .WithHtmlPart(email.Message + "<br><br>" + "Dit email is afkomstig van: " + email.EmailAddress)
                   .WithTo(new SendContact("s1168716@student.windesheim.nl"))
                   .Build();

            // invoke API to send email
            var response = await client.SendTransactionalEmailAsync(emailtosend);

            AcceptEmail = response.Messages[0].Errors is null ? true : false;
        }
    }
}