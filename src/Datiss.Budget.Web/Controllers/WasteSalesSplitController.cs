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
using Datiss.Budget.Enum;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WasteSalesSplitController : Controller
    {
        public const string Name = "WasteSalesSplit";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        public const string ACTION_ExportExcel = nameof(ExportExcel);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<WasteSalesSplitController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IWasteSalesSplitService _wasteSalesSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;


        public WasteSalesSplitController(
            ILogger<WasteSalesSplitController> logger,
            IWebHostEnvironment environment,
            IWasteSalesSplitService wasteSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wasteSalesSplitService = wasteSaleSplitService ?? throw new ArgumentNullException(nameof(_wasteSalesSplitService));
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
        public async Task<IActionResult> Create(CreateWasteSalesSplitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateWasteSalesSplitDTO>();

            var result = await _wasteSalesSplitService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<WasteSalesSplitViewModel>());

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateWasteSalesSplitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateWasteSalesSplitDTO>();
            var result = await _wasteSalesSplitService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return View(model);
            }

            return Json(
                result.Result.Adapt<WasteSalesSplitViewModel>()
            );
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new WasteSalesSplitFilterDTO();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
               .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var userTypeSource = (await _constantService.GetByConstantKeyAsync("[UserType]"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var wsPipeDiameterSource = (await _constantService.GetByConstantKeyAsync("WastewaterDiameter"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var myfilter = TempData.Get<WasteSalesSplitFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<WasteSalesSplitFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _wasteSalesSplitService.GetListAsync(filter);
            var model = result.Adapt<WasteSalesSplitIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetWasteDiameterSource(wsPipeDiameterSource);

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.PageNumber = filter.PageNumber;
            model.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WasteSalesSplitIndexViewModel model, int page = 1)
        {
            model.Filter.PageNumber = 1;
            var filter = model.Filter.Adapt<WasteSalesSplitFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _wasteSalesSplitService.GetListAsync(filter);
            model = result.Adapt<WasteSalesSplitIndexViewModel>();
            model.Filter = filter.Adapt<WasteSalesSplitFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var userTypeSource = (await _constantService.GetByConstantKeyAsync("[UserType]"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var wsPipeDiameterSource = (await _constantService.GetByConstantKeyAsync("WastewaterDiameter"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();


            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetWasteDiameterSource(wsPipeDiameterSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);

            return View(model);
        }

        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (model.ExcelFile == null ||
                model.ExcelFile.Length == 0)
                return RedirectToAction("Index");

            try
            {
                await _wasteSalesSplitService.ImportExcelAsync(model.ExcelFile);
            }
            catch (ImportExcelFileFormatInvalidException ex)
            {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileFormatInvalid);
            }
            catch (ImportExcelFileSizeInvalidException ex)
            {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileSizeInvalid);
            }
            catch (ImportExcelFileException ex)
            {
                showMessage(CssClassNames.Error,
                    string.Format(
                        ViewMessages.ImportExcelFileItemExist, ex.ExcelRowIndex)
                    );
            }

            showMessage(CssClassNames.Success,
                ViewMessages.ImportExcelSuccess);

            return RedirectToAction("Index");
        }

        [HttpPost("records/delete")]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId)
        {
            try
            {
                var result = await _wasteSalesSplitService.HardDeleteAsync(yearId, orgId);

                return Json(new
                {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForOrg,
                        result.OrganizationTitle,
                        result.Year)
                });
            }
            catch (DeleteNullRecordException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.DeleteNullRecord
                });
            }
            catch (NullReferenceException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.NullRef
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.DeleteRelatedData
                });
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _wasteSalesSplitService.HardDeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.InvalidUpdateData
                });
            }

            return Json(new
            {
                hasError = false,
                message = ViewMessages.DeleteRowSuccess
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Calculation(CalculationInputViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var result = await _wasteSalesSplitService.CalculationAsync(
                model.YearId,
                model.OrganizationId);

            List<CalculationResultViewModel> viewModel = new List<CalculationResultViewModel>();
            foreach (var item in result)
            {
                viewModel.Add(
                    new CalculationResultViewModel
                    {
                        Result = item.Value,
                        Title = getCalcTitle(item.Key)
                    }
                );
            }

            return PartialView("_calculationModal", viewModel);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            var filePath = $"{_env.WebRootPath}\\Excel\\WasteSalesSplitImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WasteSalesSplit.xlsx");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Copy()
        {
            var model = new CopyViewModel();

            model.SetOrganizationSource(
                (await _organizationService.GetDropDownDataAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>()
            );

            model.SetYearSource(
                (await _financeYearService.GetDropDownDataByStatusAsync(EntityStatus.Disbaled))
                    .Adapt<IEnumerable<DropDownItemViewModel>>()
            );

            model.SetTargetYearSource(
                (await _financeYearService.GetDropDownDataByStatusAsync(EntityStatus.Enabled))
                    .Adapt<IEnumerable<DropDownItemViewModel>>()
            );

            return PartialView("_copyModal", model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Copy(CopyViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                await _wasteSalesSplitService.CopyAsync(
                                                    model.SourceYearId,
                                                    model.SourceOrgId,
                                                    model.TargetYearId);
                model.Succeed(ViewMessages.CopySuccess);
            }
            catch (CopySameYearException)
            {
                model.AddError(ViewMessages.CopySameYear);
            }
            catch (CopyDestYearExxeption)
            {
                model.AddError(ViewMessages.CopyErrorDestYear);
            }
            catch (CopyOrgNullDataException)
            {
                model.AddError(ViewMessages.CopySourceOrgNullData);
            }
            catch (CopyDestYearHasDataException)
            {
                model.AddError(ViewMessages.CopyDestYearHasData);
            }
            catch (Exception ex)
            {
                model.AddError(ViewMessages.SystemError);
            }

            return Json(model);
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _wasteSalesSplitService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("WasteSalesSplit.xlsx");
        }
        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                "WasteSalesSplit_Cal1" => SPTitles.WasteSalesSplit_Cal1,
                "WasteSalesSplit_Cal2" => SPTitles.WasteSalesSplit_Cal2,
                "WasteSalesSplit_Cal3" => SPTitles.WasteSalesSplit_Cal3,
                "WasteSalesSplit_Cal4" => SPTitles.WasteSalesSplit_Cal4,
                "WasteSalesSplit_Cal5" => SPTitles.WasteSalesSplit_Cal5,
                "WasteSalesSplit_Cal6" => SPTitles.WasteSalesSplit_Cal6,
                _ => ""
            };
        #endregion
    }

}