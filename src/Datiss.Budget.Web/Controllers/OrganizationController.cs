
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

namespace Datiss.Budget.Web.Controllers
{

    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]/[action]")]
    public class OrganizationController : Controller
    {
        public const string Name = "Organization";
        //public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        //public const string ACTION_Edit = nameof(Edit);
        //public const string ACTION_Delete = nameof(Delete);


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
        public  async Task<IActionResult> Index(int page = 1)
        {
            var orgSource = (await _organizationService.GetDropDownTypeOrgDataAsync(OrganizationType.County))
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            var filterInput = new OrganizationFilterDTO {
                OrderBy = "id",
                OrderDesc = true,
                PageNumber = page
            };

            var result = await _organizationService.GetListAsync(filterInput);
            var model = new OrganizationIndexViewModel();
            model = result.Adapt<OrganizationIndexViewModel>();

            //Fill DropDown
            model.SetParentOrganizationFilterSource(orgSource);


            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (OrganizationIndexViewModel viewModel,int page = 1)
        {
            if(Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = viewModel.Filter.Adapt<OrganizationFilterDTO>();
                filterInput.PageNumber = page;
                var result = await _organizationService.GetListAsync(filterInput);
                var model = result.Adapt<OrganizationIndexViewModel>();
                model.SetParentOrganizationFilterSource(
                    (await _organizationService.GetDropDownDataAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>()
                );

                return View(model);
            }

            if (Request.Form["btnCreate"].Count() > 0)
            {
                //int parentId = int.Parse(Request.Form["Filter.ParentId"].ToString());

                return RedirectToAction("Create", new { });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            var parentList = await _organizationService.GetParentsAsync();
            var model = new CreateOrganizationViewModel
            {
                ParentList = parentList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }).ToList()
            };

            return View(model);
        }

    }
}
