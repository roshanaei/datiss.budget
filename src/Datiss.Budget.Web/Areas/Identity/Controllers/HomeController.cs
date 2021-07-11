using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Datiss.Budget.Areas.Identity.Controllers
{
    [Area(AreaConstants.IdentityArea)]
    [Authorize]
    [BreadCrumb(Title = "خانه", UseDefaultRouteUrl = true, Order = 0)]
    [Route("[area]")]
    public class HomeController : Controller
    {
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index() => View();
    }
}