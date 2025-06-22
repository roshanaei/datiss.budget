using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{

    public interface IReportService
    {

        Task<ReportData> GetAsync(int id);
        Task<ReportData> GetAsync(int id, int userId);
        Task<ReportData> GetAsync(string name);

        Task<PagedResult<ReportData>> GetAdminListAsync(ReportFilterDTO filter);

        Task<PagedResult<ReportData>> GetUserListAsync(ReportFilterDTO filter);

        Task<ValidationResult<ReportData>> CreateAsync(CreateReportData model);

        Task<ValidationResult<ReportData>> UpdateAsync(UpdateReportData model);

        Task<List<ReportRoleDTO>> GetRolesForReportAsync(int reportId);
        Task UpdateRolesForReportAsync(int reportId, List<int> roleIds);

        Task DeleteAsync(int id);

    }

}
