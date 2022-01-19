using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Mapster;
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
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using ClosedXML.Extensions;
using Datiss.Budget.Reports.Excel;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Common;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class SubscriptionController : Controller
    {
        public const string Name = "Subscription";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<SubscriptionController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public SubscriptionController(
            ILogger<SubscriptionController> logger,
            IWebHostEnvironment environment,
            ISubscriptionService subscriptionService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateSubscriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateSubscriptionDTO>();

            var result = await _subscriptionService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<SubscriptionViewModel>());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateSubscriptionViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateSubscriptionDTO>();
            var result = await _subscriptionService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<SubscriptionViewModel>()
            );
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new SubscriptionFilterDTO();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var userSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.YearId = maxYear;

            var myfilter = TempData.Get<SubscriptionFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<SubscriptionFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _subscriptionService.GetListAsync(filter);
            var model = result.Adapt<SubscriptionIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetUserTypeSource(userSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);

            model.Filter.YearId = filter.YearId;
            model.PageNumber = filter.PageNumber;
            model.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SubscriptionIndexViewModel model)
        {

            var filter = model.Filter.Adapt<SubscriptionFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _subscriptionService.GetListAsync(filter);
            
            model = result.Adapt<SubscriptionIndexViewModel>();
            
            model.Filter = filter.Adapt<SubscriptionFilterViewModel>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var userSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetUserTypeSource(userSource);

            return View(model);
        }
    }
}
