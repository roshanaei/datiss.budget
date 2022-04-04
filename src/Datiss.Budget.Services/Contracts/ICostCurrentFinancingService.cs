using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentFinancingService
    {
        Task<CostCurrentFinancing> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentFinancingDTO>> CreateAsync(CreateCostCurrentFinancingDTO model);

        Task<ValidationResult<CostCurrentFinancingDTO>> UpdateAsync(UpdateCostCurrentFinancingDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentFinancingDTO>> GetListAsync(CostCurrentFinancingFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentFinancingFilterDTO filter);

        Task<IEnumerable<CostCurrentFinancingDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
