using CSSHarris.Models;
using CSSHarris.Models.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CSSHarris.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;

        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Get all users and show them on the page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Username = user.UserName;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            _logger.LogInformation(HttpContext?.User?.Identity?.Name + " entered the user role manage page at " +
           DateTime.UtcNow.ToLongTimeString());

            return View(userRolesViewModel);
        }

        /// <summary>
        /// Get all roles of the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        /// <summary>
        /// Page of the specific users with a list of roles that can be applied
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                if (role.Id is not null || role.Name is not null)
                {
                    var userRolesViewModel = new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRolesViewModel.Selected = true;
                    }
                    else
                    {
                        userRolesViewModel.Selected = false;
                    }
                    model.Add(userRolesViewModel);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Return to index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Cancel()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Update users role
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");

                _logger.LogInformation(HttpContext?.User?.Identity?.Name + " removed roles " + roles
                    + " to " + user.UserName + " at " +
           DateTime.UtcNow.ToLongTimeString());

                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");

                _logger.LogInformation(HttpContext?.User?.Identity?.Name + " added roles " + model.Where(x => x.Selected).Select(y => y.RoleName).ToArray().ToString()
                    + " to " + user.UserName + " at " +
           DateTime.UtcNow.ToLongTimeString());

                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}