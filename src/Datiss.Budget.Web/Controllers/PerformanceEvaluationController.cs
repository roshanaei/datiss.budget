using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;
using Datiss.Budget.Entities;
using Datiss.Budget.Security;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class PerformanceEvaluationController : Controller
    {
        public const string Name = "PerformanceEvaluation";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly IWebHostEnvironment _env;
        private readonly IPerformanceEvaluationService _performanceEvalutionService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ITablesFieldTitleService _tablesFieldTitleService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public PerformanceEvaluationController(
            IWebHostEnvironment environment,
            IPerformanceEvaluationService performanceEvalutionService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            ITablesFieldTitleService tablesFieldTitleService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _performanceEvalutionService = performanceEvalutionService ?? throw new ArgumentNullException(nameof(performanceEvalutionService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _tablesFieldTitleService = tablesFieldTitleService ?? throw new ArgumentNullException(nameof(tablesFieldTitleService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdatePerformanceEvaluationViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
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
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1, TablesName tableName = TablesName.CurrentIncome)
        {
            var filter = new PerformanceEvaluationFilterDTO();
            filter.PageSize = 25;

            var orgSource = (await _organizationService.GetDropDownDataAsync()).ToList();
            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true)).ToList();
            var exceptOrgList = new List<DropDownItem>();
            foreach (var Torg in orgSource)
                if (!inputOrgSource.Any(x => x.Id == Torg.Id))
                    exceptOrgList.Add(Torg);

            var dropDownList = exceptOrgList
                .Adapt<List<DropDownItemViewModel>>();


            int firstOrgId = orgSource.FirstOrDefault().Id;


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);


            filter.TableName = tableName;
            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var myfilter = TempData.Get<PerformanceEvaluationFilterViewModel>(_indexFilterKey + $"_{tableName}");
            if (myfilter != null)
            {
                filter = myfilter.Adapt<PerformanceEvaluationFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _performanceEvalutionService.GetListAsync(filter);
            var model = result.Adapt<PerformanceEvaluationIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(dropDownList);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(dropDownList, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = 25;
            model.Filter.TableName = filter.TableName;

            int month = 0;
            if(result.Items.Count() != 0)
                month = result.Items.Select(x => x.Month).First();
            ViewData["Month"] = month;
            ViewData["TablesName"] = tableName.ToDisplay();

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PerformanceEvaluationIndexViewModel model, TablesName tableName = TablesName.CurrentIncome)
        {
            var filter = model.Filter.Adapt<PerformanceEvaluationFilterDTO>();
            filter.TableName = tableName;
            TempData.Put(_indexFilterKey + $"_{tableName}", filter);

            var result = await _performanceEvalutionService.GetListAsync(filter);
            model = result.Adapt<PerformanceEvaluationIndexViewModel>();
            model.Filter = filter.Adapt<PerformanceEvaluationFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync());
            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true));
            var dropDownList = orgSource.Except(inputOrgSource)
                .Adapt<List<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(dropDownList);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(dropDownList, filter.OrganizationId);

            return View(model);
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model, TablesName tablesName)
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
                var result = await _performanceEvalutionService.ImportExcelAsync(
                                                                    model.ExcelFile,
                                                                    model.YearId,
                                                                    tablesName,
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
            catch (ImportExcelFileFormatInvalidException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileFormatInvalid
                });
            }
            catch (ImportExcelFileSizeInvalidException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileSizeInvalid
                });
            }
        }


        [HttpPost("records/delete")]
        [HasPermission(claimType: Name, PermissionActionType.Delete)]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId, TablesName tablesName)
        {
            try
            {
                var result = await _performanceEvalutionService.SoftDeleteAsync(yearId, orgId, tablesName);

                return Json(new
                {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForOrg,
                        result.OrganizationTitle,
                        result.Year)
                });
            }
            catch (DisbaledYearDataInputException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.Logic_InputDisableYearData
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
            catch (Exception)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.DeleteRelatedData
                });
            }
        }

        [HttpGet("import/template/{yearId}/{orgId?}/{tablesname}")]
        public async Task<IActionResult> GetExcelTemplate(int yearId, int? orgId, TablesName tablesName)
        {
            var year = await _financeYearService.GetByIdAsync(yearId);

            var orgSource = (await _organizationService.GetWithChildrenAsync(orgId)).ToList();
            var inputOrgSource = (await _organizationService.GetWithChildrenAsync(orgId, true)).ToList();
            var exceptOrgList = new List<Organization>();
            foreach (var Torg in orgSource)
                if (!inputOrgSource.Any(x => x.Id == Torg.Id))
                    exceptOrgList.Add(Torg);

            var dropDownList = exceptOrgList
                .Adapt<List<DropDownItemViewModel>>();

            var tableName = await _tablesFieldTitleService.GetByTableSectionNameAsync(tablesName);

            var items = new List<PerformanceEvaluationDTO>();

            foreach (var org in dropDownList)
            {
                foreach (var tname in tableName)
                {
                    items.Add(new PerformanceEvaluationDTO
                    {
                        TableFieldId = tname.Id,
                        TableFieldDisplay = tname.Title,
                        OrganizationId = org.Id,
                        OrganizationDisplay = org.Title,
                        Year = year.Year,
                        YearId = year.Id
                    });
                }
            }

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver($"PerformanceEvaluation-{tablesName}-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}/{tablesname}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid, TablesName tablesName)
        {
            var result = await _performanceEvalutionService.GetExportItemsAsync(yearid, orgid, tablesName);
            if (result.Count() == 0)
                return RedirectToAction(
                    PerformanceEvaluationController.ACTION_Index,
                    PerformanceEvaluationController.Name,
                    new
                    {
                        tableName = tablesName
                    });
            using var workbook = result.ExportExcel();
            return workbook.Deliver($"PerformanceEvaluation_{tablesName}.xlsx");
        }
    }
}

