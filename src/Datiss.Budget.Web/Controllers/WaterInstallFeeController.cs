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

namespace Datiss.Budget.Web.Controllers
{

    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WaterInstallFeeController : Controller {

        public const string Name = "WaterInstallFee";
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

        private readonly ILogger<WaterInstallFeeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IWaterInstallFeeService _waterInstallFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public WaterInstallFeeController(
            ILogger<WaterInstallFeeController> logger,
            IWebHostEnvironment environment,
            IWaterInstallFeeService waterInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService) 
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _waterInstallFeeService = waterInstallFeeService ?? throw new ArgumentNullException(nameof(waterInstallFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        private void showMessage(string type, string message) {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWaterInstallFeeViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateWaterInstallFeeDTO>();

            var result = await _waterInstallFeeService.CreateAsync(data);

            if(! result.IsValid) {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<WaterInstallFeeViewModel>());
        }

        
        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateWaterInstallFeeViewModel model) {

            if (!ModelState.IsValid) {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateWaterInstallFeeDTO>();
            var result = await _waterInstallFeeService.UpdateAsync(data);

            if(!result.IsValid) {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<WaterInstallFeeViewModel>()
            ); 
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) 
        {
            var filter = new WaterInstallFeeFilterDTO();

            var myfilter = TempData.Get<WaterInstallFeeFilterViewModel>(_indexFilterKey);
            if(myfilter != null) 
            {
                filter = myfilter.Adapt<WaterInstallFeeFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }
            
            var orgSource = (await _organizationService.GetDropDownDataAsync())
               .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var dwaterSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
               .Adapt<List<DropDownItemViewModel>>();

            filter.PageNumber = page;
            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var result = await _waterInstallFeeService.GetListAsync(filter);
            var model = result.Adapt<WaterInstallFeeIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetDWaterTypeSource(dwaterSource);

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);
            
            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterInstallFeeIndexViewModel model, int page = 1) {
            model.Filter.PageNumber = page;
            var filter = model.Filter.Adapt<WaterInstallFeeFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _waterInstallFeeService.GetListAsync(filter);
            model = result.Adapt<WaterInstallFeeIndexViewModel>();
            model.Filter = filter.Adapt<WaterInstallFeeFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var dwaterSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
               .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            model.SetDWaterTypeSource(dwaterSource);
            
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (model.ExcelFile == null ||
                model.ExcelFile.Length == 0)
                    return RedirectToAction("Index");

            try {
                var result = await _waterInstallFeeService.ImportExcelAsync(model.ExcelFile);
                if(result.AskToImport) {
                    return Json(new {
                        ask = true,
                        message = result.Message
                    });
                }

                if(!result.Success) {
                    return Json(new {
                        hasError = true,
                        message = result.Message
                    });
                }
            }
            catch (ImportExcelFileFormatInvalidException ex) {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileFormatInvalid);
                return Json(new {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileFormatInvalid
                });
            }
            catch (ImportExcelFileSizeInvalidException ex) {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileSizeInvalid);
                return Json(new {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileSizeInvalid
                });
            }
            catch (ImportExcelFileException ex) {
                showMessage(CssClassNames.Error,
                    string.Format(
                        ViewMessages.ImportExcelFileItemExist, ex.ExcelRowIndex)
                    );
                return Json(new {
                    hasError = true,
                    message = string.Format(
                        ViewMessages.ImportExcelFileItemExist, ex.ExcelRowIndex)
                });
            }

            showMessage(CssClassNames.Success,
                ViewMessages.ImportExcelSuccess);

            return RedirectToAction("Index");
        }

        [HttpPost("records/delete")]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId) {
            try {
                var result = await _waterInstallFeeService.HardDeleteAsync(yearId, orgId);

                return Json(new {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForOrg,
                        result.OrganizationTitle,
                        result.Year)
                });
            }
            catch(NullReferenceException) {
                return Json(new {
                    hasError = true,
                    message = ViewMessages.NullRef
                });
            }
            catch(Exception ex) {
                return Json(new {
                    hasError = true,
                    message = ViewMessages.DeleteRelatedData
                });
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                await _waterInstallFeeService.HardDeleteAsync(id);
            }
            catch(Exception ex) {
                _logger.LogError(ex.GetBaseException().Message);
                return Json(new {
                    hasError = true,
                    message = "خطا در بروزرسانی اطلاعات. لطفاً دوباره سعی کنید."
                });
            }

            return Json(new {
                hasError = false,
                message = "حذف رکورد با موفقیت انجام شد."
            });
        }

        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculation(IFormCollection form) {
            var yearId = int.Parse(form["filterYearId"].ToString());
            var orgId = int.Parse(form["filterOrganizationId"].ToString());

            var result = await _waterInstallFeeService.CalculationAsync(yearId, orgId);

            return RedirectToAction("Index");
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadExcelTemplate() {
            var filePath = $"{_env.WebRootPath}\\Excel\\WaterInstallFeeImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                "WaterInstallFee.xlsx");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Copy() {
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
        public async Task<IActionResult> Copy(CopyViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            try {
                await _waterInstallFeeService.CopyAsync(
                                                    model.SourceYearId, 
                                                    model.SourceOrgId, 
                                                    model.TargetYearId);
                model.Succeed(ViewMessages.CopySuccess);
            }
            catch(CopySameYearException) {
                model.AddError(ViewMessages.CopySameYear);
            }
            catch (CopyDestYearExxeption) {
                model.AddError(ViewMessages.CopyErrorDestYear);
            }
            catch (CopyOrgNullDataException)
            {
                model.AddError(ViewMessages.CopySourceOrgNullData);
            }
            catch (CopyDestYearHasDataException) {
                model.AddError(ViewMessages.CopyDestYearHasData);
            }
            catch(Exception ex) {
                model.AddError(ViewMessages.SystemError);
            }
            
            return Json(model);
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid) {
            var result = await _waterInstallFeeService.GetExportItemsAsync(yearid,orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("WatreInstallFee.xlsx");
        }

    }
}
