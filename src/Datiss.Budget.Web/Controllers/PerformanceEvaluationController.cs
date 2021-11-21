using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Web.Helpers;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class PerformanceEvaluationController : Controller
    {

        public const string Name = "WaterInstallFee";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_ImportExcel = nameof(ImportExcel);

        private readonly IWebHostEnvironment _env;
        private readonly IPerformanceEvalutionService _performanceEvalutionService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public PerformanceEvaluationController(
            IWebHostEnvironment environment,
            IPerformanceEvalutionService performanceEvalutionService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _performanceEvalutionService = performanceEvalutionService ?? throw new ArgumentNullException(nameof(performanceEvalutionService));
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            var model = new CreatePerformanceEvaluationViewModel();

            //var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            //model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem
            //{
            //    Text = x.Title,
            //    Value = x.Id.ToString()
            //});

            return PartialView("_create", model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreatePerformanceEvaluationViewModel model)
        {
            var result = await _performanceEvalutionService.CreateAsync(new CreatePerformanceEvaluationDTO
            {
                TableFieldId = model.TableFieldId,
                OrganizationId = model.OrganizationId,
                Operation = model.Operation,
                YearId = model.YearId,
                Target = model.Target
            });

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<PerformanceEvaluationViewModel>());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _performanceEvalutionService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdatePerformanceEvaluationViewModel>();
            //var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            //model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem
            //{
            //    Text = x.Title,
            //    Value = x.Id.ToString()
            //});

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdatePerformanceEvaluationViewModel model)
        {
            //var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("[usertype]");
            //model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
            //    Text = x.Title,
            //    Value = x.Id.ToString()
            //});

            if (!ModelState.IsValid)
            {
                model.AddError("خطاهای داده ای را بررسی نمایید.");
                return Json(model);
            }

            var data = model.Adapt<UpdatePerformanceEvaluationDTO>();
            var result = await _performanceEvalutionService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<PerformanceEvaluationViewModel>()
            );

            //return RedirectToAction("Index", new { page = model._CurrentPage });
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

            var filterInput = new PerformanceEvaluationFilterDTO
            {
                OrderBy = "displayorder",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _performanceEvalutionService.GetListAsync(filterInput);
            var model = result.Adapt<PerformanceEvaluationIndexViewModel>();

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
        public async Task<IActionResult> Index(PerformanceEvaluationIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<PerformanceEvaluationFilterDTO>();

            var result = await _performanceEvalutionService.GetListAsync(filterInput);
            model = result.Adapt<PerformanceEvaluationIndexViewModel>();

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
                await _performanceEvalutionService.ImportExcelAsync(model.ExcelFile);
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
            var orgId = int.Parse(form["filterOrganizationId"].ToString());
            await _performanceEvalutionService.SoftDeleteAsync(orgId);
            return RedirectToAction("Index");
        }
    }
}

