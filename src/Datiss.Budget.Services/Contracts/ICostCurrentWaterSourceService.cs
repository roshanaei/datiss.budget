using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentWaterSourceService
    {
        Task<CostCurrentWaterSource> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentWaterSourceDTO>> CreateAsync(CreateCostCurrentWaterSourceDTO model);

        Task<ValidationResult<CostCurrentWaterSourceDTO>> UpdateAsync(UpdateCostCurrentWaterSourceDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentWaterSourceDTO>> GetListAsync(CostCurrentWaterSourceFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostCurrentWaterSourceDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

    }
}
