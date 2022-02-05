using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class ReportsController : Controller
    {

        public const string Name = "Reports";
        public const string ACTION_Index = nameof(Index);

        [HttpGet("{page?}")]
        public IActionResult Index(int page = 1) {
            
            return View();
        }
    }
}
