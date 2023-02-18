using Microsoft.AspNetCore.Mvc;
using Setup.Models;
using System.Diagnostics;

namespace Setup.Controllers
{
    public class HomeController : Controller
    {
        private const string PageViews = "PageViews";
        private readonly DeveloperViewModel developer = new();

        /// <summary>
        /// Main page, the index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            UpdatePageViewCookie();
            return View();
        }

        /// <summary>
        /// Privacy page but is empty for now
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            UpdatePageViewCookie();
            return View();
        }

        /// <summary>
        /// Developer profile page
        /// </summary>
        /// <returns></returns>
        public IActionResult Developer()
        {
            UpdatePageViewCookie();
            return View(developer);
        }

        /// <summary>
        /// Contact page
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            UpdatePageViewCookie();
            return View(developer);
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            UpdatePageViewCookie();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Updates pageview cookie and checks for GDPR
        /// </summary>
        public void UpdatePageViewCookie()
        {
            var currentCookieValue = Request.Cookies[PageViews];
            var acceptedGDRP = Request.Cookies["gdpr"];

            if (acceptedGDRP is null || !acceptedGDRP.Equals("accept"))
            {
                //Delete cookie if it exists and return
                Response.Cookies.Delete(PageViews);
                return;
            }
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
    }
}