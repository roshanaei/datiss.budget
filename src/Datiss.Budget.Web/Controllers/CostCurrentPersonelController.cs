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
using Datiss.Budget.Services;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize]
    [Route("[controller]")]
    public class CostCurrentPersonelController : Controller
    {

        public const string Name = "CostCurrentPersonel";
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

        private readonly ILogger<CostCurrentPersonelController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostCurrentPersonelService _costCurrentPersonelService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostCurrentPersonelController(
            ILogger<CostCurrentPersonelController> logger,
            IWebHostEnvironment environment,
            ICostCurrentPersonelService costCurrentPersonelService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costCurrentPersonelService = costCurrentPersonelService ?? throw new ArgumentNullException(nameof(costCurrentPersonelService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostCurrentPersonelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostCurrentPersonelDTO>();

            var result = await _costCurrentPersonelService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CostCurrentPersonelViewModel>());
        }

        [HttpGet("[action]/{id}")]
        [HasPermission(claimType: Name, PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _costCurrentPersonelService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateCostCurrentPersonelViewModel>();

            #region dropdown

            var costCenterSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IList<DropDownItemViewModel>>();

            var gradeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__GradeType))
                .Adapt<IList<DropDownItemViewModel>>();

            var contractSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ContractType))
                .Adapt<IList<DropDownItemViewModel>>();


            var jobDepartmentSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__JobDepartmentType))
                .Adapt<IList<DropDownItemViewModel>>();


            var jobStatusSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__JobStatusType))
                .Adapt<IList<DropDownItemViewModel>>();


            var jobSatusDetailSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__JobStatusDetailsType))
                .Adapt<IList<DropDownItemViewModel>>();


            var inputOrgSource = (await _organizationService.GetDropDownDataAsync(input : true))
               .Adapt<List<DropDownItemViewModel>>();

            #endregion

            model.SetContractSource(contractSource);
            model.SetCostCenterSource(costCenterSource);
            model.SetGradeSource(gradeSource);
            model.SetInputOrganizationSource(inputOrgSource);
            model.SetJobDepartment(jobDepartmentSource);
            model.SetJobStatusSource(jobStatusSource);
            model.SetJobStatusDetailSource(jobSatusDetailSource);

            return PartialView("_editModal", model);
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateCostCurrentPersonelViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostCurrentPersonelDTO>();
            var result = await _costCurrentPersonelService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostCurrentPersonelViewModel>()
            );
        }

        [HttpGet("{page?}")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.List)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filter = new CostCurrentPersonelFilterDTO();
            var orgSource = (await _organizationService.GetDropDownDataAsync())
              .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);


            filter.YearId = maxYear;
            filter.OrganizationId = firstOrgId;

            var jobStatusData = await _constantService.GetDataByKeyAsync(ConstantKeys.__JobStatusType);
            var jobStatusTypeSource = jobStatusData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var jobStatusTypeKeys = "";
            foreach (var key in jobStatusData)
            {
                jobStatusTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["jobStatusTypeKeys"] = jobStatusTypeKeys.TrimEnd(',');


            var myfilter = TempData.Get<CostCurrentPersonelFilterViewModel>(_indexFilterKey);
            if (myfilter != null)
            {
                filter = myfilter.Adapt<CostCurrentPersonelFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;
            filter.RecordType = RecordType.Forcast;

            var result = await _costCurrentPersonelService.GetListAsync(filter);
            var model = result.Adapt<CostCurrentPersonelIndexViewModel>();

            //model.SetYearSource(yearSource);
            //model.SetOrganizationSource(orgSource);
            model.SetJobStatusTypeSource(jobStatusTypeSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            model.Filter.YearId = filter.YearId;
            model.Filter.OrganizationId = filter.OrganizationId;
            model.Filter.RecordType = filter.RecordType;
            model.Filter.PageNumber = filter.PageNumber;
            model.Filter.PageSize = filter.PageSize;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CostCurrentPersonelIndexViewModel model)
        {

            var filter = model.Filter.Adapt<CostCurrentPersonelFilterDTO>();

            TempData.Put(_indexFilterKey, filter);

            var result = await _costCurrentPersonelService.GetListAsync(filter);
            model = result.Adapt<CostCurrentPersonelIndexViewModel>();
            model.Filter = filter.Adapt<CostCurrentPersonelFilterViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            var jobStatusData = await _constantService.GetDataByKeyAsync(ConstantKeys.__JobStatusType);
            var jobStatusTypeSource = jobStatusData.Select(x => new DropDownItemViewModel
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            var jobStatusTypeKeys = "";
            foreach (var key in jobStatusData)
            {
                jobStatusTypeKeys += $"'{key.ConstantKey}',";
            }
            ViewData["jobStatusTypeKeys"] = jobStatusTypeKeys.TrimEnd(',');


            model.SetJobStatusTypeSource(jobStatusTypeSource);
            model.SetFinanceYearFilterSource(yearSource, filter.YearId);
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);


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
                var result = await _costCurrentPersonelService.ImportExcelAsync(
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
        public async Task<IActionResult> DeleteRecords(int yearId, int orgId,RecordType recordType)
        {
            try
            {
                var result = await _costCurrentPersonelService.HardDeleteAsync(yearId, orgId , recordType);

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
                await _costCurrentPersonelService.HardDeleteAsync(id);
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
        public async Task<IActionResult> Copy(CopyViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                await _costCurrentPersonelService.CopyAsync(
                                                    model.TargetYearId,
                                                    model.SourceOrgId);
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
            
            var lastyearRecords = await _costCurrentPersonelService.GetLastYearBaseItemsAsync(yearId);

            var model = new CostCurrentPersonelImportViewModel();

            var items = new List<CostCurrentPersonelViewModel>();

            foreach (var item in lastyearRecords)
            {
                items.Add(new CostCurrentPersonelViewModel
                {
                    OrganizationId = item.OrganizationId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    GradeTypeId = item.GradeTypeId,
                    PersonelCode = item.PersonelCode,
                    GenderId = item.GenderId,
                    ContractTypeId = item.ContractTypeId,
                    JobDepartmentTypeId = item.JobDepartmentTypeId,
                    JobStatusTypeId = item.JobStatusTypeId,
                    JobStatusDetailTypeId = item.JobStatusDetailTypeId,
                    CostCenterTypeId = item.CostCenterTypeId,
                });
            }

            var costCenterSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__CostCenterType))
                .Adapt<IList<DropDownItemViewModel>>();

            var gradeSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__GradeType))
                .Adapt<IList<DropDownItemViewModel>>();

            var contractSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ContractType))
                .Adapt<IList<DropDownItemViewModel>>();


            var jobDepartmentSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__JobDepartmentType))
                .Adapt<IList<DropDownItemViewModel>>();


            var jobStatusSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__JobStatusType))
                .Adapt<IList<DropDownItemViewModel>>();


            var jobSatusDetailSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__JobStatusDetailsType))
                .Adapt<IList<DropDownItemViewModel>>();

            var organizationSource = organizations.Adapt<IList<DropDownItemViewModel>>();

            model.CostCenterTypeSource = costCenterSource;
            model.GradeTypeSource = gradeSource;
            model.ContractTypeSource = contractSource;
            model.JobDepartmentTypeSource = jobDepartmentSource;
            model.JobStatusTypeSource = jobStatusSource;
            model.JobStatusDetailTypeSource = jobSatusDetailSource;
            model.OrganizationSource = organizationSource;

            model.Items = items;
            using var workbook = model.GetImportTemplate(year.Year);
            return workbook.Deliver("CostCurrentPersonel-Import-Template.xlsx");
        }

        [HttpGet("[action]/{orgid}/{yearid}/{recordType}")]
        public async Task<IActionResult> ExportExcel(int orgid, int yearid , RecordType recordType)
        {
            var result = await _costCurrentPersonelService.GetExportItemsAsync(yearid, orgid , recordType);
            if (result.Count() == 0)
                return RedirectToAction("Index");
            using var workbook = result.ExportExcel();
            return workbook.Deliver("CostCurrentPersonel.xlsx");
        }

        [HttpPost, Route("GetJobStatusDetailType")]
        public async Task<JsonResult> GetJobStatusDetailTypeAsync(string key)
        {
            var result = await _constantService.GetRecordsByKeyAsynce(ConstantKeys.__JobStatusDetailsType, key.Replace(".",""));

            return new JsonResult(result);
        }
    }
}
