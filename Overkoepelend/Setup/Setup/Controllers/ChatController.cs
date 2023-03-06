using Microsoft.AspNetCore.Mvc;

namespace Setup.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}