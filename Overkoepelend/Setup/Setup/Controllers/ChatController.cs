using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Setup.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        [AllowAnonymous]
        public IActionResult Global()
        {
            return View();
        }
        
        public IActionResult Friends()
        {
            return View();
        }
    }
}