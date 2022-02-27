using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Entities.DWH;
using System.Threading.Tasks;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using Datiss.Budget.Common;
using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Web.Helpers;
using System.IO;
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;
using Datiss.Budget.Security;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WWsFeeController : Controller
    {
        public const string Name = "WWsFee";
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

        private readonly ILogger<WWsFee> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConsumeForcastService _consumeForcastService;
        private readonly IWWsFeeService _wWsFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public WWsFeeController(
            ILogger<WWsFee> logger,
            IWebHostEnvironment environment,
            IWWsFeeService wWsFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wWsFeeService = wWsFeeService ?? throw new ArgumentNullException(nameof(wWsFeeService));
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

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateWWsFeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateWWsFeeDTO>();

            var result = await _wWsFeeService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<WWsFeeViewModel>());
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateWWsFeeViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateWWsFeeDTO>();
            var result = await _wWsFeeService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<WWsFeeViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new WWsFeeFilterDTO();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(x => x.Id);

            var userTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__UserType);

            var userTypeSource = userTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();

            var userTypeKeys = "";

            foreach (var key in userTypeData)
            {
                userTypeKeys += $"'{key.ConstantKey}',";
            }

            ViewData["userTypeKeys"] = userTypeKeys.TrimEnd(',');

            var activity = new ActivityType();
            var activityTypeSource = EnumSelectListProvider.GetActivityTypeItems(activity);

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            var myfilter = TempData.Get<WWsFeeFilterViewModel>(_indexFilterKey);

            if (myfilter != null)
            {
                filter = myfilter.Adapt<WWsFeeFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _wWsFeeService.GetListAsync(filter);
            var model = result.Adapt<WWsFeeIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetActivityTypeSource(activityTypeSource);
            model.SetUserTypeSource(userTypeSource);

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
        public async Task<IActionResult> Index(WWsFeeIndexViewModel model)
        {
            var filter = model.Filter.Adapt<WWsFeeFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _wWsFeeService.GetListAsync(filter);

            model = result.Adapt<WWsFeeIndexViewModel>();

            model.Filter = filter.Adapt<WWsFeeFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var userTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__UserType);

            var userTypeSource = userTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var userTypeKeys = "";
            foreach (var key in userTypeData)
            {
                userTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["userTypeKeys"] = userTypeKeys.TrimEnd(',');

            var activity = new ActivityType();
            var activityTypeSource = EnumSelectListProvider.GetActivityTypeItems(activity);

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);
            model.SetUserTypeSource(userTypeSource);
            model.SetActivityTypeSource(activityTypeSource);

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
                var result = await _wWsFeeService.ImportExcelAsync(
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
            catch (ImportExcelFileFormatInvalidException)
            {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileFormatInvalid);
                return Json(new
                {
                    hasError = true,
                    message = ViewMessages.ImportExcelFileFormatInvalid
                });
            }
            catch (ImportExcelFileSizeInvalidException)
            {
                showMessage(CssClassNames.Error,
                    ViewMessages.ImportExcelFileSizeInvalid);
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
                var result = await _wWsFeeService.HardDeleteAsync(yearId, orgId);

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
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _wWsFeeService.HardDeleteAsync(id);
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

            var result = await _wWsFeeService.CalculationAsync(
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
        public async Task<IActionResult> DownloadExcelTemplate()
        {
            var filePath = $"{_env.WebRootPath}\\Excel\\WWsFeeImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WWsFee.xlsx");
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
                await _wWsFeeService.CopyAsync(
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
            catch (Exception)
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
            var userTypes = await _constantService.GetDataByKeyAsync(ConstantKeys.__UserType);

            var houseUsageLayer = await _constantService.GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType, true);
            var nhouseUsagelayer = await _constantService.GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType);

            var activity = ActivityType.GetValues<ActivityType>();

            var items = new List<WWsFeeDTO>();

            foreach (var org in organizations)
            {
                foreach (var act in activity)
                {
                    userTypes.Where(ut => ut.ConstantKey == ConstantKeys.__House)
                        .ToList()
                        .ForEach(ut => items.AddRange(houseUsageLayer
                                            .Select(hul => new WWsFeeDTO
                                            {
                                                ActivityType = act,
                                                ActivityTypeDisplay = act.ToDisplay(),
                                                UserTypeDisplay = ut.Title,
                                                UserTypeId = ut.Id,
                                                UsageLayerDisplay = hul.Title,
                                                UsageLayerId = hul.Id,
                                                OrganizationId = org.Id,
                                                OrganizationDisplay = org.Title,
                                                Year = year.Year,
                                                YearId = year.Id
                                            }).ToList())
                        );
                    userTypes.Where(ut => ut.ConstantKey != ConstantKeys.__House)
                        .ToList()
                        .ForEach(ut => items.AddRange(nhouseUsagelayer
                                            .Select(hul => new WWsFeeDTO
                                            {
                                                ActivityType = act,
                                                ActivityTypeDisplay = act.ToDisplay(),
                                                UserTypeDisplay = ut.Title,
                                                UserTypeId = ut.Id,
                                                UsageLayerDisplay = hul.Title,
                                                UsageLayerId = hul.Id,
                                                OrganizationId = org.Id,
                                                OrganizationDisplay = org.Title,
                                                Year = year.Year,
                                                YearId = year.Id
                                            }).ToList())
                        );
                }
            }
            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("WWsFee-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _wWsFeeService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("WWsFee.xlsx");
        }

        [HttpPost, Route("GetUsageLayerAsync")]
        public async Task<JsonResult> GetUsageLayerAsync(string key)
        {
            bool isHouse = false;
            if (key == ConstantKeys.__House)
                isHouse = true;
            var result = await _constantService
                .GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType, isHouse);

            return new JsonResult(result);
        }

        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                //"WaterInstallFees_Cal1" => SPTitles.WaterInstallFees_Cal1,
                //_ => ""
            };
        #endregion


    }
}
