using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSSHarris.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;

        public RoleManagerController(RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        /// <summary>
        /// Adds role to the use and logs it
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }

            _logger.LogInformation("Role " + roleName + " added by " + HttpContext?.User?.Identity?.Name + "at" +
            DateTime.UtcNow.ToLongTimeString());

            return RedirectToAction("Index");
        }
    }
}