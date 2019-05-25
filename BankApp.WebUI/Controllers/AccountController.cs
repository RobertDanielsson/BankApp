using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BankApp.WebUI.Models;
using BankApp.Persistence.Identity;
using BankApp.Application.Interfaces;
using BankApp.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    [Authorize]
    //[Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var claim = new Claim(model.RoleId, "");
                    await _userManager.AddClaimAsync(user, claim);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

        public async Task<IActionResult> AccessDenied(string returnUrl)
        {
            return View();
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UserList([FromServices] BankAppDbContext context)
        {
            var model = await (from u in context.Users
                               join ur in context.UserClaims on u.Id equals ur.UserId
                               select new SelectListItem
                               {
                                   Value = u.Id,
                                   Text = u.Email + "*" + ur.ClaimType
                               }).ToListAsync();

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateUser(string userId, string oldClaim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var model = new UpdateUserViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
                OldClaim = oldClaim
            };

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model, [FromServices] BankAppDbContext context)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var user = await context.Users.FindAsync(model.UserId);

                var claimToRemove = new Claim(model.OldClaim, "");

                if (model.Password != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);

                    if (result.Succeeded)
                    {
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }
                }
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                await _userManager.RemoveClaimAsync(user, claimToRemove);
                await _userManager.AddClaimAsync(user, new Claim(model.RoleId, ""));
                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return RedirectToAction("userlist");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update employee");
                    return View(model);
                }
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string userId, [FromServices] BankAppDbContext context)
        {
            var user = await context.Users.FindAsync(userId);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["successMessage"] = "Employee successfully deleted";
                return RedirectToAction("UserList");
            }
            else
            {
                TempData["errorMessage"] = "Error deleting employee";
                return RedirectToAction("UserList");
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
