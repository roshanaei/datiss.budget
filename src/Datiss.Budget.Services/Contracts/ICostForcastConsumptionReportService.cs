using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public interface ICostForcastConsumptionReportService
    {
        Task<CostForcastConsumptionReport> GetByIdAsync(int id);
        Task<ValidationResult<CostForcastConsumptionReportDTO>> UpdateAsync(UpdateCostForcastConsumptionReportDTO model);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);
        Task<PagedResult<CostForcastConsumptionReportDTO>> GetListAsync(CostForcastConsumptionReportFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);
        Task<ValidationResult> CalculationAsync(int yearId, int organizationId);
        Task<IEnumerable<CostForcastConsumptionReportDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
