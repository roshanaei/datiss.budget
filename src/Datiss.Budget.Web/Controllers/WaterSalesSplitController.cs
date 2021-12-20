using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using Microsoft.AspNetCore.Http;
using System.IO;
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Services.Identity;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Common;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WaterSalesSplitController : Controller
    {
        public const string Name = "WaterSalesSplit";
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

        private readonly ILogger<WaterSalesSplitController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IWaterSalesSplitService _waterSalesSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;


        public WaterSalesSplitController(
            ILogger<WaterSalesSplitController> logger,
            IWebHostEnvironment environment,
            IWaterSalesSplitService waterSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _waterSalesSplitService = waterSaleSplitService ?? throw new ArgumentNullException(nameof(_waterSalesSplitService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }
        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWaterSalesSplitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateWaterSalesSplitDTO>();

            var result = await _waterSalesSplitService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<WaterSalesSplitViewModel>());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateWaterSalesSplitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateWaterSalesSplitDTO>();
            var result = await _waterSalesSplitService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<WaterSalesSplitViewModel>()
            );
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new WaterSalesSplitFilterDTO();

            var myfilter = TempData.Get<WaterSalesSplitFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<WaterSalesSplitFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }
            var orgSource = (await _organizationService.GetDropDownDataAsync())
               .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
               .Adapt<List<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var userTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var waterDiameterSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WaterDiameter))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            filter.PageNumber = page;
            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var result = await _waterSalesSplitService.GetListAsync(filter);
            var model = result.Adapt<WaterSalesSplitIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetWaterDiameterSource(waterDiameterSource);

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;

            return View(model);
        }
        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterSalesSplitIndexViewModel model, int page = 1)
        {
            model.Filter.PageNumber = page;
            var filter = model.Filter.Adapt<WaterSalesSplitFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _waterSalesSplitService.GetListAsync(filter);
            model = result.Adapt<WaterSalesSplitIndexViewModel>();
            model.Filter = filter.Adapt<WaterSalesSplitFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
               .Adapt<List<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var userTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var waterDiameterSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WaterDiameter))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetWaterDiameterSource(waterDiameterSource);

            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            
            return View(model);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (model.ExcelFile == null ||
                model.ExcelFile.Length == 0)
                return RedirectToAction("Index");

            try
            {
                await _waterSalesSplitService.ImportExcelAsync(model.ExcelFile);
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
                var result = await _waterSalesSplitService.HardDeleteAsync(yearId, orgId);

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
                await _waterSalesSplitService.HardDeleteAsync(id);
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

            var result = await _waterSalesSplitService.CalculationAsync(
                model.YearId,
                model.OrganizationId);

            var output = new CalculationResultViewModel
            {
                Result = result,
                Title = "WaterSalesSplit calc" //TODO : change it to proper title
            };

            return PartialView("_calculationModal", output);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            var filePath = $"{_env.WebRootPath}\\Excel\\WaterSalesSplitImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WaterSalesSplit.xlsx");
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
                (await _financeYearService.GetDropDownDataAsync())
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
                await _waterSalesSplitService.CopyAsync(
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
            var result = await _waterSalesSplitService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("WaterSalesSplit.xlsx");
        }
    }
    
}
