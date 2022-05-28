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
using Datiss.Budget.Services.Contracts.Identity;
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
    public class CostForcastPipingWsController : Controller
    {

        public const string Name = "CostForcastPipingWs";
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

        private readonly ILogger<CostForcastPipingWsController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostForcastPipingWsService _costForcastPipingWsService;
        private readonly IConstantService _constantService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostForcastPipingWsController(
            ILogger<CostForcastPipingWsController> logger,
            IWebHostEnvironment environment,
            ICostForcastPipingWsService costForcastPipingWsService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costForcastPipingWsService = costForcastPipingWsService ?? throw new ArgumentNullException(nameof(costForcastPipingWsService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostForcastPipingWsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostForcastPipingWsDTO>();

            var result = await _costForcastPipingWsService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostForcastPipingWsViewModel>());
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostForcastPipingWsViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostForcastPipingWsDTO>();
            var result = await _costForcastPipingWsService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostForcastPipingWsViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostForcastPipingWsFilterDTO();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            #region dropdown


            var digTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__DigType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var diameterPipeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WasteTubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var tubeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            #endregion

            filter.YearId = maxYear;

            var myfilter = TempData.Get<CostForcastPipingWsFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostForcastPipingWsFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costForcastPipingWsService.GetListAsync(filter);
            var model = result.Adapt<CostForcastPipingWsIndexViewModel>();

            model.SetYearSource(yearSource);

            model.SetDigTypeSource(digTypeSource);
            model.SetTubeTypeSource(tubeTypeSource);
            model.SetDiameterPipeTypeSource(diameterPipeTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);

            model.Filter.YearId = filter.YearId;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CostForcastPipingWsIndexViewModel model)
        {

            var filter = model.Filter.Adapt<CostForcastPipingWsFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costForcastPipingWsService.GetListAsync(filter);
            model = result.Adapt<CostForcastPipingWsIndexViewModel>();
            model.Filter = filter.Adapt<CostForcastPipingWsFilterViewModel>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            #region dropdown


            var digTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__DigType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var diameterPipeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WasteTubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var tubeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            #endregion

            model.SetYearSource(yearSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);

            model.SetDigTypeSource(digTypeSource);
            model.SetTubeTypeSource(tubeTypeSource);
            model.SetDiameterPipeTypeSource(diameterPipeTypeSource);

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
                var result = await _costForcastPipingWsService.ImportExcelAsync(
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
        [HasPermission(claimType: Name, PermissionActionType.Delete)]
        public async Task<IActionResult> DeleteRecords(int yearId)
        {
            try
            {
                var result = await _costForcastPipingWsService.HardDeleteAllAsync(yearId);

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
        }

        [HttpPost("[action]/{id}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _costForcastPipingWsService.HardDeleteAsync(id);
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
                await _costForcastPipingWsService.CopyAsync(
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

            var digTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__DigType))
                .Adapt<IList<DropDownItemViewModel>>();

            var diameterPipeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WasteTubeType))
                .Adapt<IList<DropDownItemViewModel>>();


            var tubeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TubeType))
                .Adapt<IList<DropDownItemViewModel>>();

            var items = new List<CostForcastPipingWsDTO>();

            foreach (var tube in tubeTypeSource)
            {
                foreach (var diameter in diameterPipeTypeSource)
                {
                    foreach (var dig in digTypeSource)

                    {
                        items.Add(new CostForcastPipingWsDTO
                        {
                            TubeTypeDisplay = tube.Title,
                            TubeTypeId = tube.Id,
                            DiameterPipeTypeDisplay = diameter.Title,
                            DiameterPipeTypeId = diameter.Id,
                            DigTypeDisplay = dig.Title,
                            DigTypeId = dig.Id,
                        });
                    }
       
                }
            }

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("CostForcastPipingWs-Import-Template.xlsx");
        }

        [HttpGet("[action]/{yearid}")]
        public async Task<IActionResult> ExportExcel( int yearid)
        {
            var result = await _costForcastPipingWsService.GetExportItemsAsync(yearid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostForcastPipingWs.xlsx");
        }


        #region Private Helper Methods

        #endregion

    }
}
