using Microsoft.AspNetCore.Mvc;
using Opdracht_op_eindniveau_les_4.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Diagnostics;

namespace Opdracht_op_eindniveau_les_4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Send()
        {
            Execute().Wait();
            return RedirectToAction(nameof(Index));
        }

        static async Task Execute()
        {
            var client = new SendGridClient("SG.vTmSAfeBS4iO4VAhhqqqvA._tYvJt2c90t4xXJ_29L3wTdU6rVhmhOmL1CEq7m0ttE");
            var from = new EmailAddress("s1168716@student.windesheim.nl", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("s1168716@student.windesheim.nl", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}