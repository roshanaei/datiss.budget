using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Stimulsoft.Report.Mvc;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Reports.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Reports;
using Datiss.Budget.Enum;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize]
    [Route("[controller]")]  
    public class ReportController : Controller
    {

        public const string Name = "Report";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Report = nameof(Report);

        private readonly string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly IReportEngine _reportEngine;
        private readonly IReportService _reportService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _yearService;

        public ReportController(
            IReportEngine reportEngine,
            IReportService reportService,
            IOrganizationService organizationService,
            IFinanceYearService yearService) {
            _reportEngine = reportEngine 
                ?? throw new ArgumentNullException(nameof(reportEngine));
            _reportService = reportService
                ?? throw new ArgumentNullException(nameof(reportService));
            _organizationService = organizationService
                ?? throw new ArgumentNullException(nameof(organizationService));
            _yearService = yearService
                ?? throw new ArgumentNullException(nameof(yearService));
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            var filter = new ReportFilterDTO();
            var myfilter = TempData.Get<ReportFilterViewModel>(_indexFilterKey);
            if(myfilter != null) {
                filter = myfilter.Adapt<ReportFilterDTO>();
                TempData.Put(_indexFilterKey, filter);
            }
            filter.PageNumber = page;
            var result = await _reportService.GetUserListAsync(filter);
            var model = result.Adapt<ReportIndexViewModel>();
            model.Filter = filter.Adapt<ReportFilterViewModel>();

            return View(model);
        }

        [HttpPost("{page?}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReportIndexViewModel model, int page = 1) {
            model.CheckArgumentIsNull(nameof(model));

            var filter = model.Filter.Adapt<ReportFilterDTO>();
            TempData.Put(_indexFilterKey, filter);

            var result = await _reportService.GetUserListAsync(filter);
            model = result.Adapt<ReportIndexViewModel>();
            model.Filter = filter.Adapt<ReportFilterViewModel>();

            return View(model);
        }

        [HttpGet("show/{id}")]
        public async Task<IActionResult> Report(int id) {
            try {
                var report = await _reportService.GetAsync(id);
                var model = new ReportDisplayViewModel
                {
                    Report = report.Adapt<ReportViewModel>()
                };
                await addRelatedDataAsync(model);

                return View(model);
            }
            catch {
                return NotFound();
            }
        }

        [HttpPost("show/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int id, ReportViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            return View(model);
        }


        //[HttpGet("show/{id}")]
        //public async Task<IActionResult> Report(int id) {



        //    var model = new ReportViewData {
        //        Id = id,
        //        Params = new List<ReportParamViewData> {
        //            new ReportParamViewData {
        //                ParamType = Enum.ReportParamType.Year,
        //                Name = "YearId",
        //                Value = 1
        //            },
        //            new ReportParamViewData {
        //                ParamType = Enum.ReportParamType.Organization,
        //                Name = "OrganizationId",
        //                Value = 5
        //            },
        //            new ReportParamViewData {
        //                ParamType = Enum.ReportParamType.Constant,
        //                Name = "UserTypeId",
        //                Value = 3
        //            }
        //        }
        //    };

        //    //var myreport = await _reportEngine.GenerateReportAsync(model);

        //    //myreport.Render(true);

        //    //await myreport.ExportDocumentAsync(
        //    //     Stimulsoft.Report.StiExportFormat.Pdf, @"E:\Projects\Datiss\datiss.budget\src\Datiss.Budget.Web\wwwroot\iman.pdf");

        //    return View(model);
        //}

        [HttpPost("report1")]
        public async Task<IActionResult> Report1(int id) {

            return PartialView("_report1");
        }

        [HttpGet("getreport")]
        public async Task<IActionResult> GetReport() {
            var model = new ReportViewData {
                Id = 1,
                Params = new List<ReportParamViewData> {
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Year,
                        Name = "YearId",
                        Value = 1
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Organization,
                        Name = "OrganizationId",
                        Value = 5
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Constant,
                        Name = "UserTypeId",
                        Value = 3
                    }
                }
            };

            //var myreport = await _reportEngine.GenerateReportAsync(model);

            var myreport = await _reportEngine.GenerateReportAsync(model);

            myreport.Render(false);

            //return await StiNetCoreViewer.GetReportResultAsync(this, myreport);

            return StiNetCoreViewer.GetReportResult(this, myreport);
        }

        [Route("ViewerEvent")]
        public IActionResult ViewerEvent() {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        [HttpGet("report1")]
        public IActionResult Report1() {
            return PartialView("_report1");
        }

        [Route("GetReport1")]
        public IActionResult GetReport1() {
            var model = new ReportViewData
            {
                Id = 1,
                Params = new List<ReportParamViewData> {
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Year,
                        Name = "YearId",
                        Value = 1
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Organization,
                        Name = "OrganizationId",
                        Value = 5
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Constant,
                        Name = "UserTypeId",
                        Value = 3
                    }
                }
            };

            //var myreport = await _reportEngine.GenerateReportAsync(model);

            var myreport = _reportEngine.GenerateReportAsync(model).Result;

            myreport.Render(false);

            return StiNetCoreViewer.GetReportResult(this, myreport);
        }

        #region private helper methods

        private async Task addRelatedDataAsync(ReportDisplayViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            model.SetYearSource((await _yearService.GetDropDownDataAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>());
            
            model.SetOrganizationSource((await _organizationService
                    .GetDropDownDataAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>());
            
            model.SetCountySource((await _organizationService
                    .GetDropDownDataAsync(input: false, OrganizationType.County))
                    .Adapt<IEnumerable<DropDownItemViewModel>>());

            model.SetCitySource((await _organizationService
                    .GetDropDownDataAsync(input: false, OrganizationType.City))
                    .Adapt<IEnumerable<DropDownItemViewModel>>());

            model.SetVillageSource((await _organizationService
                    .GetDropDownDataAsync(input: false, OrganizationType.Village))
                    .Adapt<IEnumerable<DropDownItemViewModel>>());
        }

        #endregion

    }
}
