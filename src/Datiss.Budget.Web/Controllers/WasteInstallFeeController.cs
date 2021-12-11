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
using Microsoft.AspNetCore.Hosting;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WasteInstallFeeController : Controller
    {
        public const string Name = "WasteInstallFee";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        //public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        public const string ACTION_ExportExcel = nameof(ExportExcel);

        private readonly IWebHostEnvironment _env;
        private readonly IWasteInstallFeeService _wasteInstallFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;

        public WasteInstallFeeController(
            IWebHostEnvironment environment,
            IWasteInstallFeeService wasteInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wasteInstallFeeService = wasteInstallFeeService ?? throw new ArgumentNullException(nameof(wasteInstallFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }
        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId) {
            var model = new CreateWasteInstallFeeViewModel {
                OrganizationId = organizationId,
                YearId = yearId
            };

            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWasteInstallFeeViewModel model)
        {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {

                return View(model);
            }

            var result = await _wasteInstallFeeService.CreateAsync(new CreateWasteInstallFeeDTO {
                DWasteTypeId = model.DWasteTypeId,
                OrganizationId = model.OrganizationId,
                WInstllFee = model.WInstllFee,
                YearId = model.YearId,
                DWasteTypeTitle = model.DWasteTypeTitle
            });

            if (!result.IsValid) {
                model.AddError(result.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            var entity = await _wasteInstallFeeService.GetByIdAsync(id);

            if (entity == null) {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWasteInstallFeeViewModel>();
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateWasteInstallFeeViewModel model) {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {
                return View(model);
            }

            var result = await _wasteInstallFeeService.UpdateAsync(model.Adapt<UpdateWasteInstallFeeDTO>());

            if (!result.IsValid) {
                model.AddError(result.Message);
                return View(model);
            }

            return RedirectToAction("Index", new { page = model._CurrentPage });
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var orgSource = (await _organizationService.GetDropDownDataAsync())
               .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var dwaterSource = (await _constantService.GetByConstantKeyAsync("usertype"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var filterInput = new WasteInstallFeeFilterDTO
            {
                OrderBy = "dwastetype",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _wasteInstallFeeService.GetListAsync(filterInput);
            var model = result.Adapt<WasteInstallFeeIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetDWaterTypeSource(dwaterSource);

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);

            model.Filter.YearId = filterInput.YearId;
            model.Filter.OrganizationId = filterInput.OrganizationId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WasteInstallFeeIndexViewModel model, int page = 1)
        {
            model.Filter.PageNumber = page;
            var filterInput = model.Filter.Adapt<WasteInstallFeeFilterDTO>();

            var result = await _wasteInstallFeeService.GetListAsync(filterInput);
            model = result.Adapt<WasteInstallFeeIndexViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var dwaterSource = (await _constantService.GetByConstantKeyAsync("usertype"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            model.SetDWaterTypeSource(dwaterSource);

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
                await _wasteInstallFeeService.ImportExcelAsync(model.ExcelFile);
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


        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IFormCollection form)
        {
            var yearId = int.Parse(form["filterYearId"].ToString());
            var orgId = int.Parse(form["filterOrganizationId"].ToString());

            await _wasteInstallFeeService.HardDeleteAsync(yearId, orgId);

            return RedirectToAction("Index");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            var filePath = $"{_env.WebRootPath}\\Excel\\WasteInstallFeeImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WasteInstallFee.xlsx");
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
                await _wasteInstallFeeService.CopyAsync(
                                                    model.SourceYearId,
                                                    model.SourceOrgId,
                                                    model.TargetYearId);
            }
            catch (CopySameYearException ex)
            {
                model.AddError(ViewMessages.CopySameYear);
                return View(model);
            }
            catch (CopyDestYearHasDataException ex)
            {
                model.AddError(ViewMessages.CopyDestYearHasData);
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ExportExcel(WasteInstallFeeIndexViewModel viewModel)
        {
            var filter = viewModel.Filter.Adapt<WasteInstallFeeFilterDTO>();
            var result = await _wasteInstallFeeService.GetExportItemsAsync(filter);
            using var workbook = result.ExportExcel();
            return workbook.Deliver("WasteInstallFee.xlsx");
        }
        [HttpPost("records/delete")]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId)
        {
            try
            {
                var result = await _wasteInstallFeeService.HardDeleteAsync(yearId, orgId);

                return Json(new
                {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForOrg,
                        result.OrganizationTitle,
                        result.Year)
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

    }
}
