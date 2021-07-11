using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.ViewModels.Identity;
using DNTBreadCrumb.Core;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Datiss.Budget.Areas.Identity.Controllers
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2581
    /// </summary>
    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.IdentityArea)]
    [BreadCrumb(Title = "مدیریت نقش‌های پویا", UseDefaultRouteUrl = true, Order = 0)]
    [Route("[area]")]
    public class DynamicRoleClaimsManagerController : Controller
    {
        private readonly IMvcActionsDiscoveryService _mvcActionsDiscoveryService;
        private readonly IApplicationRoleManager _roleManager;

        public DynamicRoleClaimsManagerController(
            IMvcActionsDiscoveryService mvcActionsDiscoveryService,
            IApplicationRoleManager roleManager)
        {
            _mvcActionsDiscoveryService = mvcActionsDiscoveryService ?? throw new ArgumentNullException(nameof(mvcActionsDiscoveryService));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        [Route("manage/claims")]
        public async Task<IActionResult> Index(int? id)
        {
            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "مدیریت نقش‌ها",
                Url = Url.Action("Index", "RolesManager"),
                Order = -1
            });

            if (!id.HasValue)
            {
                return View("Error");
            }

            var role = await _roleManager.FindRoleIncludeRoleClaimsAsync(id.Value);
            if (role == null)
            {
                return View("NotFound");
            }

            var securedControllerActions = _mvcActionsDiscoveryService.GetAllSecuredControllerActionsWithPolicy(ConstantPolicies.DynamicPermission);
            return View(model: new DynamicRoleClaimsManagerViewModel
            {
                SecuredControllerActions = securedControllerActions,
                RoleIncludeRoleClaims = role
            });
        }

        [AjaxOnly, HttpPost("manage/claims"), ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index(DynamicRoleClaimsManagerViewModel model)
        {
            var result = await _roleManager.AddOrUpdateRoleClaimsAsync(
                roleId: model.RoleId,
                roleClaimType: ConstantPolicies.DynamicPermissionClaimType,
                selectedRoleClaimValues: model.ActionIds);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }
            return Json(new { success = true });
        }
    }
}