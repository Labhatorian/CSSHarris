using CSSHarris.Data;
using CSSHarris.Models.DeveloperModels;
using Ganss.Xss;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Mvc;


namespace CSSHarris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ApplicationDbContext db;

        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        private bool AcceptCaptcha = false;
        private bool AcceptEmail = false;

        /// <summary>
        /// Constructor that gets <see cref="EmailContext"/> and <paramref name="configuration"/>.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="configuration"></param>
        public DevContactController(ApplicationDbContext db, IConfiguration configuration)
        {
            this.db = db;
            Configuration = configuration;
        }

        /// <summary>
        /// When /api/DevContact/Contact gets posted, cast body data to <see cref="Email"/>
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("Contact")]
        public IActionResult ContactDeveloper([FromBody] Email email)
        {
            if (email is null) return BadRequest();

            //Prevent XSS
            var sanitizer = new HtmlSanitizer();
            if (email.Message is not null) email.Message = sanitizer.Sanitize(email.Message);
            if (email.Subject is not null) email.Subject = sanitizer.Sanitize(email.Subject);

            if (email.Response is not null)
            {
                Task captchatask = VerifyCaptcha(email.Response);
                captchatask.Wait();
            }

            if (!AcceptCaptcha) return StatusCode(403, 0);

            db.Add(email);
            db.SaveChanges();

            SendEmail(email).Wait();

            if (!AcceptEmail) return StatusCode(403, 1);

            return Ok("{\"Email\": \"" + email.EmailAddress + "\", \"Subject\": \"" + email.Subject + "\", \"Message\": \"" + email.Message + "\"}");
        }

        /// <summary>
        /// Captcha will be done server side for better security
        /// Verifies captcha from Google
        /// </summary>
        /// <param name="ResponseUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends email with <see cref="MailjetClient"/>
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task SendEmail(Email email)
        {
            AcceptEmail = false;

            MailjetClient client = new(Configuration["PublicKeys:MailJetPublicKey"], Configuration["SecretKeys:MailJetSecret"]);

            // construct your email with builder
            var emailtosend = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("s1168716@student.windesheim.nl"))
                   .WithSubject(email.Subject)
                   .WithHtmlPart(email.Message + "<br><br>" + "Dit email is afkomstig van: " + email.EmailAddress)
                   .WithTo(new SendContact("s1168716@student.windesheim.nl"))
                   .Build();

            // invoke API to send email
            var response = await client.SendTransactionalEmailAsync(emailtosend);

            AcceptEmail = response.Messages[0].Errors is null;
        }
    }
}