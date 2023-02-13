using GameShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers
{
    public class GamesController : Controller
    {
        public IActionResult Details()
        {
            var game = new Game() { Id = 1, Title="Minecraft", Description="Mojang????", ReleaseDate=new DateTime(2009,1,1)};

            return View(game);
        }

        public IActionResult Edit(int id)
        {
            var game = new Game() { Id = id, Title = "Minecraft", Description = "Mojang????", ReleaseDate = new DateTime(2009, 1, 1) };

            return View(game);
        }

        [HttpPost]
        public IActionResult Edit(Game game)
        {
            //code to save game in db

            //if succesful:
            return RedirectToAction("details");
        }
    }
}