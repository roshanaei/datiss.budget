using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Entities.DWH;
using System.Threading.Tasks;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using Datiss.Budget.Common;
using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WWsFeeController : Controller
    {
        public const string Name = "WWsFee";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<WWsFee> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConsumeForcastService _consumeForcastService;
        private readonly IWWsFeeService _wWsFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public WWsFeeController(
            ILogger<WWsFee> logger,
            IWebHostEnvironment environment,
            IWWsFeeService wWsFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wWsFeeService = wWsFeeService ?? throw new ArgumentNullException(nameof(wWsFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWWsFeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateWWsFeeDTO>();

            var result = await _wWsFeeService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<WWsFeeViewModel>());
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateWWsFeeViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateWWsFeeDTO>();
            var result = await _wWsFeeService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<WWsFeeViewModel>()
            );
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new WWsFeeFilterDTO();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(x => x.Id);

            var userTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__UserType);

            var userTypeSource = userTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();

            var userTypeKeys = "";

            foreach (var key in userTypeData)
            {
                userTypeKeys += $"'{key.ConstantKey}',";
            }

            ViewData["userTypeKeys"] = userTypeKeys.TrimEnd(',');

            var usagelayerSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UsageLayerType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var activity = new ActivityType();
            var activityTypeSource = EnumSelectListProvider.GetActivityTypeItems(activity);

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var myfilter = TempData.Get<WWsFeeFilterViewModel>(_indexFilterKey);

            if (myfilter != null)
            {
                filter = myfilter.Adapt<WWsFeeFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _wWsFeeService.GetListAsync(filter);
            var model = result.Adapt<WWsFeeIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetActivityTypeSource(activityTypeSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetUsageLayerSource(usagelayerSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.PageNumber = filter.PageNumber;
            model.PageSize = filter.PageSize;

            return View(model);
        }

    }
}
