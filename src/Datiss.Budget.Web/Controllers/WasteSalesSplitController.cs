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
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;

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
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        //public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        public const string ACTION_ExportExcel = nameof(ExportExcel);

        private readonly IWebHostEnvironment _env;
        private readonly IWasteSalesSplitService _wasteSalesSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;

        public WasteSalesSplitController(
            IWebHostEnvironment environment,
            IWasteSalesSplitService wasteSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {

            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wasteSalesSplitService = wasteSaleSplitService ?? throw new ArgumentNullException(nameof(_wasteSalesSplitService));
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
        public async Task<IActionResult> Create(int organizationId, int yearId)
        {
            var model = new CreateWasteSalesSplitViewModel
            {
                OrganizationId = organizationId,
                YearId = yearId
            };
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWasteSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _wasteSalesSplitService.CreateAsync(new CreateWasteSalesSplitDTO { 
                OrganizationId = model.OrganizationId,
                YearId = model.YearId,
                UserTypeId = model.UserTypeId,
                WsPipeDiameterId = model.WsPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales
            
            });


            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _wasteSalesSplitService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWasteSalesSplitViewModel>();
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWasteSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = model.Adapt<UpdateWasteSalesSplitDTO>();
            var result = await _wasteSalesSplitService.UpdateAsync(data);

            if (!result.IsValid)
            {
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

            var filterInput = new WasteSalesSplitFilterDTO
            {
                OrderBy = "usertype",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _wasteSalesSplitService.GetListAsync(filterInput);
            var model = result.Adapt<WasteSalesSplitIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);

            model.Filter.YearId = filterInput.YearId;
            model.Filter.OrganizationId = filterInput.OrganizationId;

            return View(model);
        }
        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WasteSalesSplitIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<WasteSalesSplitFilterDTO>();
            var result = await _wasteSalesSplitService.GetListAsync(filterInput);
            model = result.Adapt<WasteSalesSplitIndexViewModel>();

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

            return View(model);

            if (Request.Form["btnFilter"].Count() > 0)
            {

            }

            if (Request.Form["btnCreate"].Count() > 0)
            {
                int yearId = int.Parse(Request.Form["Filter.YearId"].ToString());
                int orgId = int.Parse(Request.Form["Filter.OrganizationId"].ToString());

                return RedirectToAction("Create", new
                {
                    organizationId = orgId,
                    yearId = yearId
                });
            }

            return RedirectToAction("Index");
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
        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IFormCollection form)
        {
            var yearId = int.Parse(form["filterYearId"].ToString());
            var orgId = int.Parse(form["filterOrganizationId"].ToString());

            await _wasteSalesSplitService.HardDeleteAsync(yearId, orgId);

            return RedirectToAction("Index");
        }

        //[HttpPost("[action]"), ValidateAntiForgeryToken]
        //public async Task<IActionResult> Calculation(IFormCollection form)
        //{
        //    var yearId = int.Parse(form["filterYearId"].ToString());
        //    var orgId = int.Parse(form["filterOrganizationId"].ToString());

        //    var result = await _waterInstallFeeService.CalculationAsync(yearId, orgId);

        //    return RedirectToAction("Index");
        //}

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
                await _wasteSalesSplitService.CopyAsync(
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

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _wasteSalesSplitService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();

            return workbook.Deliver("WasteSalesSplit.xlsx");
        }
    }
    
}
