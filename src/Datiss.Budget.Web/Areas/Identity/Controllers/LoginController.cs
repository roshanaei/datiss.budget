using Datiss.Budget.Resources;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.ViewModels.Identity;
using Datiss.Budget.ViewModels.Identity.Settings;
using DNTBreadCrumb.Core;
using DNTCaptcha.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Datiss.Budget.Areas.Identity.Controllers
{
    [Area(AreaConstants.IdentityArea)]
    [AllowAnonymous]
    [BreadCrumb(Title = "ورود به سیستم", UseDefaultRouteUrl = true, Order = 0)]
    [Route("[area]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IApplicationUserManager _userManager;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public LoginController(
            IApplicationSignInManager signInManager,
            IApplicationUserManager userManager,
            IOptionsSnapshot<SiteSettings> siteOptions,
            ILogger<LoginController> logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [NoBrowserCache]
        [HttpGet("login")]
        public IActionResult Index(string returnUrl = null)
        {
            var model = new LoginViewModel();
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        //[ValidateDNTCaptcha(CaptchaGeneratorLanguage = Language.Persian,
        //                    CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if(!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) {
                model.AddError(ViewMessages.InvalidUsernameOrPassword);
                return View(model);
            }

            if (!user.IsActive) {
                model.AddError(ViewMessages.UserDisabled);
                return View(model);
            }

            if (_siteOptions.Value.EnableEmailConfirmation &&
                    !await _userManager.IsEmailConfirmedAsync(user)) {
                model.AddError(ViewMessages.UserEmailNotConfirmed);
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                                    model.Username,
                                    model.Password,
                                    model.RememberMe,
                                    lockoutOnFailure: true);

            if (result.RequiresTwoFactor) {
                return RedirectToAction(
                    nameof(TwoFactorController.SendCode),
                    "TwoFactor",
                    new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }
            else if (result.IsLockedOut) {
                return View("~/Areas/Identity/Views/TwoFactor/Lockout.cshtml");
            }
            else if (result.IsNotAllowed) {
                model.AddError(ViewMessages.LoginNotAllowed);
                return View(model);
            }
            else if (result.Succeeded) {
                _logger.LogInformation(1, $"{model.Username} logged in.");
                if (returnUrl == "/Identity")
                    returnUrl = null;
                if (Url.IsLocalUrl(returnUrl)) {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            model.AddError(ViewMessages.InvalidUsernameOrPassword);
            return View(model);
        }

        [Route("logOff")]
        public async Task<IActionResult> LogOff()
        {
            var user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;
            await _signInManager.SignOutAsync();
            if (user != null)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                _logger.LogInformation(4, $"{user.UserName} logged out.");
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}