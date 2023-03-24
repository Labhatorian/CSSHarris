using CSSHarris.Models;
using CSSHarris.Models.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;

namespace CSSHarris.Controllers
{
    [Authorize(Policy = "RequireModRole")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;

        public UserManagerController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        /// <summary>
        /// Index page that gets all users and shows them in a list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserManageViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserManageViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Username = user.UserName;
                thisViewModel.Email = user.Email;
                thisViewModel.Banned = user.Banned;
                thisViewModel.Verified = user.EmailConfirmed;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            _logger.LogInformation(HttpContext?.User?.Identity?.Name + " entered the usermanage page at " +
           DateTime.UtcNow.ToLongTimeString());

            return View(userRolesViewModel);
        }

        /// <summary>
        /// Get the roles of the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List<string></returns>
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        /// <summary>
        /// Bans or Unbans the user depending on their status
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> BanUnban(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                RedirectToAction("Index");
            }

            user.Banned = !user.Banned;
            await _userManager.UpdateAsync(user);

            if (user.Banned) { TempData["message"] = user.UserName + " has been banned"; }
            else { TempData["message"] = user.UserName + " has been unbanned"; }

            _logger.LogInformation(HttpContext?.User?.Identity?.Name + " performed ban/unban on " + user.UserName + " at " +
           DateTime.UtcNow.ToLongTimeString());

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Resets the user's password and sends them an email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ResetPassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.Email is null)
            {
                RedirectToAction("Index");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
            pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "We have reset your password for you",
                $"Our team at MHeet have reset your password as you may or may not have been compromised. <br>" +
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            TempData["message"] = "Password for " + user.UserName + " has been reset";

            _logger.LogInformation(HttpContext?.User?.Identity?.Name + " performed password reset on " + user.UserName + " at " +
           DateTime.UtcNow.ToLongTimeString());

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verifies the users email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> VerifyUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                RedirectToAction("Index");
            }
            if (!user.EmailConfirmed)
            {
                TempData["message"] = user.UserName + " has been email verified";
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                TempData["message"] = user.UserName + " has already been email verified";
            }

            _logger.LogInformation(HttpContext?.User?.Identity?.Name + " performed verify on " + user.UserName + " at " +
           DateTime.UtcNow.ToLongTimeString());

            return RedirectToAction("Index");
        }
    }
}