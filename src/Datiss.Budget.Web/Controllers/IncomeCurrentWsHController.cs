using Datiss.Budget.Common;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class IncomeCurrentWsHController : Controller
    {
        public const string Name = "IncomeCurrentWsH";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_ImportExcel = nameof(ImportExcel);


        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<IncomeCurrentWsHController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IIncomeCurrentWsHService _incomeCurrentWsHService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public IncomeCurrentWsHController(
            ILogger<IncomeCurrentWsHController> logger,
            IWebHostEnvironment environment,
            IIncomeCurrentWsHService incomeCurrentWsHService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _incomeCurrentWsHService = incomeCurrentWsHService ?? throw new ArgumentNullException(nameof(incomeCurrentWsHService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateIncomeCurrentWsHViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<CreateIncomeCurrentWsHDTO>();

            var result = await _incomeCurrentWsHService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<IncomeCurrentWsHViewModel>());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateIncomeCurrentWsHViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateIncomeCurrentWsHDTO>();
            var result = await _incomeCurrentWsHService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<IncomeCurrentWsHViewModel>()
            );
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new IncomeCurrentWsHFilterDTO();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(x => x.Id);

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            var userTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var usageLayerTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UsageLayerType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var myfilter = TempData.Get<IncomeCurrentWHFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<IncomeCurrentWsHFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _incomeCurrentWsHService.GetListAsync(filter);
            var model = result.Adapt<IncomeCurrentWsHIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetUsageLayerTypeSource(usageLayerTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IncomeCurrentWsHIndexViewModel model)
        {
            var filter = model.Filter.Adapt<IncomeCurrentWsHFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _incomeCurrentWsHService.GetListAsync(filter);
            model = result.Adapt<IncomeCurrentWsHIndexViewModel>();
            model.Filter = filter.Adapt<IncomeCurrentWsHFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var userTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var usageLayerTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UsageLayerType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetUsageLayerTypeSource(usageLayerTypeSource);


            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (model.ExcelFile == null || model.ExcelFile.Length == 0)
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.ImportExcelInvalidFile
                });

            try
            {
                var result = await _incomeCurrentWsHService.ImportExcelAsync(
                                                                    model.ExcelFile,
                                                                    model.YearId,
                                                                    model.ContinueIfAnyOrgMissing);
                if (result.AskToImport)
                {
                    return Json(new
                    {
                        ask = true,
                        message = result.Message
                    });
                }

                if (result.Success)
                {
                    return Json(new
                    {
                        hasError = false,
                        message = result.Message
                    });
                }
                else
                {
                    return Json(new
                    {
                        hasError = true,
                        message = result.Message
                    });
                }
            }
            catch (ImportExcelFileFormatInvalidException ex)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileFormatInvalid
                });
            }
            catch (ImportExcelFileSizeInvalidException ex)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileSizeInvalid
                });
            }
        }


    }
}
