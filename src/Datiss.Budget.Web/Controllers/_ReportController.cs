using System;
using System.Linq;
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
using Microsoft.AspNetCore.Hosting;
using Stimulsoft.Base;
using System.IO;
using Newtonsoft.Json;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize]
    [Route("[controller]")]  
    public class _ReportController : Controller
    {

        public const string Name = "Report";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Report = nameof(Report);
        public const string ACTION_ShowReport = nameof(ShowReport);

        private readonly string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly IWebHostEnvironment _host;
        private readonly IReportEngine _reportEngine;
        private readonly IReportService _reportService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _yearService;

        public _ReportController(
            IWebHostEnvironment host,
            IReportEngine reportEngine,
            IReportService reportService,
            IOrganizationService organizationService,
            IFinanceYearService yearService) {
            _host = host
                ?? throw new ArgumentNullException(nameof(host));
            _reportEngine = reportEngine 
                ?? throw new ArgumentNullException(nameof(reportEngine));
            _reportService = reportService
                ?? throw new ArgumentNullException(nameof(reportService));
            _organizationService = organizationService
                ?? throw new ArgumentNullException(nameof(organizationService));
            _yearService = yearService
                ?? throw new ArgumentNullException(nameof(yearService));

            var stimulLicenseKey = Path.Combine(_host.WebRootPath, "reporting\\license.key");
            StiLicense.LoadFromFile(stimulLicenseKey);
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            var filter = new ReportFilterDTO();
            var myfilter = TempData.Get<ReportFilterViewModel>(_indexFilterKey);
            if (myfilter != null) {
                filter = myfilter.Adapt<ReportFilterDTO>();
                TempData.Put(_indexFilterKey, filter);
            }
            filter.PageNumber = page;
            var result = await _reportService.GetUserListAsync(filter);
            var model = result.Adapt<ReportIndexViewModel>();
            model.Filter = filter.Adapt<ReportFilterViewModel>();


            return View(model);
        }

        [HttpGet("Index2")]
        public async Task<IActionResult> Index2() {

            return PartialView("_Index");
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

                if (Request.Query["show_report"].Any()) {
                    var showReport = Request.Query["show_report"].ToString();

                    if (showReport.ToUpper() == "true".ToUpper()) {
                        var data = new ReportViewData
                        {
                            Id = report.Id,
                            Name = report.Name,
                            TemplateFileData = report.FileData,
                            Title = report.Title,
                            Description = report.Description,
                            Params = report.Params.Select(_ => new ReportParamViewData
                            { 
                                Id = _.Id,
                                Name = _.Name,
                                ParamType = _.ParamType,
                                ReportId = report.Id,
                                Title = _.Title,
                                Value = Request.Query[_.Name].ToString()
                            }).ToList()
                        };

                        Dictionary<string, string> param_vals = new Dictionary<string, string>();
                        foreach (var p in report.Params) {
                            if (!Request.Query.Keys.Contains(p.Name))
                                continue;
                            var fval = Request.Query[p.Name];
                            param_vals.Add(p.Name, fval.ToString());
                        }

                        TempData.Put("report_values", param_vals);

                        model.DisplayReport = true;

                        return View(model);
                    }
                }

                return View(model);
            }
            catch {
                return NotFound();
            }
        }

        [HttpPost("show/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int id, ReportDisplayViewModel model) {
            model.CheckArgumentIsNull(nameof(model));
            var report = await _reportService.GetAsync(model.Report.Id);
            model.Report = report.Adapt<ReportViewModel>();
            await addRelatedDataAsync(model);

            TempData["reportId"] = model.Report.Id;
            
            var data = new ReportViewData
            {
                Id = report.Id,
                Name = report.Name,
                TemplateFileData = report.FileData,
                Title = report.Title,
                Description = report.Description,
                Params = report.Params.Select(_=> new ReportParamViewData
                {
                    Id = _.Id,
                    Name = _.Name,
                    ParamType = _.ParamType,
                    ReportId = report.Id,
                    Title = _.Title,
                    Value = Request.Form[_.Name].ToString()
                }).ToList()
            };

            Dictionary<string, string> param_vals = new Dictionary<string, string>();
            foreach(var p in report.Params) {
                if (!Request.Form.Keys.Contains(p.Name))
                    continue;
                var fval = Request.Form[p.Name];
                param_vals.Add(p.Name, fval.ToString());
            }

            TempData.Put("report_values", param_vals);

            model.DisplayReport = true;
            TempData["display_report"] = model.DisplayReport;

            //return RedirectToAction(ACTION_Report, new { id = report.Id });

            return RedirectToAction("Index2");

            return PartialView("_Index");

            //var _rp = await _reportEngine.GenerateReportAsync(data);

            //await _rp.ExportDocumentAsync(
            //     Stimulsoft.Report.StiExportFormat.Pdf, 
            //     @"E:\Projects\Datiss\datiss.budget\src\Datiss.Budget.Web\wwwroot\001.pdf"
            //    );

            return View(model);
        }

        [HttpPost("showreport")]
        public async Task<IActionResult> ShowReport(ShowReportViewModel model) {
            var report = await _reportService.GetAsync(model.Id);
            TempData["reportId"] = report.Id;

            if(model.Form == null) {
                model.Form = new Dictionary<string, string>
                {
                    { "asdasd", "3" }
                };
            }
            
            var data = new ReportViewData
            {
                Id = report.Id,
                Name = report.Name,
                TemplateFileData = report.FileData,
                Title = report.Title,
                Description = report.Description,
                Params = report.Params.Select(_ => new ReportParamViewData
                {
                    Id = _.Id,
                    Name = _.Name,
                    ParamType = _.ParamType,
                    ReportId = report.Id,
                    Title = _.Title,
                    Value = model.Form[_.Name].ToString()
                }).ToList()
            };

            Dictionary<string, string> param_vals = new Dictionary<string, string>();
            foreach (var p in report.Params) {
                if (!model.Form.Keys.Contains(p.Name))
                    continue;
                var fval = Request.Form[p.Name];
                param_vals.Add(p.Name, fval.ToString());
            }

            TempData.Put("report_values", param_vals);

            return PartialView("_Index");
        }

        [Route("fuck")]
        public async Task<IActionResult> Fuck() {
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

            HttpContext.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            var info = _host.WebRootFileProvider.GetFileInfo("report-templates/sample-report.mrt");
            if(info.Exists) {
                using var stream = info.CreateReadStream();
                await stream.CopyToAsync(HttpContext.Response.Body);
                await HttpContext.Response.CompleteAsync();
            }

            return Ok();
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

        [Route("GetReport")]
        public async Task<IActionResult> GetReport() {
            object objId = 0;
            if (!TempData.TryGetValue("reportId", out objId))
                return NotFound();

            var id = Convert.ToInt32(objId);
            var report = await _reportService.GetAsync(id);

            object values = null;
            var paramVals = new Dictionary<string, string>();

            paramVals = JsonConvert.DeserializeObject<Dictionary<string, string>>
                                        (TempData["report_values"].ToString());
            //if (TempData.TryGetValue("report_values", out values)) {
                
            //}

            var data = new ReportViewData
            {
                Id = report.Id,
                Name = report.Name,
                TemplateFileData = report.FileData,
                Title = report.Title,
                Description = report.Description,
                Params = report.Params.Select(_ => new ReportParamViewData
                {
                    Id = _.Id,
                    Name = _.Name,
                    ParamType = _.ParamType,
                    ReportId = report.Id,
                    Title = _.Title,
                    Value = paramVals.FirstOrDefault(__=> __.Key == _.Name)
                }).ToList()
            };
            var myreport = await _reportEngine.GenerateReportAsync(data);

            myreport.Render(false);

            //return await StiNetCoreViewer.GetReportResultAsync(this, myreport);

            //return StiNetCoreViewer.GetReportResult(this, myreport);

            return StiNetCoreReportResponse.ResponseAsHtml(myreport);
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
