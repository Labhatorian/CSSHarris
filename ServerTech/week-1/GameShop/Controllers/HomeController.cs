using GameShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameShop.Controllers
{
    public class HomeController : Controller
    {
<<<<<<<< HEAD:ServerTech/week-1/GameShop/Controllers/HomeController.cs
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
========
        private const string PageViews = "PageViews";
        private DeveloperViewModel developer = new();
>>>>>>>> 54d668273f4a0e9f814c243107ff3ada037718c5:Overkoepelend/Setup/Setup/Controllers/HomeController.cs

        public IActionResult Index()
        {
            UpdatePageViewCookie();
            return View();
        }

        public IActionResult Privacy()
        {
            UpdatePageViewCookie();
            return View();
        }

        public IActionResult Developer()
        {
            UpdatePageViewCookie();
            return View(developer);
        }

        public IActionResult Contact()
        {
            UpdatePageViewCookie();
            return View(developer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            UpdatePageViewCookie();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
<<<<<<<< HEAD:ServerTech/week-1/GameShop/Controllers/HomeController.cs
========

        //TOdo dont do this when cookie not accepted
        public void UpdatePageViewCookie()
        {
            var currentCookieValue = Request.Cookies[PageViews];

            if (currentCookieValue == null)
            {
                Response.Cookies.Append(PageViews, "1");
            }
            else
            {
                var newCookieValue = short.Parse(currentCookieValue) + 1;

                Response.Cookies.Append(PageViews, newCookieValue.ToString());
            }
        }
>>>>>>>> 54d668273f4a0e9f814c243107ff3ada037718c5:Overkoepelend/Setup/Setup/Controllers/HomeController.cs
    }
}