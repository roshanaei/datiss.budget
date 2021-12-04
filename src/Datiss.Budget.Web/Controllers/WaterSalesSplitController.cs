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

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WaterSalesSplitController : Controller
    {
        public const string Name = "WaterSalesSplit";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        //public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        //public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        public const string ACTION_ExportExcel = nameof(ExportExcel);

        private readonly IWebHostEnvironment _env;
        private readonly IWaterSalesSplitService _waterSalesSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;


        public WaterSalesSplitController(
            IWebHostEnvironment environment,
            IWaterSalesSplitService waterSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId)
        {
            var model = new CreateWaterSalesSplitViewModel
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

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWaterSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _waterSalesSplitService.CreateAsync(new CreateWaterSalesSplitDTO { 
                OrganizationId = model.OrganizationId,
                YearId = model.YearId,
                UserTypeId = model.UserTypeId,
                WPipeDiameterId = model.WPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales
            
            });


            if (!result.IsValid)
            {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _waterSalesSplitService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWaterSalesSplitViewModel>();
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWaterSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var data = model.Adapt<UpdateWaterSalesSplitDTO>();
            var result = await _waterSalesSplitService.UpdateAsync(data);

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

            var filterInput = new WaterSalesSplitFilterDTO
            {
                OrderBy = "usertype",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _waterSalesSplitService.GetListAsync(filterInput);
            var model = result.Adapt<WaterSalesSplitIndexViewModel>();

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
        public async Task<IActionResult> Index(WaterSalesSplitIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<WaterSalesSplitFilterDTO>();
            var result = await _waterSalesSplitService.GetListAsync(filterInput);
            model = result.Adapt<WaterSalesSplitIndexViewModel>();

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
        [HttpPost("[action]"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IFormCollection form)
        {
            var yearId = int.Parse(form["filterYearId"].ToString());
            var orgId = int.Parse(form["filterOrganizationId"].ToString());

            await _waterSalesSplitService.HardDeleteAsync(yearId, orgId);

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
        public async Task<IActionResult> ExportExcel(WaterSalesSplitIndexViewModel viewModel)
        {
            var filter = viewModel.Filter.Adapt<WaterSalesSplitFilterDTO>();

            var result = await _waterSalesSplitService.GetExportItemsAsync(filter);
            //var stream = new MemoryStream();
            using var workbook = result.ExportExcel();

            return workbook.Deliver("WaterSalesSplit.xlsx");
        }
    }
    
}
