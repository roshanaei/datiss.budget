using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Contracts;
using Stimulsoft.Report;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Stimulsoft.Report.Dictionary;
using Datiss.Budget.Reports.Contracts;

namespace Datiss.Budget.Reports
{
    public class ReportEngine : IReportEngine {

        private readonly IWebHostEnvironment _env;
        private readonly IReportService _reportService;
        private readonly IFinanceYearService _yearService;
        private readonly IOrganizationService _orgService;
        private readonly IConstantService _constantService;
        private readonly IConfiguration _config;

        public ReportEngine(
            IWebHostEnvironment env,
            IConfiguration config,
            IReportService reportService,
            IFinanceYearService yearService,
            IOrganizationService orgService,
            IConstantService constantService) {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _reportService = reportService
                ?? throw new ArgumentNullException(nameof(reportService));
            _yearService = yearService
                ?? throw new ArgumentNullException(nameof(yearService));
            _orgService = orgService
                ?? throw new ArgumentNullException(nameof(orgService));
            _constantService = constantService
                ?? throw new ArgumentNullException(nameof(constantService));
        }

        private string getConnectionString() 
            => _config.GetConnectionString("ApplicationDbContextConnection");

        public async Task<ReportViewData> GetViewDataAsync(int reportId) {
            var report = await _reportService.GetAsync(reportId);

            var result = new ReportViewData {
                Id = report.Id,
                Name = report.Name,
                Title = report.Title,
                Description = report.Description,
                Params = report.Params.Select(_=> new ReportParamViewData {
                    ConstantKey = _.ConstantKey,
                    Id = _.Id,
                    Name = _.Name,
                    ParamType = _.ParamType,
                    ReportId = _.ReportId,
                    Title = _.Title
                }).ToList()
            };

            //if(report.Params.Any(_=> _.ParamType == ReportParamType.Year)) {
            //    result.FinanceYearSource = await _yearService.GetDropDownDataAsync();
            //}

            //if(report.Params.Any(_=> _.ParamType == ReportParamType.Organization)) {
            //    result.OrganizationSource = await _orgService.GetDropDownDataAsync();
            //}

            //if(result.Params.Any(_=> _.ParamType == ReportParamType.Constant)) {
            //    result.ConstantSource = new Dictionary<string, IEnumerable<Services.Models.DropDownItem>>();
            //    foreach(var prm in result.Params.Where(_ => _.ParamType == ReportParamType.Constant)) {
            //        result.ConstantSource.Add(
            //            prm.ConstantKey,
            //            (await _constantService.GetByConstantKeyAsync(prm.ConstantKey))
            //        );
            //    }
            //}

            return result;
        }

        
        public async Task<StiReport> GenerateReportAsync(ReportViewData model) {
            if (model == null) 
                throw new ArgumentNullException(nameof(model));

            if (model.TemplateFileData == null ||
                model.TemplateFileData.Length <= 0)
                    throw new ArgumentNullException(nameof(model.TemplateFileData));

            //var report = await _reportService.GetAsync(model.Id);

            var _report = new StiReport();
            _report.Dictionary.Databases.Add(new StiSqlDatabase(
                "Budget", 
                getConnectionString())
            );

            _report.Load(model.TemplateFileData);

            foreach (var prm in model.Params) {
                _report[prm.Name] = prm.Value;
            }

            return _report;
        }
    }
}
