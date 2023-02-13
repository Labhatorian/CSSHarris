using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Setup.Models.DeveloperModels;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web.Helpers;

namespace Setup.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DevContactController : ControllerBase
    {
        private readonly string PrivateKey = "OBFUSCATED";
        private readonly string GoogleCaptchaUrl = "https://www.google.com/recaptcha/api/siteverify";

        //todo verify captcha in html and js too?
        [HttpPost]
        public IActionResult ValidatePost([FromBody] Email email)
        {
            //todo verift captcha
            return Ok("ok");
        }

        public async void VerifyCaptcha(string ResponseUser)
        {
            string content = null;
            using (var client = new HttpClient())
            {
                string requestJson = "{\"secret\": \"" + PrivateKey + "\", \"response\": \"" + ResponseUser + "\"}";
                HttpContent httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(GoogleCaptchaUrl, httpContent);

                content = await response.Content.ReadAsStringAsync();
            }
        }

        public void SendEmail()
        {

        }
    }
}
