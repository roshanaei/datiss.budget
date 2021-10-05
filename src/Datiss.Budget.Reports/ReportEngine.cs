using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Contracts;
using Stimulsoft.Report;

namespace Datiss.Budget.Reports
{
    public class ReportEngine
    {

        private readonly IReportService _reportService;
        private readonly IFinanceYearService _yearService;
        private readonly IOrganizationService _orgService;
        private readonly IConstantService _constantService;

        public ReportEngine(
            IReportService reportService,
            IFinanceYearService yearService,
            IOrganizationService orgService,
            IConstantService constantService) {
            _reportService = reportService
                ?? throw new ArgumentNullException(nameof(reportService));
            _yearService = yearService
                ?? throw new ArgumentNullException(nameof(yearService));
            _orgService = orgService
                ?? throw new ArgumentNullException(nameof(orgService));
            _constantService = constantService
                ?? throw new ArgumentNullException(nameof(constantService));
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

            if(report.Params.Any(_=> _.ParamType == ReportParamType.Year)) {
                result.FinanceYearSource = await _yearService.GetDropDownDataAsync();
            }

            if(report.Params.Any(_=> _.ParamType == ReportParamType.Organization)) {
                result.OrganizationSource = await _orgService.GetDropDownDataAsync();
            }

            if(result.Params.Any(_=> _.ParamType == ReportParamType.Constant)) {
                result.ConstantSource = new Dictionary<string, IEnumerable<Services.Models.DropDownItem>>();
                foreach(var prm in result.Params.Where(_ => _.ParamType == ReportParamType.Constant)) {
                    result.ConstantSource.Add(
                        prm.ConstantKey,
                        (await _constantService.GetByConstantKeyAsync(prm.ConstantKey))
                    );
                }
            }

            return result;
        }

        
        public async Task<StiReport> GenerateReportAsync(ReportViewData model) {
            if (model == null) throw new ArgumentNullException(nameof(model));

        }
    }
}
