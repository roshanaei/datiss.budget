using ClosedXML.Extensions;
using Datiss.Budget.Common;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.Reports.Excel;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Identity;
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

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class IncomeCurrentWsHController : Controller
    {
        public const string Name = "IncomeCurrentWsH";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<IncomeCurrentWsHController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IIncomeCurrentWsHService _incomeCurrentWsHService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public IncomeCurrentWsHController(
            ILogger<IncomeCurrentWsHController> logger,
            IWebHostEnvironment environment,
            IIncomeCurrentWsHService incomeCurrentWsHService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _incomeCurrentWsHService = incomeCurrentWsHService ?? throw new ArgumentNullException(nameof(incomeCurrentWsHService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateIncomeCurrentWsHViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<CreateIncomeCurrentWsHDTO>();

            var result = await _incomeCurrentWsHService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<IncomeCurrentWsHViewModel>());
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateIncomeCurrentWsHViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateIncomeCurrentWsHDTO>();
            var result = await _incomeCurrentWsHService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<IncomeCurrentWsHViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]

        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new IncomeCurrentWsHFilterDTO();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(x => x.Id);

            var userTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var usageLayerTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType,true))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            var myfilter = TempData.Get<IncomeCurrentWsHFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<IncomeCurrentWsHFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _incomeCurrentWsHService.GetListAsync(filter);
            var model = result.Adapt<IncomeCurrentWsHIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetUsageLayerTypeSource(usageLayerTypeSource);

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
        public async Task<IActionResult> Index(IncomeCurrentWsHIndexViewModel model)
        {
            var filter = model.Filter.Adapt<IncomeCurrentWsHFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _incomeCurrentWsHService.GetListAsync(filter);
            model = result.Adapt<IncomeCurrentWsHIndexViewModel>();
            model.Filter = filter.Adapt<IncomeCurrentWsHFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var userTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UserType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var usageLayerTypeSource = (await _constantService.GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType,true))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            model.SetUserTypeSource(userTypeSource);
            model.SetUsageLayerTypeSource(usageLayerTypeSource);


            return View(model);
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]

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
                var result = await _incomeCurrentWsHService.ImportExcelAsync(
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
        }

        [HttpPost("records/delete")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]

        public async Task<IActionResult> DeleteRecords(int yearId, int orgId)
        {
            try
            {
                var result = await _incomeCurrentWsHService.HardDeleteAsync(yearId, orgId);

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


        [HttpPost("[action]/{id}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _incomeCurrentWsHService.HardDeleteAsync(id);
            }
            catch (DisbaledYearDataInputException)
            {
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.Logic_InputDisableYearData
                });
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

            var result = await _incomeCurrentWsHService.CalculationAsync(
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
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]

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
                await _incomeCurrentWsHService.CopyAsync(
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

        [HttpGet("import/template/{yearId}/{orgId?}")]
        public async Task<IActionResult> GetExcelTemplate(int yearId, int? orgId)
        {
            var year = await _financeYearService.GetByIdAsync(yearId);
            var organizations = await _organizationService.GetWithChildrenAsync(orgId, input: true);

            var userTypes = await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UserType);
            var houseUsageLayerTypes = await _constantService.GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType,true);

            var items = new List<IncomeCurrentWsHDTO>();

            foreach (var org in organizations)
            {
                foreach (var usert in userTypes)
                {
                    foreach (var hult in houseUsageLayerTypes)
                    {
                        items.Add(new IncomeCurrentWsHDTO
                        {
                            UserTypeDisplay = usert.Title,
                            UserTypeId = usert.Id,
                            OrganizationId = org.Id,
                            OrganizationDisplay = org.Title,
                            UsageLayerId = hult.Id,
                            UsageLayerDisplay = hult.Title,
                            Year = year.Year,
                            YearId = year.Id
                        });
                    }
                }
            }

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("IncomeCurrentWsH-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _incomeCurrentWsHService.GetExportItemsAsync(yearid, orgid);

            if (result.Count() == 0)
                return RedirectToAction("Index");

            using var workbook = result.ExportExcel();
            return workbook.Deliver("IncomeCurrentWsH.xlsx");

        }


        [HttpPost, Route("GetUsageLayerAsync")]
        public async Task<JsonResult> GetUsageLayerAsync(string key)
        {
            var result = await _constantService
                .GetByKeyAsync(key, ConstantKeys.__UsageLayerType);

            return new JsonResult(result);
        }

        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                "IncomeCurrentWsH_Cal1" => SPTitles.IncomeCurrentWsH_Cal1,
                "IncomeCurrentWsH_Cal2" => SPTitles.IncomeCurrentWsH_Cal2,
                "IncomeCurrentWsH_Cal3" => SPTitles.IncomeCurrentWsH_Cal3,
                "IncomeCurrentWsH_Cal4" => SPTitles.IncomeCurrentWsH_Cal4,
                "IncomeCurrentWsH_Cal5" => SPTitles.IncomeCurrentWsH_Cal5,
                "IncomeCurrentWsH_Cal6" => SPTitles.IncomeCurrentWsH_Cal6,
                "IncomeCurrentWsH_Cal7" => SPTitles.IncomeCurrentWsH_Cal7,
                "IncomeCurrentWsH_Cal8" => SPTitles.IncomeCurrentWsH_Cal8,
                _ => ""
            };
        #endregion
    }
}
