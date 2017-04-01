using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BeardBook.Models;
using BeardBook.Identity;
using BeardBook.Entities;
using Microsoft.Owin.Security;

namespace BeardBook.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region dependencies

        private readonly SignInManager<User, int> _signInManager;
        private readonly AppUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(
            AppUserManager userManager,
            SignInManager<User, int> signInManager,
            IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        #endregion

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated) return RedirectToHome();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindAsync(model.Email, model.Password);
            if (!user?.EmailConfirmed == true)
            {
                ModelState.AddModelError("", "Please check the confirmation email sent to this address.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated) return RedirectToHome();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrorsTo(ModelState, result.Errors);
                return View(model);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Url?.Scheme);

            await _userManager.SendEmailAsync
                (user.Id, "Confirm your account", $"Please confirm your account by clicking the following link:\n{callbackUrl}");

            return View("ConfirmEmailSent");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string token)
        {
            if (Request.IsAuthenticated) return RedirectToHome();
            if (userId <= 0 || token == null) return View("Error");

            try
            {
                var result = await _userManager.ConfirmEmailAsync(userId, token);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            catch (InvalidOperationException)
            {
                return View("Error");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user.Id))
            {
                AddErrorsTo(ModelState, new []{"Please check if the email is correct or check the confirmation email first"});
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, Request.Url?.Scheme);

            await _userManager.SendEmailAsync
                (user.Id, "Reset your password", $"Please reset your password by clicking the following link:\n{callbackUrl}");

            return View("ResetPasswordEmailSent");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(int userId, string token)
        {
            return userId <= 0 || token == null
                ? View("Error")
                : View(new ResetPasswordViewModel {UserId = userId});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ResetPasswordAsync(user.Id, model.Token, model.Password);
            return View(result.Succeeded ? "ResetPasswordConfirmation" : "Error");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId<int>());
                await _signInManager.SignInAsync(user, false, false);
                return RedirectToAction("Index", "Profile");
            }
            AddErrorsTo(ModelState, result.Errors);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }
    }
}