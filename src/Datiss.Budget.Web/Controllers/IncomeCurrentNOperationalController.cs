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
using Datiss.Budget.Enum; 
using Datiss.Budget.Security;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize]
    [Route("[controller]")]
    public class IncomeCurrentNOperationalController : Controller {

        public const string Name = "IncomeCurrentNOperational";
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
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<IncomeCurrentNOperationalController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IIncomeCurrentNOperationalService _incomeCurrentNOperationalService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public IncomeCurrentNOperationalController(
            ILogger<IncomeCurrentNOperationalController> logger,
            IWebHostEnvironment environment,
            IIncomeCurrentNOperationalService incomeCurrentNOperationalService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService) 
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _incomeCurrentNOperationalService = incomeCurrentNOperationalService ?? throw new ArgumentNullException(nameof(incomeCurrentNOperationalService));
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
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateIncomeCurrentNOperationalViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateIncomeCurrentNOperationalDTO>();

            var result = await _incomeCurrentNOperationalService.CreateAsync(data);

            if(! result.IsValid) {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<IncomeCurrentNOperationalViewModel>());
        }

        
        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateIncomeCurrentNOperationalViewModel model) {

            if (!ModelState.IsValid) {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateIncomeCurrentNOperationalDTO>();
            var result = await _incomeCurrentNOperationalService.UpdateAsync(data);

            if(!result.IsValid) {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<IncomeCurrentNOperationalViewModel>()
            ); 
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1) 
        {
            var filter = new IncomeCurrentNOperationalFilterDTO();
            var orgSource = (await _organizationService.GetDropDownDataAsync())
              .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var cionSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CINOType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            var myfilter = TempData.Get<IncomeCurrentNOperationalFilterViewModel>(_indexFilterKey);
            if(myfilter != null) 
            {
                filter = myfilter.Adapt<IncomeCurrentNOperationalFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _incomeCurrentNOperationalService.GetListAsync(filter);
            var model = result.Adapt<IncomeCurrentNOperationalIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetNOICTypeSource(cionSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);
            
            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IncomeCurrentNOperationalIndexViewModel model) {

            var filter = model.Filter.Adapt<IncomeCurrentNOperationalFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _incomeCurrentNOperationalService.GetListAsync(filter);
            model = result.Adapt<IncomeCurrentNOperationalIndexViewModel>();
            model.Filter = filter.Adapt<IncomeCurrentNOperationalFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var cionSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CINOType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);
            model.SetNOICTypeSource(cionSource);
            
            return View(model);
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (model.ExcelFile == null || model.ExcelFile.Length == 0)
                return Json(new {
                    hasError = true,
                    message = ViewMessages.ImportExcelInvalidFile
                });

            try {
                var result = await _incomeCurrentNOperationalService.ImportExcelAsync(
                                                                    model.ExcelFile, 
                                                                    model.YearId, 
                                                                    model.ContinueIfAnyOrgMissing);

                if(result.AskToImport) {
                    return Json(new {
                        ask = true,
                        message = result.Message
                    });
                }

                if(result.Success) 
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
            catch (ImportExcelFileFormatInvalidException) {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileFormatInvalid);
                return Json(new {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileFormatInvalid
                });
            }
            catch (ImportExcelFileSizeInvalidException) {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileSizeInvalid);
                return Json(new {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileSizeInvalid
                });
            }

        }

        [HttpPost("records/delete")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId) {
            try {
                var result = await _incomeCurrentNOperationalService.HardDeleteAsync(yearId, orgId);

                return Json(new {
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
                    hasError = true ,
                    message = ViewMessages.DeleteNullRecord
                });
            }
            catch(NullReferenceException) {
                return Json(new {
                    hasError = true,
                    message = ViewMessages.NullRef
                });
            }
            catch(Exception) {
                return Json(new {
                    hasError = true,
                    message = ViewMessages.DeleteRelatedData
                });
            }
        }

        [HttpPost("[action]/{id}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]
        public async Task<IActionResult> Delete(int id) {
            try {
                await _incomeCurrentNOperationalService.HardDeleteAsync(id);
            }
            catch(DisbaledYearDataInputException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.Logic_InputDisableYearData
                });
            }
            catch(Exception ex) {
                _logger.LogError(ex.GetBaseException().Message);
                return Json(new {
                    hasError = true,
                    message = ViewMessages.InvalidUpdateData
                });
            }

            return Json(new {
                hasError = false,
                message = ViewMessages.DeleteRowSuccess
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Calculation(CalculationInputViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var result = await _incomeCurrentNOperationalService.CalculationAsync(
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
        public async Task<IActionResult> DownloadExcelTemplate() {
            var filePath = $"{_env.WebRootPath}\\Excel\\IncomeCurrentNOperationalImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                "IncomeCurrentNOperational.xlsx");
        }

        [HttpGet("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Copy() {
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
        public async Task<IActionResult> Copy(CopyViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            try {
                await _incomeCurrentNOperationalService.CopyAsync(
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
            catch (CopyDataBaseException)
            {
                model.AddError(ViewMessages.CalculationField);
            }
            catch (Exception) {
                model.AddError(ViewMessages.SystemError);
            }
            
            return Json(model);
        }

        [HttpGet("import/template/{yearId}/{orgId?}")]
        public async Task<IActionResult> GetExcelTemplate(int yearId, int? orgId) {
            var year = await _financeYearService.GetByIdAsync(yearId);
            var organizations = await _organizationService.GetWithChildrenAsync(orgId, input: true);
            var cinoTypes = await _constantService.GetByConstantKeyAsync(ConstantKeys.__CINOType);

            var items = new List<IncomeCurrentNOperationalDTO>();

            foreach(var org in organizations) {
                foreach(var cinot in cinoTypes) {
                    items.Add(new IncomeCurrentNOperationalDTO
                    {
                        NOICTypeDisplay = cinot.Title,
                        NOICTypeId = cinot.Id,
                        OrganizationId = org.Id,
                        OrganizationDisplay = org.Title,
                        Year = year.Year,
                        YearId = year.Id
                    });
                }
            }

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("IncomeCurrentNOperational-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid) {
            var result = await _incomeCurrentNOperationalService.GetExportItemsAsync(yearid,orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("IncomeCurrentNOperational.xlsx");
        }

        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                //"IncomeCurrentNOperationals_Cal1" => SPTitles.IncomeCurrentNOperationals_Cal1,
                //_ => ""
            };
        #endregion

    }
}
