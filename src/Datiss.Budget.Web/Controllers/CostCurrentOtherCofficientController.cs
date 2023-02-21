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
using Datiss.Budget.Services.Identity;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Common;
using Datiss.Budget.Enum;
using Datiss.Budget.Security;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CostCurrentOtherCofficientController : Controller
    {
        public const string Name = "CostCurrentOtherCofficient";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteAllRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<CostCurrentOtherCofficientController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostCurrentOtherCofficientService _costCurrentOtherCofficientService;
        private readonly IConstantService _constantService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;


        public CostCurrentOtherCofficientController(
            ILogger<CostCurrentOtherCofficientController> logger,
            IWebHostEnvironment environment,
            ICostCurrentOtherCofficientService costCurrentOtherCofficientService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costCurrentOtherCofficientService = costCurrentOtherCofficientService ?? throw new ArgumentNullException(nameof(_costCurrentOtherCofficientService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostCurrentOtherCofficientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<CreateCostCurrentOtherCofficientDTO>();

            var result = await _costCurrentOtherCofficientService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostCurrentOtherCofficientViewModel>());
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostCurrentOtherCofficientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostCurrentOtherCofficientDTO>();
            var result = await _costCurrentOtherCofficientService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostCurrentOtherCofficientViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostCurrentOtherCofficientFilterDTO();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var costCenterTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var ccOtherCostsTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CCOtherCostsType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.YearId = maxYear;


            var myfilter = TempData.Get<CostCurrentOtherCofficientFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostCurrentOtherCofficientFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costCurrentOtherCofficientService.GetListAsync(filter);
            var model = result.Adapt<CostCurrentOtherCofficientIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetCostCenterTypeSource(costCenterTypeSource);
            model.SetCCOtherCostsTypeSource(ccOtherCostsTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);

            model.Filter.YearId = filter.YearId;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = filter.PageSize;

            return View(model);
        }
        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CostCurrentOtherCofficientIndexViewModel model)
        {
            var filter = model.Filter.Adapt<CostCurrentOtherCofficientFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costCurrentOtherCofficientService.GetListAsync(filter);
            model = result.Adapt<CostCurrentOtherCofficientIndexViewModel>();
            model.Filter = filter.Adapt<CostCurrentOtherCofficientFilterViewModel>();


            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var costCenterTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var ccOtherCostsTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CCOtherCostsType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetCostCenterTypeSource(costCenterTypeSource);
            model.SetCCOtherCostsTypeSource(ccOtherCostsTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);


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
                var result = await _costCurrentOtherCofficientService.ImportExcelAsync(
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
        public async Task<IActionResult> DeleteAllRecords(int yearId)
        {
            try
            {
                var result = await _costCurrentOtherCofficientService.HardDeleteAllAsync(yearId);

                return Json(new
                {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForOrg,
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
                await _costCurrentOtherCofficientService.HardDeleteAsync(id);
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
                await _costCurrentOtherCofficientService.CopyAsync(
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

        [HttpGet("import/template/{yearId}")]
        public async Task<IActionResult> GetExcelTemplate(int yearId)
        {
            var year = await _financeYearService.GetByIdAsync(yearId);
            var costCenterTypes = await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType);
            var ccOtherCostsType = await _constantService.GetByConstantKeyAsync(ConstantKeys.__CCOtherCostsType);

            var items = new List<CostCurrentOtherCofficientDTO>();

            foreach (var org in costCenterTypes)
            {
                foreach (var usert in costCenterTypes)
                {
                    foreach (var item in ccOtherCostsType)
                    {
                        items.Add(new CostCurrentOtherCofficientDTO
                        {
                            CostCenterTypeDisplay = usert.Title,
                            CostCenterTypeId = usert.Id,
                            CCOtherCostsTypeId = item.Id,
                            CCOtherCostsTypeDisplay = item.Title,
                            Year = year.Year,
                            YearId = year.Id
                        });
                    }
                }
            }

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("CostCurrentOtherCofficient-Import-Template.xlsx");
        }

        [HttpGet("[action]/{yearid}")]
        public async Task<IActionResult> ExportExcel(int yearid)
        {
            var result = await _costCurrentOtherCofficientService.GetExportItemsAsync(yearid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostCurrentOtherCofficient.xlsx");
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
