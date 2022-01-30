using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.ViewModels.Identity;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Contracts.Identity;
using Mapster;
using Datiss.Budget.Resources;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class RolesController : Controller {

        public const string Name = "Roles";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Edit = nameof(Edit);

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

            foreach(var claim in model.ClaimTypeSource) {
                model.SelectedClaims.Add(claim.Name, "");
            }

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

            var data = model.Adapt<CreateRoleDTO>();
            var result = await _roleService.CreateAsync(data);
            if(result.NotValid) {
                model.AddError(result.Message);
                model.ClaimTypeSource = (await _claimTypeService.GetEnabledTypesAsync())
                    .Adapt<List<AppClaimTypeViewModel>>();
                return View(model);
            }

            return RedirectToAction(ACTION_Index);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var data = await _roleService.GetByIdAsync(id);
                var model = data.Adapt<UpdateRoleViewModel>();
                foreach (var claim in data.Claims) {
                    model.SelectedClaims.Add(claim.ClaimType, claim.ClaimValue);
                }
                model.ClaimTypeSource = (await _claimTypeService.GetEnabledTypesAsync())
                    .Adapt<List<AppClaimTypeViewModel>>();
                return View(model);
            }
            catch(Exception ex) {
                return NotFound();
            }
        }

        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateRoleViewModel model) 
        {
            model.CheckArgumentIsNull(nameof(model));
            if (!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                model.ClaimTypeSource = (await _claimTypeService.GetEnabledTypesAsync())
                    .Adapt<List<AppClaimTypeViewModel>>();
                return View(model);
            }

            var data = model.Adapt<UpdateRoleDTO>();
            var result = await _roleService.UpdateAsync(data);
            if(result.NotValid) {
                model.AddError(result.Message);
                model.ClaimTypeSource = (await _claimTypeService.GetEnabledTypesAsync())
                    .Adapt<List<AppClaimTypeViewModel>>();
                return View(model);
            }

            return RedirectToAction(ACTION_Index);
        }

    }
}
