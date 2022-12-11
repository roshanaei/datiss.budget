using Datiss.Budget.Common;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
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
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CostForcastWsInvestmentReportController : Controller
    {
        public const string Name = "CostForcastWsInvestmentReport";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<CostForcastWsInvestmentReportController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostForcastWsInvestmentReportService _costForcastWsInvestmentReportService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostForcastWsInvestmentReportController(
            ILogger<CostForcastWsInvestmentReportController> logger,
            IWebHostEnvironment environment,
            ICostForcastWsInvestmentReportService costForcastWsInvestmentReportService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costForcastWsInvestmentReportService = costForcastWsInvestmentReportService ?? throw new ArgumentNullException(nameof(costForcastWsInvestmentReportService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostForcastWsInvestmentReportViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostForcastWsInvestmentReportDTO>();
            var result = await _costForcastWsInvestmentReportService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostForcastWsInvestmentReportViewModel>()
            );
        }


        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostForcastWsInvestmentReportFilterDTO();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(x => x.Id);
            int numberYear = Convert.ToInt32(yearSource.Max(x => x.Title));

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.NumberYear = numberYear;
            filter.OrganizationId = firstOrgId;

            var myfilter = TempData.Get<CostForcastWsInvestmentReportFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostForcastWsInvestmentReportFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costForcastWsInvestmentReportService.GetListAsync(filter);
            var model = result.Adapt<CostForcastWsInvestmentReportIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.NumberYear = filter.NumberYear;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.PageNumber = filter.PageNumber;
            model.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CostForcastWsInvestmentReportIndexViewModel model)
        {
            var filter = model.Filter.Adapt<CostForcastWsInvestmentReportFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costForcastWsInvestmentReportService.GetListAsync(filter);
            model = result.Adapt<CostForcastWsInvestmentReportIndexViewModel>();
            model.Filter = filter.Adapt<CostForcastWsInvestmentReportFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            //
            if (filter.YearId.HasValue)
            {
                var year = yearSource.SingleOrDefault(_ => _.Id == filter.YearId);
                model.Filter.NumberYear = Convert.ToInt32(year.Title);
            }

            return View(model);
        }

        [HttpPost("records/delete")]
        [HasPermission(claimType: Name, PermissionActionType.Delete)]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId)
        {
            try
            {
                var result = await _costForcastWsInvestmentReportService.HardDeleteAsync(yearId, orgId);

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
            catch (Exception ex)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.DeleteRelatedData
                });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Calculation(CalculationInputViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            await _costForcastWsInvestmentReportService.CalculationAsync(
                        model.YearId,
                        model.OrganizationId);

            return RedirectToAction("Index");
        }

        [HttpGet("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
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
                await _costForcastWsInvestmentReportService.CopyAsync(
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
            catch (CopyDataBaseException)
            {
                model.AddError(ViewMessages.CalculationField);
            }
            catch (Exception ex)
            {
                model.AddError(ViewMessages.SystemError);
            }

            return Json(model);
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
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
                var result = await _costForcastWsInvestmentReportService.ImportExcelAsync(
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
            catch (Exception ex)
            {
                return Json(new
                {
                    hasError = true,
                    message = ex.Message
                });
            }
        }

        [HttpGet("import/template/{yearId}/{orgId?}")]
        public async Task<IActionResult> GetExcelTemplate(int yearId, int? orgId)
        {
            var year = await _financeYearService.GetByIdAsync(yearId);
            var organizations = await _organizationService.GetWithChildrenAsync(orgId, input: true);
            var sectionTypes = await _constantService.GetDataByKeyAsync(ConstantKeys.__WsInvestmentReportType);
            var costCenterTypes = await _constantService.GetDataByKeyAsync(ConstantKeys.__CostCenterType);

            var items = new List<CostForcastWsInvestmentReportDTO>();

            foreach (var org in organizations)
            {
                foreach (var center in costCenterTypes)
                {
                    foreach (var sec in sectionTypes)
                    {                       
                            items.Add(new CostForcastWsInvestmentReportDTO
                            {
                                OrganizationDisplay = org.Title,
                                OrganizationId = org.Id,
                                CostCenterTypeDisplay = center.Title,
                                CostCenterTypeId = center.Id,
                                SectionTypeDisplay = sec.Title,
                                SectionTypeId = sec.Id,
                            });
                    }
                }
            }

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("CostForcastWsInvestmentReport-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _costForcastWsInvestmentReportService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostForcastWsInvestmentReports.xlsx");
        }
    }
}
