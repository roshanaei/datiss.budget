using Datiss.Budget.Common;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;
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
using Datiss.Budget.Reports.Excel;
using ClosedXML.Extensions;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class IncomeCurrentWHController : Controller
    {
        public const string Name = "IncomeCurrentWH";
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

        private readonly ILogger<IncomeCurrentWHController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IIncomeCurrentWHService _incomeCurrentWHService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;


        public IncomeCurrentWHController(
            ILogger<IncomeCurrentWHController> logger,
            IWebHostEnvironment environment,
            IIncomeCurrentWHService incomeCurrentWHService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _incomeCurrentWHService = incomeCurrentWHService ?? throw new ArgumentNullException(nameof(incomeCurrentWHService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateIncomeCurrentWHViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<CreateIncomeCurrentWHDTO>();

            var result = await _incomeCurrentWHService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<IncomeCurrentWHViewModel>());
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateIncomeCurrentWHViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateIncomeCurrentWHDTO>();
            var result = await _incomeCurrentWHService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<IncomeCurrentWHViewModel>()
            );
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new IncomeCurrentWHFilterDTO();

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

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var myfilter = TempData.Get<IncomeCurrentWHFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<IncomeCurrentWHFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _incomeCurrentWHService.GetListAsync(filter);
            var model = result.Adapt<IncomeCurrentWHIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetUserTypeSource(userTypeSource);

            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IncomeCurrentWHIndexViewModel model, int page = 1)
        {
            model.Filter.PageNumber = 1;
            var filter = model.Filter.Adapt<IncomeCurrentWHFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _incomeCurrentWHService.GetListAsync(filter);
            model = result.Adapt<IncomeCurrentWHIndexViewModel>();
            model.Filter = filter.Adapt<IncomeCurrentWHFilterViewModel>();

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

            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(true))
                .Adapt<List<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            model.SetUserTypeSource(userTypeSource);

            return View(model);
        }

        [HttpPost("[action]")]
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
                var result = await _incomeCurrentWHService.ImportExcelAsync(
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
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId)
        {
            try
            {
                var result = await _incomeCurrentWHService.HardDeleteAsync(yearId, orgId);

                return Json(new
                {
                    success = true,
                    message = string.Format(
                        ViewMessages.DeleteMultipleDataForOrg,
                        result.OrganizationTitle,
                        result.Year)
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _incomeCurrentWHService.HardDeleteAsync(id);
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

            var result = await _incomeCurrentWHService.CalculationAsync(
                model.YearId,
                model.OrganizationId);

            var output = new CalculationResultViewModel
            {
                Result = result,
                Title = "IncomeCurrentWH calc" //TODO : change it to proper title
            };

            return PartialView("_calculationModal", output);
        }

        [HttpGet("[action]")]
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
                await _incomeCurrentWHService.CopyAsync(
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
            var userTypes = await _constantService.GetDataByKeyAsync(ConstantKeys.__UserType);

            var houseUsageLayer = await _constantService.GetByKeyAsync(ConstantKeys.__House, ConstantKeys.__UsageLayerType);
            var nhouseUsagelayer = await _constantService.GetByKeyAsync(ConstantKeys.__UsageLayerType, ConstantKeys.__UsageLayerType);

            var items = new List<IncomeCurrentWHDTO>();

            foreach (var org in organizations)
            {
                userTypes.Where(ut => ut.ConstantKey == ConstantKeys.__House)
                    .ToList()
                    .ForEach(ut => items.AddRange(houseUsageLayer
                                        .Select(hul => new IncomeCurrentWHDTO
                                        {
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
                                        .Select(hul => new IncomeCurrentWHDTO
                                        {
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

            using var workbook = items.GetImportTemplate(year.Year);
            return workbook.Deliver("IncomeCurrentWH-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid)
        {
            var result = await _incomeCurrentWHService.GetExportItemsAsync(yearid, orgid);

            if (result.Count() == 0)
                return RedirectToAction("Index");

            using var workbook = result.ExportExcel();
            return workbook.Deliver("IncomeCurrentWH.xlsx");

        }

        [HttpPost, Route("GetUsageLayerAsync")]
        public async Task<JsonResult> GetUsageLayerAsync(string key)
        {
            var result = await _constantService
                .GetByKeyAsync(key, ConstantKeys.__UsageLayerType);

            return new JsonResult(result);
        }

    }
}
