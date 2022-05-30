using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Common.GuardToolkit;
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
    public class CostForcastBuyController : Controller
    {

        public const string Name = "CostForcastBuy";
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

        private readonly ILogger<CostForcastBuyController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostForcastBuyService _costForcastBuyService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostForcastBuyController(
            ILogger<CostForcastBuyController> logger,
            IWebHostEnvironment environment,
            ICostForcastBuyService costForcastBuyService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costForcastBuyService = costForcastBuyService ?? throw new ArgumentNullException(nameof(costForcastBuyService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostForcastBuyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostForcastBuyDTO>();

            var result = await _costForcastBuyService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostForcastBuyViewModel>());
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostForcastBuyViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostForcastBuyDTO>();
            var result = await _costForcastBuyService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostForcastBuyViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostForcastBuyFilterDTO();
            var orgSource = (await _organizationService.GetDropDownDataAsync())
              .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            #region dropdown

            var locationSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                .Adapt<List<DropDownItemViewModel>>();

            var buyDepartmentTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__BuyDepartmentType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var costCenterTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var measurementTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__MeasurementType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var creditSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CreditType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var assetTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__FinanceSubjectType);
            var assetTypeSource = assetTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var assetTypeKeys = "";
            foreach (var key in assetTypeData)
            {
                assetTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["assetTypeKeys"] = assetTypeKeys.TrimEnd(',');

            #endregion

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
               .Adapt<List<DropDownItemViewModel>>();

            var myfilter = TempData.Get<CostForcastBuyFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostForcastBuyFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costForcastBuyService.GetListAsync(filter);
            var model = result.Adapt<CostForcastBuyIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetLocationTypeSource(locationSource);
            model.SetBuyDepartmentTypeSource(buyDepartmentTypeSource);
            model.SeCostCenterTypeSource(costCenterTypeSource);
            model.SetMeasurementTypeSource(measurementTypeSource);
            model.SetAssetTypeSource(assetTypeSource);
            model.SetCreditTypeSource(creditSource);

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
        public async Task<IActionResult> Index(CostForcastBuyIndexViewModel model)
        {

            var filter = model.Filter.Adapt<CostForcastBuyFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costForcastBuyService.GetListAsync(filter);
            model = result.Adapt<CostForcastBuyIndexViewModel>();
            model.Filter = filter.Adapt<CostForcastBuyFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            #region dropdown

            var locationSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
                            .Adapt<List<DropDownItemViewModel>>();

            var buyDepartmentTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__BuyDepartmentType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var costCenterTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var measurementTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__MeasurementType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var creditSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CreditType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var assetTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__FinanceSubjectType);
            var assetTypeSource = assetTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var assetTypeKeys = "";
            foreach (var key in assetTypeData)
            {
                assetTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["assetTypeKeys"] = assetTypeKeys.TrimEnd(',');

            #endregion

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
               .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.SetLocationTypeSource(locationSource);
            model.SetBuyDepartmentTypeSource(buyDepartmentTypeSource);
            model.SeCostCenterTypeSource(costCenterTypeSource);
            model.SetMeasurementTypeSource(measurementTypeSource);
            model.SetAssetTypeSource(assetTypeSource);
            model.SetCreditTypeSource(creditSource);

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
                var result = await _costForcastBuyService.ImportExcelAsync(
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
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId)
        {
            try
            {
                var result = await _costForcastBuyService.HardDeleteAsync(yearId, orgId);

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
                await _costForcastBuyService.HardDeleteAsync(id);
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

            var result = await _costForcastBuyService.CalculationAsync(
                model.YearId,
                model.OrganizationId);

            List<CalculationResultViewModel> viewModel = new List<CalculationResultViewModel>();
            //foreach (var item in result)
            //{
            //    viewModel.Add(
            //        new CalculationResultViewModel
            //        {
            //            Result = item.Value,
            //            Title = getCalcTitle(item.Key)
            //        }
            //    );
            //}

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
                await _costForcastBuyService.CopyAsync(
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
            var organizations = (await _organizationService.GetWithChildrenAsync(orgId, input: true))
                                .OrderBy(x => x.DisplayOrder)
                                .ThenBy(x => x.RowOrder);

            var model = new CostForcastBuyImportViewModel();
            var items = new List<CostForcastBuyViewModel>();

            foreach (var org in organizations)
            {
                items.Add(new CostForcastBuyViewModel
                {
                    OrganizationId = org.Id,
                    OrganizationDisplay = org.Title
                });
            }

            var buyDepartmentTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__BuyDepartmentType))
                .Adapt<IList<DropDownItemViewModel>>();

            var costCenterTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IList<DropDownItemViewModel>>();

            var measurementeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__MeasurementType))
                .Adapt<IList<DropDownItemViewModel>>();

            var creditSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CreditType))
                .Adapt<IList<DropDownItemViewModel>>();

            var assetSource = (await _constantService.GetDataByKeyAsync(ConstantKeys.__FinanceSubjectType))
                .Adapt<IList<DropDownItemViewModel>>();

            var assetDetailSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__FinanceSubjectDetailType))
                .Adapt<IList<DropDownItemViewModel>>();

            model.LocationTypeSource = organizations.Adapt<IList<DropDownItemViewModel>>();
            model.BuyDepartmentTypeSource = buyDepartmentTypeSource;
            model.CostCenterTypeSource = costCenterTypeSource;
            model.MeasurementTypeSource = measurementeTypeSource;
            model.CreditTypeSource = creditSource;
            model.AssetTypeSource = assetSource;
            model.AssetDetailTypeSource = assetDetailSource;

            model.Items = items;
            using var workbook = model.GetImportTemplate(year.Year);
            return workbook.Deliver("CostForcastBuy-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _costForcastBuyService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostForcastBuy.xlsx");
        }

        [HttpPost, Route("GetAssetDetailAsync")]
        public async Task<JsonResult> GetAssetDetailAsync(string key)
        {
            key = key.Substring(key.IndexOf('.') + 1);

            if (key != key.Substring(key.IndexOf('.') + 1))
            {
                key = key.Substring(0, key.IndexOf('.'));
            }


            var result = await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__FinanceSubjectDetailType, key.Replace(".", ""));
            if (result.Count() == 0)
            {
                result = await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__FinanceSubjectDetailType, "Dash");
            }
            return new JsonResult(result);
        }

    }
}