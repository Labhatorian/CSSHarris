﻿using Microsoft.AspNetCore.Mvc;
using Setup.Models;
using System.Diagnostics;

namespace Setup.Controllers
{
    public class HomeController : Controller
    {
        private const string PageViews = "PageViews";
        private DeveloperViewModel developer = new();

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
    }
}