using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public interface ITotalBudgetWsReportService
    {
        Task<TotalBudgetWsReport> GetByIdAsync(int id);
        Task<ValidationResult<TotalBudgetWsReportDTO>> UpdateAsync(UpdateTotalBudgetWsReportDTO model);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);
        Task<PagedResult<TotalBudgetWsReportDTO>> GetListAsync(TotalBudgetWsReportFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);
        Task<ValidationResult> CalculationAsync(int yearId, int organizationId);
        Task<IEnumerable<TotalBudgetWsReportDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
