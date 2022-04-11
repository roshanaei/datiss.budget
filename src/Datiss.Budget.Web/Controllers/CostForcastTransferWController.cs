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
    public class CostForcastTransferWController : Controller
    {

        public const string Name = "CostForcastTransferW";
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

        private readonly ILogger<CostForcastTransferWController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostForcastTransferWService _costForcastTransferWService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostForcastTransferWController(
            ILogger<CostForcastTransferWController> logger,
            IWebHostEnvironment environment,
            ICostForcastTransferWService costForcastTransferWService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costForcastTransferWService = costForcastTransferWService ?? throw new ArgumentNullException(nameof(costForcastTransferWService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostForcastTransferWViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostForcastTransferWDTO>();

            var result = await _costForcastTransferWService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostForcastTransferWViewModel>());
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostForcastTransferWViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostForcastTransferWDTO>();
            var result = await _costForcastTransferWService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostForcastTransferWViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostForcastTransferWFilterDTO();
            var orgSource = (await _organizationService.GetDropDownDataAsync())
              .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            #region dropdown

            var transferTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TransferType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var digTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__DigType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var diameterPipeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WaterTubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var tubeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var creditSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CreditType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var extensionTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__ExtensionType);
            var extensionTypeSource = extensionTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var extensionTypeKeys = "";
            foreach (var key in extensionTypeData)
            {
                extensionTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["extensionTypeKeys"] = extensionTypeKeys.TrimEnd(',');


            var suggestedBudgetTopicSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__SuggestedBudgetTopicType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            #endregion

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
               .Adapt<List<DropDownItemViewModel>>();

            var myfilter = TempData.Get<CostForcastTransferWFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostForcastTransferWFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _costForcastTransferWService.GetListAsync(filter);
            var model = result.Adapt<CostForcastTransferWIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);

            model.SetTransferTypeSource(transferTypeSource);
            model.SetDigTypeSource(digTypeSource);
            model.SetTubeTypeSource(tubeTypeSource);
            model.SetExtensionTypeSource(extensionTypeSource);
            model.SetDiameterPipeTypeSource(diameterPipeTypeSource);
            model.SetCreditTypeSource(creditSource);
            model.SetSuggestedBudgetTopicTypeSource(suggestedBudgetTopicSource);

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
        public async Task<IActionResult> Index(CostForcastTransferWIndexViewModel model)
        {

            var filter = model.Filter.Adapt<CostForcastTransferWFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costForcastTransferWService.GetListAsync(filter);
            model = result.Adapt<CostForcastTransferWIndexViewModel>();
            model.Filter = filter.Adapt<CostForcastTransferWFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            #region dropdown

            var transferTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TransferType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var digTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__DigType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var diameterPipeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WaterTubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var tubeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TubeType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var creditSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CreditType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var extensionTypeData = await _constantService.GetDataByKeyAsync(ConstantKeys.__ExtensionType);
            var extensionTypeSource = extensionTypeData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var extensionTypeKeys = "";
            foreach (var key in extensionTypeData)
            {
                extensionTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["extensionTypeKeys"] = extensionTypeKeys.TrimEnd(',');


            var suggestedBudgetTopicSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__SuggestedBudgetTopicType))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            #endregion

            var inputOrgSource = (await _organizationService.GetDropDownInputDataAsync(filter.OrganizationId))
               .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.SetTransferTypeSource(transferTypeSource);
            model.SetDigTypeSource(digTypeSource);
            model.SetTubeTypeSource(tubeTypeSource);
            model.SetExtensionTypeSource(extensionTypeSource);
            model.SetDiameterPipeTypeSource(diameterPipeTypeSource);
            model.SetCreditTypeSource(creditSource);
            model.SetSuggestedBudgetTopicTypeSource(suggestedBudgetTopicSource);

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
                var result = await _costForcastTransferWService.ImportExcelAsync(
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
                var result = await _costForcastTransferWService.HardDeleteAsync(yearId, orgId);

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
                await _costForcastTransferWService.HardDeleteAsync(id);
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

            var result = await _costForcastTransferWService.CalculationAsync(
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
                await _costForcastTransferWService.CopyAsync(
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

            var model = new CostForcastTransferWImportViewModel();
            var items = new List<CostForcastTransferWViewModel>();

            foreach (var org in organizations)
            {
                items.Add(new CostForcastTransferWViewModel
                {
                    OrganizationId = org.Id,
                    OrganizationDisplay = org.Title
                });
            }

            var transferTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TransferType))
                .Adapt<IList<DropDownItemViewModel>>();

            var digTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__DigType))
                .Adapt<IList<DropDownItemViewModel>>();

            var diameterPipeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__WaterTubeType))
                .Adapt<IList<DropDownItemViewModel>>();


            var tubeTypeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__TubeType))
                .Adapt<IList<DropDownItemViewModel>>();


            var creditSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CreditType))
                .Adapt<IList<DropDownItemViewModel>>();


            var extensionSource = (await _constantService.GetDataByKeyAsync(ConstantKeys.__ExtensionType))
                .Adapt<IList<DropDownItemViewModel>>();

            var suggestedBudgetTopicSource = (await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__SuggestedBudgetTopicType,
                                            ConstantKeys.__ExtensionYes)).Adapt<IList<DropDownItemViewModel>>();
            var extensionNoSource = (await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__FinanceSubjectType,
                                            ConstantKeys.__ExtensionNo)).Adapt<IList<DropDownItemViewModel>>();
            foreach (var item in extensionNoSource)
            {
                suggestedBudgetTopicSource.Add(item);
            }
            model.TransferTypeSource = transferTypeSource;
            model.DigTypeSource = digTypeSource;
            model.DiameterPipeTypeSource = diameterPipeTypeSource;
            model.TubeTypeSource = tubeTypeSource;
            model.CreditTypeSource = creditSource;
            model.ExtensionTypeSource = extensionSource;
            model.SuggestedBudgetTopicTypeSource = suggestedBudgetTopicSource;

            model.Items = items;
            using var workbook = model.GetImportTemplate(year.Year);
            return workbook.Deliver("CostForcastTransferW-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _costForcastTransferWService.GetExportItemsAsync(yearid, orgid);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostForcastTransferW.xlsx");
        }

        [HttpPost, Route("GetSuggestedBudgetTopicAsync")]
        public async Task<JsonResult> GetSuggestedBudgetTopicAsync(string key)
        {
            IEnumerable<DropDownItem> result = new DropDownItem[] { };

            if (key.ToUpper().Trim() == ConstantKeys.__ExtensionYes.Trim().ToUpper())
            {
                key = key.Replace(".", "");
                result = await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__SuggestedBudgetTopicType, key);
            }
            else
            {
                key = ConstantKeys.__ExtensionNo;
                result = await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__FinanceSubjectType, key);
            }
            return new JsonResult(result);
        }

        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                "CostForcastTransferW_Cal1" => SPTitles.CostForcastConstructionW_Cal1,
                "CostForcastTransferW_Cal2" => SPTitles.CostForcastConstructionW_Cal2,
                "CostForcastTransferW_Cal3" => SPTitles.CostForcastConstructionW_Cal3,
                "CostForcastTransferW_Cal4" => SPTitles.CostForcastConstructionW_Cal4,
                "CostForcastTransferW_Cal5" => SPTitles.CostForcastConstructionW_Cal5,
                "CostForcastTransferW_Cal6" => SPTitles.CostForcastConstructionW_Cal6,
                "CostForcastTransferW_Cal7" => SPTitles.CostForcastConstructionW_Cal7,
                "CostForcastTransferW_Cal8" => SPTitles.CostForcastConstructionW_Cal8,
                _ => ""
            };
        #endregion

    }
}
