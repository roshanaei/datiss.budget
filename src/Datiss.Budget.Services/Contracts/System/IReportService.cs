using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{

    public interface IReportService
    {

        Task<Report> GetAsync(int id);

        Task<Report> GetAsync(string name);

        Task<PagedResult<ReportDTO>> GetAdminListAsync(ReportFilterDTO filter);

        Task<ValidationResult<ReportDTO>> CreateAsync(CreateReportData model);

        Task<ValidationResult<ReportDTO>> UpdateAsync(UpdateReportData model);

    }

}
