
using System;
using System.Linq;
using System.Threading.Tasks;
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
using Datiss.Budget.Web.ViewModels;
using Datiss.Budget.Common.GuardToolkit;
using Mapster;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]/[action]")]
    public class OrganizationController : Controller
    {

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
            var model = new OrganizationIndexViewModel();

            model.SetParentOrganizationFilterSource(await _organizationService.GetDropDownDataAsync());

            var filterInput = new OrganizationFilter
            {
                OrderBy = "DisplayOrder",
                PageNumber = page,
                PageSize = 10
            };

            var result = await _organizationService.GetListAsync(filterInput);

            model.Model = result;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (OrganizationIndexViewModel viewModel,int page = 1)
        {
            if(Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = viewModel.Filter.Adapt<OrganizationFilter>();

                var result = await _organizationService.GetListAsync(filterInput);

                viewModel.SetParentOrganizationFilterSource(await _organizationService.GetDropDownDataAsync());
                viewModel.Model = result;

                return View(viewModel);
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
            var model = new AddOrganizationViewModel
            {
                ParentList = parentList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                })
            };

            return View(model);
        }

    }
}
