using Ganss.Xss;
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
        private readonly IConfiguration Configuration;

        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        private bool AcceptCaptcha = false;
        private bool AcceptEmail = false;

        private EmailContext db;

        public DevContactController(EmailContext db, IConfiguration configuration)
        {
            this.db = db;
            Configuration = configuration;
        }

        [HttpPost("Validate")]
        public IActionResult ValidatePost([FromBody] Email email)
        {
            //Prevent XSS
            var sanitizer = new HtmlSanitizer();
            email.Message = sanitizer.Sanitize(email.Message);
            email.Subject = sanitizer.Sanitize(email.Subject);

            Task captchatask = VerifyCaptcha(email.Response);
            captchatask.Wait();

            if (!AcceptCaptcha) return Unauthorized(); //TODO use forbid?

            //todo secure database
            //todo use migrations?

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
                    { "secret", Configuration["SecretKeys:CaptchaSecret"] },
                    { "response", ResponseUser }
                });

            HttpResponseMessage resp = await client.SendAsync(req);
            AcceptCaptcha = (bool)resp.IsSuccessStatusCode;
        }

        private async Task SendEmail(Email email)
        {
            AcceptEmail = false;

            MailjetClient client = new MailjetClient(Configuration["PublicKeys:MailJetPublicKey"], Configuration["SecretKeys:MailJetSecret"]);

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