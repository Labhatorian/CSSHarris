using AngleSharp.Css;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CSSHarris.Controllers
{
    [Authorize(Policy = "RequireModRole")]
    public class StaffController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public StaffController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation(HttpContext.User.Identity.Name + " entered the staff page at " +
           DateTime.UtcNow.ToLongTimeString());

            return View();
        }
    }
}
