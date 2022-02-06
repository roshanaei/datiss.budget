using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Common.GuardToolkit;
using Mapster;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class OrganizationController : Controller
    {
        public const string Name = "Organization";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Delete = nameof(Delete);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly IWebHostEnvironment _env;
        private readonly ISecurityTrimmingService _securityTrimmingService;
        private readonly IOrganizationService _organizationService;

        public OrganizationController(
            IWebHostEnvironment environment,
            ISecurityTrimmingService securityTrimmingService,
            IOrganizationService organizationService)
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new OrganizationFilterDTO();

            var orgSource = (await _organizationService.GetDropDownTypeOrgDataAsync(OrganizationType.County))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var myfilter = TempData.Get<OrganizationFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<OrganizationFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _organizationService.GetListAsync(filter);
            var model = new OrganizationIndexViewModel();
            model = result.Adapt<OrganizationIndexViewModel>();

            model.SetParentOrganizationFilterSource(orgSource,filter.OrganizationId);


            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (OrganizationIndexViewModel model,int page = 1)
        {
            model.Filter.PageNumber = 1;
            var filter = model.Filter.Adapt<OrganizationFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _organizationService.GetListAsync(filter);
            model = result.Adapt<OrganizationIndexViewModel>();
            model.Filter = filter.Adapt<OrganizationFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownTypeOrgDataAsync(OrganizationType.County))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetParentOrganizationFilterSource(orgSource, filter.OrganizationId);
            return View(model);
        }

        [HttpGet("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
        public async Task<IActionResult> Create()
        {
            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var model = new CreateOrganizationViewModel();

            model.SetParentOrganizationSource(orgSource);

            return PartialView("_createModal", model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateOrganizationViewModel model)
        {
            var data = model.Adapt<CreateOrganizationDTO>();

            var result = await _organizationService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result);
        }

        [HttpGet("[action]/{id}")]
        [HasPermission(claimType: Name, PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            var entity = await _organizationService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }
            var model = new UpdateOrganizationViewModel
            {
                Type = entity.Type,
                DisplayOrder = entity.DisplayOrder,
                ParentId = entity.ParentId,
                Title = entity.Title,
                SewageStatus = entity.SewageStatus,
                Enabled = entity.Status == EntityStatus.Enabled ? true : false
            };

            model.SetParentOrganizationSource(orgSource);


            return PartialView("_editModal", model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateOrganizationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateOrganizationDTO>();
            var result = await _organizationService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result);
        }

        [HttpPost("[action]/{id}")]
        [HasPermission(claimType: Name, PermissionActionType.Delete)]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                await _organizationService.SoftDeleteAsync(id);

                return Json(new
                {
                    hasError = false,
                    message = ViewMessages.FinanceYearSuccessSoftDelete
                });
            }
            catch(OrganizationHasChildException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.OrganizationHasChild
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.SystemError
                });
            }
        }
    }
}
