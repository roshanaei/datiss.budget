using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public interface ICostCurrentProfitLossReportService
    {
        Task<CostCurrentProfitLossReport> GetByIdAsync(int id);
        Task<ValidationResult<CostCurrentProfitLossReportDTO>> UpdateAsync(UpdateCostCurrentProfitLossReportDTO model);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);
        Task<PagedResult<CostCurrentProfitLossReportDTO>> GetListAsync(CostCurrentProfitLossReportFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);
        Task<ValidationResult> CalculationAsync(int yearId, int organizationId);
        Task<IEnumerable<CostCurrentProfitLossReportDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
