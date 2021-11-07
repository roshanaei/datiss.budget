using System;
using System.Linq;
using System.Threading.Tasks;
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
using Datiss.Budget.Web.ViewModels;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using Ganss.Excel;

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
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        public const string ACTION_ExportExcel = nameof(ExportExcel);

        private readonly IWebHostEnvironment _env;
        private readonly IWaterInstallFeeService _waterInstallFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public WaterInstallFeeController(
            IWebHostEnvironment environment,
            IWaterInstallFeeService waterInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService) 
        {
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId) {
            var model = new AddWaterInstallFeeViewModel {
                OrganizationId = organizationId,
                YearId = yearId
            };

            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(AddWaterInstallFeeViewModel model) 
        {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {

                return View(model);
            }

            var result = await _waterInstallFeeService.AddAsync(new CreateWaterInstallFeeDTO {
                DWaterTypeId = model.DWaterTypeId,
                OrganizationId = model.OrganizationId,
                WInstllFee = model.WInstllFee,
                YearId = model.YearId,
                DWaterTypeTitle = model.DWaterTypeTitle
            });

            if(! result.IsValid) {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            var entity = await _waterInstallFeeService.GetByIdAsync(id);

            if(entity == null) {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWaterInstallFeeViewModel>();
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWaterInstallFeeViewModel model) {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {
                return View(model);
            }

            var result = await _waterInstallFeeService.UpdateAsync(model);

            if(!result.IsValid) {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index", new { page = model._CurrentPage });
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) 
        {
            //var access = _securityTrimmingService.CanCurrentUserAccess("", "WaterInstallFee", "Index");

            var model = new WaterInstallFeeIndexViewModel();
            var years = await _financeYearService.GetDropDownDataAsync();
            int maxYear = years.Max(_ => _.Id);

            model.SetFinanceYearFilterSource(years, maxYear);
            model.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync());

            int firstOrgId = int.Parse(model.Filter.OrganizationSource.FirstOrDefault().Value);

            var filterInput = new WaterInstallFeeFilter {
                OrderBy = "dwatertype",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _waterInstallFeeService.GetListAsync(filterInput);

            model.Model = result;
            model.Filter.YearId = filterInput.YearId;
            model.Filter.OrganizationId = filterInput.OrganizationId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterInstallFeeIndexViewModel viewModel, int page = 1) {
            if (Request.Form["btnFilter"].Count() > 0) {
                var filterInput = viewModel.Filter.Adapt<WaterInstallFeeFilter>();

                var result = await _waterInstallFeeService.GetListAsync(filterInput);

                viewModel.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
                viewModel.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync());
                viewModel.Model = result;

                return View(viewModel);
            }

            if (Request.Form["btnCreate"].Count() > 0) {
                int yearId = int.Parse(Request.Form["Filter.YearId"].ToString());
                int orgId = int.Parse(Request.Form["Filter.OrganizationId"].ToString());

                return RedirectToAction("Create", new {
                    organizationId = orgId,
                    yearId = yearId
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (model.ExcelFile == null ||
                model.ExcelFile.Length == 0)
                    return RedirectToAction("Index");

            try {
                await _waterInstallFeeService.ImportExcelAsync(model.ExcelFile);
            }
            catch (ImportExcelFileFormatInvalidException ex) {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileFormatInvalid);
            }
            catch (ImportExcelFileSizeInvalidException ex) {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileSizeInvalid);
            }
            catch (ImportExcelFileException ex) {
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
        public async Task<IActionResult> Delete(IFormCollection form) {
            var yearId = int.Parse(form["filterYearId"].ToString());
            var orgId = int.Parse(form["filterOrganizationId"].ToString());

            await _waterInstallFeeService.HardDeleteAsync(yearId, orgId);

            return RedirectToAction("Index");
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
            model.SetOrganizationSource(await _organizationService.GetDropDownDataAsync());
            model.SetYearSource(await _financeYearService.GetDropDownDataAsync());

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
            }
            catch(CopySameYearException ex) {
                model.AddError(ViewMessages.CopySameYear);
                return View(model);
            }
            catch(CopyDestYearHasDataException ex) {
                model.AddError(ViewMessages.CopyDestYearHasData);
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ExportExcel(WaterInstallFeeIndexViewModel viewModel) {
            var filter = viewModel.Filter.Adapt<WaterInstallFeeFilter>();

            var result = await _waterInstallFeeService.GetExportItemsAsync(filter);

            var _excelMapper = new ExcelMapper();
            //var ms = new MemoryStream();
            //await _excelMapper.SaveAsync(ms, result);

            var stream = new FileStream("export.xlsx", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            await _excelMapper.SaveAsync(stream, result);

            //var mem = new MemoryStream(ms.ToArray());
            //mem.Seek(0, SeekOrigin.Begin);

            //ms.Seek(0, SeekOrigin.Begin);
            //using (var stream = new MemoryStream()) {
            //    await _waterInstallFeeService.ExportExcelAsync(filter, stream);
            //    return File(
            //        stream,
            //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //        "WaterInstallFee.xlsx");
            //}

            //var stream = new MemoryStream();

            //try {
            //    var result = await _waterInstallFeeService.ExportExcelAsync(filter, stream);
            //    result.CopyTo(stream);
            //}
            //catch(Exception ex) {

            //}

            //return new FileStreamResult(
            //    ms,
            //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WaterInstallFee.xlsx");
        }

    }
}
