using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.ViewModels.Identity;
using Datiss.Budget.ViewModels.Identity.Emails;
using Datiss.Budget.ViewModels.Identity.Settings;
using DNTBreadCrumb.Core;
using DNTCaptcha.Core;
using DNTCommon.Web.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Datiss.Budget.Areas.Identity.Controllers
{
    [Area(AreaConstants.IdentityArea)]
    [AllowAnonymous]
    [BreadCrumb(Title = "ثبت نام", UseDefaultRouteUrl = true, Order = 0)]
    [Route("[area]/[controller]/[action]")]
    public class RegisterController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterController> _logger;
        private readonly IApplicationUserManager _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUserValidator<User> _userValidator;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public RegisterController(
            IApplicationUserManager userManager,
            IPasswordValidator<User> passwordValidator,
            IUserValidator<User> userValidator,
            IEmailSender emailSender,
            IOptionsSnapshot<SiteSettings> siteOptions,
            ILogger<RegisterController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
            _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _siteOptions = siteOptions ?? throw new ArgumentNullException(nameof(siteOptions));
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateUsername(string username, string email)
        {
            var result = await _userValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username, Email = email });
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidatePassword(string password, string username)
        {
            var result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username }, password);
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }

        [BreadCrumb(Title = "تائید ایمیل", Order = 1)]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

        [BreadCrumb(Title = "تائیدیه ایمیل", Order = 1)]
        public IActionResult ConfirmedRegisteration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Language.Persian,
        //                    CaptchaGeneratorDisplayMode = DisplayMode.SumOfTwoNumbers)]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(3, $"{user.UserName} created a new account with password.");

                    if (_siteOptions.Value.EnableEmailConfirmation)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //ControllerExtensions.ShortControllerName<RegisterController>(), //todo: use everywhere .................

                        await _emailSender.SendEmailAsync(
                           email: user.Email,
                           subject: "لطفا اکانت خود را تائید کنید",
                           viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                           model: new RegisterEmailConfirmationViewModel
                           {
                               User = user,
                               EmailConfirmationToken = code,
                               EmailSignature = _siteOptions.Value.Smtp.FromName,
                               MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                           });

                        return RedirectToAction(nameof(ConfirmYourEmail));
                    }
                    return RedirectToAction(nameof(ConfirmedRegisteration));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [BreadCrumb(Title = "ایمیل خود را تائید کنید", Order = 1)]
        public IActionResult ConfirmYourEmail()
        {
            return View();
        }
    }
}