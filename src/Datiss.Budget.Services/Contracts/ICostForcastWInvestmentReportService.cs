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
    public interface ICostForcastWInvestmentReportService
    {
        Task<CostForcastWInvestmentReport> GetByIdAsync(int id);
        Task<ValidationResult<CostForcastWInvestmentReportDTO>> UpdateAsync(UpdateCostForcastWInvestmentReportDTO model);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);
        Task<PagedResult<CostForcastWInvestmentReportDTO>> GetListAsync(CostForcastWInvestmentReportFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);
        Task<ValidationResult> CalculationAsync(int yearId, int organizationId);
        Task<IEnumerable<CostForcastWInvestmentReportDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
