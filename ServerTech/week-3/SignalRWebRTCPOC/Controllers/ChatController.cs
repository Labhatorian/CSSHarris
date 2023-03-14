using Microsoft.AspNetCore.Mvc;
using SignalRWebRTCPOC.Models;

namespace SignalRWebRTCPOC.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Video()
        {
            return View();
        }
    }
}
