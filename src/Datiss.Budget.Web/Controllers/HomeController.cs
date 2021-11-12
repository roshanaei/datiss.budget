using System;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DNTCommon.Web.Core;
using Datiss.Budget.Security;

namespace Datiss.Budget.Controllers
{
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {
        private readonly IUserContext _userContext;

        public HomeController(IUserContext userContext) {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            if (!_userContext.IsAuthenticated)
                return RedirectToAction("Login", "Index", new { area = "Identity" });

            return View();
        }

        [BreadCrumb(Title = "خطا", Order = 1)]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// To test automatic challenge after redirecting from another site
        /// Sample URL: http://localhost:5000/Home/CallBackResult?token=1&status=2&orderId=3&terminalNo=4&rrn=5
        /// </summary>
        [Authorize]
        public IActionResult CallBackResult(long token, string status, string orderId, string terminalNo, string rrn)
        {
            var userId = User.Identity.GetUserId();
            return Json(new { userId, token, status, orderId, terminalNo, rrn });
        }
    }
}