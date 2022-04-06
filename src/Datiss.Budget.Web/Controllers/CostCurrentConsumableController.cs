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
    [Authorize]
    [Route("[controller]")]
    public class CostCurrentConsumableController : Controller
    {
        public const string Name = "CostCurrentConsumable";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<CostCurrentConsumableController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostCurrentConsumableService _costCurrentConsumableService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;
        private readonly IConstantService _constantService;


        public CostCurrentConsumableController(
            ILogger<CostCurrentConsumableController> logger,
            IWebHostEnvironment environment,
            ICostCurrentConsumableService costCurrentConsumableService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costCurrentConsumableService = costCurrentConsumableService ?? throw new ArgumentNullException(nameof(costCurrentConsumableService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostCurrentConsumableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostCurrentConsumableDTO>();

            var result = await _costCurrentConsumableService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostCurrentConsumableViewModel>());
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostCurrentConsumableViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostCurrentConsumableDTO>();
            var result = await _costCurrentConsumableService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostCurrentConsumableViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostCurrentConsumableFilterDTO();
            var orgSource = (await _organizationService.GetDropDownDataAsync())
              .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var consumableTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ConsumablesType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;
            filter.ActivityType = ActivityType.Water;

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            var myfilter = TempData.Get<CostCurrentConsumableFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostCurrentConsumableFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costCurrentConsumableService.GetListAsync(filter);
            var model = result.Adapt<CostCurrentConsumableIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetConsumableTypeSource(consumableTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = filter.PageSize;
            model.Filter.ActivityType = filter.ActivityType;
            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CostCurrentConsumableIndexViewModel model, int page = 1)
        {
            model.Filter.PageNumber = 1;
            var filter = model.Filter.Adapt<CostCurrentConsumableFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costCurrentConsumableService.GetListAsync(filter);
            model = result.Adapt<CostCurrentConsumableIndexViewModel>();
            model.Filter = filter.Adapt<CostCurrentConsumableFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var consumableTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ConsumablesType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetConsumableTypeSource(consumableTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            return View(model);
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
        public async Task<IActionResult> ImportExcel(ImportExcelViewModel model, ActivityType activityType)
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
                var result = await _costCurrentConsumableService.ImportExcelAsync(
                                                                    model.ExcelFile,
                                                                    model.YearId,
                                                                    activityType,
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
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId, ActivityType activityType)
        {
            try
            {
                var result = await _costCurrentConsumableService.HardDeleteAsync(yearId, orgId, activityType);

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

        [HttpPost("[action]/{id}")]
        [HasPermission(claimType: Name, PermissionActionType.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _costCurrentConsumableService.HardDeleteAsync(id);
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

            var result = await _costCurrentConsumableService.CalculationAsync(
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
        public async Task<IActionResult> Copy(CopyViewModel model, ActivityType activityType)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                await _costCurrentConsumableService.CopyAsync(
                                                    model.SourceYearId,
                                                    model.SourceOrgId,
                                                    model.TargetYearId,
                                                    activityType);
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
            catch (Exception)
            {
                model.AddError(ViewMessages.SystemError);
            }

            return Json(model);
        }

        [HttpGet("import/template/{yearId}/{activity}/{orgId?}")]
        public async Task<IActionResult> GetExcelTemplate(int yearId, ActivityType activity, int? orgId)
        {
            var year = await _financeYearService.GetByIdAsync(yearId);
            var organizations = (await _organizationService.GetWithChildrenAsync(orgId, input: true))
                                .OrderBy(x => x.DisplayOrder)
                                .ThenBy(x => x.RowOrder);

            var consumableType = await _constantService.GetByConstantKeyAsync(ConstantKeys.__ConsumablesType);

            var items = new List<CostCurrentConsumableDTO>();

            foreach (var org in organizations)
            {
                foreach (var type in consumableType)
                {
                    items.Add(new CostCurrentConsumableDTO
                    {
                        OrganizationId = org.Id,
                        OrganizationDisplay = org.Title,
                        ActivityType = activity,
                        Year = year.Year,
                        YearId = year.Id,
                        ConsumableTypeDisplay = type.Title,
                        ConsumableTypeId = type.Id
                    });
                }
            }
            using var workbook = items.GetImportTemplate(year.Year, activity);
            return workbook.Deliver("CostCurrentConsumable-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _costCurrentConsumableService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostCurrentConsumable.xlsx");
        }

        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                "CostCurrentConsumable_Cal1" => SPTitles.CostCurrentConsumable_Cal1,
                "CostCurrentConsumable_Cal2" => SPTitles.CostCurrentConsumable_Cal2,
                "CostCurrentConsumable_Cal3" => SPTitles.CostCurrentConsumable_Cal3,
                "CostCurrentConsumable_Cal4" => SPTitles.CostCurrentConsumable_Cal4,
                "CostCurrentConsumable_Cal5" => SPTitles.CostCurrentConsumable_Cal5,
                "CostCurrentConsumable_Cal6" => SPTitles.CostCurrentConsumable_Cal6,
                _ => ""
            };
        #endregion
    }
}
