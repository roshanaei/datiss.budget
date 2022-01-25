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
        public const string ACTION_Create = nameof(Create);

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
        
        [HttpGet("[action]")]
        public async Task<IActionResult> Create() {
            var model = new CreateRoleViewModel {
                ClaimTypeSource = (await _claimTypeService.GetEnabledTypesAsync())
                    .Adapt<List<AppClaimTypeViewModel>>()
            };

            return View(model);
        }

        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model) {
            if(!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                model.ClaimTypeSource = (await _claimTypeService.GetEnabledTypesAsync())
                    .Adapt<List<AppClaimTypeViewModel>>();
                return View(model);
            }

            var form = HttpContext.Request.Form;
            foreach(var key in form.Keys) {
                if(key.StartsWith(""))
            }

            return View(model);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                return NotFound();
            }
            catch(Exception ex) {
                return NotFound();
            }
        }
    }
}
