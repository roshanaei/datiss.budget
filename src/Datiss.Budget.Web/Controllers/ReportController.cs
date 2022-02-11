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
using Datiss.Budget.Common.WebToolkit;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Stimulsoft.Base;
using System.IO;
using Microsoft.AspNetCore.WebUtilities;
using Stimulsoft.Report;

namespace Datiss.Budget.Web.Controllers {

    [Authorize]
    [Route("[controller]")]
    public class ReportController : Controller {

        public const string Name = "Report";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Report = nameof(Report);
        //public const string ACTION_ShowReport = nameof(ShowReport);

        private readonly string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly IWebHostEnvironment _host;
        private readonly IReportEngine _reportEngine;
        private readonly IReportService _reportService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _yearService;

        public ReportController(
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

        [HttpGet("display/{id}")]
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
            catch (Exception ex) {
                return NotFound();
            }
        }

        [HttpPost("display/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Report(int id, ReportDisplayViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var _params = new Dictionary<string, string>();
            var _form = Request.Form.ToList();
            foreach(var f in _form) {
                _params.Add(f.Key, f.Value);
            }
            var queryStr = QueryHelpers.AddQueryString(Url.Action("View"), _params);

            return Redirect(queryStr);
        }

        [HttpGet("view")]
        public IActionResult View() => PartialView("_view");
        
        [Route("view/[action]")]
        public async Task<IActionResult> GetReport() {
            int id = Convert.ToInt32(Request.Query["id"].ToString());
            var report = await _reportService.GetAsync(id);

            var _params = Request.QueryString.QueryStringToDictionary();
            var myreport = await getStiReportAsync(report, _params);

            return await StiNetCoreViewer.GetReportResultAsync(this, myreport);
        }

        [Route("view/[action]")]
        public async Task<IActionResult> ExportReport() {
            int id = Convert.ToInt32(Request.Query["id"].ToString());
            var report = await _reportService.GetAsync(id);

            var _params = Request.QueryString.QueryStringToDictionary();
            var myreport = await getStiReportAsync(report, _params);

            return await StiNetCoreViewer.ExportReportResultAsync(this, myreport);
        }

        [Route("view/[action]")]
        public async Task<IActionResult> PrintReport() {
            int id = Convert.ToInt32(Request.Query["id"].ToString());
            var report = await _reportService.GetAsync(id);

            var _params = Request.QueryString.QueryStringToDictionary();
            var myreport = await getStiReportAsync(report, _params);

            return await StiNetCoreViewer.PrintReportResultAsync(this, myreport);
        }

        [Route("view/[action]")]
        public IActionResult ViewerEvent()
            => StiNetCoreViewer.ViewerEventResult(this);
        
        #region private helper methods

        private async Task<StiReport> getStiReportAsync(
            ReportData model, 
            Dictionary<string, string> parameters) {

            var data = new ReportViewData
            {
                Id = model.Id,
                Name = model.Name,
                TemplateFileData = model.FileData,
                Title = model.Title,
                Description = model.Description,
                Params = model.Params.Select(_ => new ReportParamViewData
                {
                    Id = _.Id,
                    Name = _.Name,
                    ParamType = _.ParamType,
                    ReportId = model.Id,
                    Title = _.Title,
                    Value = parameters[_.Name].ToString()
                }).ToList()
            };

            var report = await _reportEngine.GenerateReportAsync(data);
            report.Render(false);

            return await Task.FromResult(report);
        }

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
