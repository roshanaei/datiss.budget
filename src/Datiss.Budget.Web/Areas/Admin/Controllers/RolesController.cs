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

        private readonly IRoleService _roleService;
        private readonly IAppClaimTypeService _claimTypeService;

        public RolesController(
            IRoleService roleService,
            IAppClaimTypeService claimTypeService) {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _claimTypeService = claimTypeService ?? throw new ArgumentNullException(nameof(claimTypeService));
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var roles = (await _roleService.GetAllAsync())
                .Adapt<List<RoleViewModel>>();

            var claimTypes = (await _claimTypeService.GetEnabledTypesAsync())
                .Adapt<List<AppClaimTypeViewModel>>();

            var model = new RolesIndexViewModel(roles, claimTypes);
            
            return View(model);
        }
        
    }
}
