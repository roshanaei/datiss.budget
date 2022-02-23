using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentConsumableService
    {
        Task<CostCurrentConsumable> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentConsumableDTO>> CreateAsync(CreateCostCurrentConsumableDTO model);

        Task<ValidationResult<CostCurrentConsumableDTO>> UpdateAsync(UpdateCostCurrentConsumableDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostCurrentConsumableDTO>> GetListAsync(CostCurrentConsumableFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostCurrentConsumableDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
