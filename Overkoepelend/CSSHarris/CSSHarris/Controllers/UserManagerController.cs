using CSSHarris.Areas.Identity.Pages.Account.Manage;
using CSSHarris.Models;
using CSSHarris.Models.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Encodings.Web;
using System.Text;

namespace CSSHarris.Controllers
{
    [Authorize(Policy = "RequireModRole")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserManagerController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

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
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> BanUnban(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                RedirectToAction("Index");
            }

            user.Banned = !user.Banned;

            if(user.Banned) { TempData["message"] = user.UserName + " has been banned"; }
            else { TempData["message"] = user.UserName + " has been unbanned"; }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ResetPassword(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null )
            {
                RedirectToAction("Index");
            }
            if (!user.EmailConfirmed)
            {
                TempData["message"] = user.UserName + " has been email verified";
                user.EmailConfirmed = true;
            } else
            {
                TempData["message"] = user.UserName + " has already been email verified";
            }
            

            return RedirectToAction("Index");
        }
    }
}
