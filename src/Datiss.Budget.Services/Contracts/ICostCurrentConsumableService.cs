using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentConsumableService
    {
        Task<CostCurrentConsumable> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentConsumableDTO>> CreateAsync(CreateCostCurrentConsumableDTO model);

        Task<ValidationResult<CostCurrentConsumableDTO>> UpdateAsync(UpdateCostCurrentConsumableDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostCurrentConsumableDTO>> GetListAsync(CostCurrentConsumableFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId, ActivityType activityType);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId, ActivityType activityType);

        Task<IEnumerable<CostCurrentConsumableDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, ActivityType activityType, bool continueIfAnyOrgMissing = false);
    }
}
