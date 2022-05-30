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
    public class CostForcastBuyDescriptionController : Controller
    {

        public const string Name = "CostForcastBuyDescription";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<CostForcastBuyDescriptionController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostForcastBuyDescriptionService _costForcastBuyDescriptionService;
        private readonly IConstantService _constantService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostForcastBuyDescriptionController(
            ILogger<CostForcastBuyDescriptionController> logger,
            IWebHostEnvironment environment,
            ICostForcastBuyDescriptionService costForcastBuyDescriptionService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costForcastBuyDescriptionService = costForcastBuyDescriptionService ?? throw new ArgumentNullException(nameof(costForcastBuyDescriptionService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostForcastBuyDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostForcastBuyDescriptionDTO>();

            var result = await _costForcastBuyDescriptionService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostForcastBuyDescriptionViewModel>());
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostForcastBuyDescriptionViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostForcastBuyDescriptionDTO>();
            var result = await _costForcastBuyDescriptionService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostForcastBuyDescriptionViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostForcastBuyDescriptionFilterDTO();


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            #region dropdown

            var measurementTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__MeasurementType))
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

            var myfilter = TempData.Get<CostForcastBuyDescriptionFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostForcastBuyDescriptionFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costForcastBuyDescriptionService.GetListAsync(filter);
            var model = result.Adapt<CostForcastBuyDescriptionIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetMeasurementTypeSource(measurementTypeSource);
            model.SetAssetTypeSource(assetTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);

            model.Filter.YearId = filter.YearId;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CostForcastBuyDescriptionIndexViewModel model)
        {

            var filter = model.Filter.Adapt<CostForcastBuyDescriptionFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costForcastBuyDescriptionService.GetListAsync(filter);
            model = result.Adapt<CostForcastBuyDescriptionIndexViewModel>();
            model.Filter = filter.Adapt<CostForcastBuyDescriptionFilterViewModel>();


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            #region dropdown

            var measurementTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__MeasurementType))
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

            model.SetYearSource(yearSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);

            model.SetMeasurementTypeSource(measurementTypeSource);
            model.SetAssetTypeSource(assetTypeSource);

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
                var result = await _costForcastBuyDescriptionService.ImportExcelAsync(
                                                                    model.ExcelFile,
                                                                    model.YearId);

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
                var result = await _costForcastBuyDescriptionService.HardDeleteAllAsync(yearId);

                return Json(new
                {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForYear,
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
                await _costForcastBuyDescriptionService.HardDeleteAsync(id);
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

        [HttpGet("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Copy()
        {
            var model = new CopyViewModel();


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
                await _costForcastBuyDescriptionService.CopyAsync(
                                                    model.SourceYearId,
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
        public async Task<IActionResult> GetExcelTemplate(int yearId)
        {
            var year = await _financeYearService.GetByIdAsync(yearId);

            var model = new CostForcastBuyDescriptionImportViewModel();

            var measurementeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__MeasurementType))
                .Adapt<IList<DropDownItemViewModel>>();

            var assetSource = (await _constantService.GetDataByKeyAsync(ConstantKeys.__FinanceSubjectType))
                .Adapt<IList<DropDownItemViewModel>>();

            var assetDetailSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__FinanceSubjectDetailType))
                .Adapt<IList<DropDownItemViewModel>>();

            var items = new List<CostForcastBuyDescriptionDTO>();

            foreach (var assest in assetSource)
            {
                foreach (var assestDetail in assetDetailSource)
                {
                        items.Add(new CostForcastBuyDescriptionDTO
                        {
                            AssetTypeDisplay = assest.Title,
                            AssetTypeId = assest.Id,
                            AssetDetailTypeDisplay = assestDetail.Title,
                            AssetDetailTypeId = assestDetail.Id,
                        });
                }
            }

            using var workbook = model.GetImportTemplate(year.Year);
            return workbook.Deliver("CostForcastBuyDescription-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int yearid)
        {
            var result = await _costForcastBuyDescriptionService.GetExportItemsAsync(yearid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostForcastBuyDescription.xlsx");
        }

        [HttpPost, Route("GetAssetDetailAsync")]
        public async Task<JsonResult> GetAssetDetailAsync(string key)
        {
            var result = await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__FinanceSubjectDetailType, key.Replace(".", ""));

            return new JsonResult(result);
        }

    }
}