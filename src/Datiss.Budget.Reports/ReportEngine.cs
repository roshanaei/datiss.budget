using System;
using System.Linq;
using System.Threading.Tasks;
using Datiss.Budget.Services.Contracts;
using Stimulsoft.Report;
using Microsoft.Extensions.Configuration;
using Stimulsoft.Report.Dictionary;
using Datiss.Budget.Reports.Contracts;

namespace Datiss.Budget.Reports
{
    public class ReportEngine : IReportEngine {

        private readonly IReportService _reportService;
        private readonly IConfiguration _config;

        public ReportEngine(
            IConfiguration config,
            IReportService reportService) {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _reportService = reportService
                ?? throw new ArgumentNullException(nameof(reportService));
        }

        private string getConnectionString() 
            => _config.GetConnectionString("ApplicationDbContextConnection");

        private void setConnection(StiReport report) {
            var dbAlias = report.Dictionary.Databases.Items[0].Alias;
            var connection = new StiSqlDatabase(dbAlias, getConnectionString());
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(connection);
        }

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

            return result;
        }

        public async Task<StiReport> GenerateReportAsync(ReportViewData model) {
            if (model == null) 
                throw new ArgumentNullException(nameof(model));

            if (model.TemplateFileData == null ||
                model.TemplateFileData.Length <= 0)
                    throw new ArgumentNullException(nameof(model.TemplateFileData));

            var report = new StiReport();
            report.Load(model.TemplateFileData);
            setConnection(report);

            foreach (var prm in model.Params) {
                report[prm.Name] = prm.Value;
            }

            return report;
        }

    }
}
