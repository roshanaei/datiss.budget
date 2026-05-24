using Stimulsoft.Report;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Contracts {

    public interface IReportEngine {

        Task<ReportViewData> GetViewDataAsync(int reportId);
        //..

        Task<StiReport> GenerateReportAsync(ReportViewData model);
    }
}
