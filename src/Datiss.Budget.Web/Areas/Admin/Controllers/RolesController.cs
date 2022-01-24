using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Enum;
using Datiss.Budget.Common;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels.Identity;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Contracts.Identity;
using Mapster;
using Datiss.Budget.Resources;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class RolesController : Controller {

        public const string Name = "Roles";
        public const string ACTION_Index = nameof(Index);


        public RolesController() {

        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }
    }
}
