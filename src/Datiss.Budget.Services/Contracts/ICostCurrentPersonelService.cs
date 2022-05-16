using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public interface ICostCurrentPersonelService
    {
        Task<CostCurrentPersonel> GetByIdAsync(int id);
        Task<ValidationResult<CostCurrentPersonelDTO>> UpdateAsync(UpdateCostCurrentPersonelDTO model);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId, RecordType recordType);
        Task HardDeleteAsync(int Id);
        Task<PagedResult<CostCurrentPersonelDTO>> GetListAsync(CostCurrentPersonelFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId);
        Task<ValidationResult> CalculationAsync(int yearId, int organizationId);
        Task<IEnumerable<CostCurrentPersonelDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
        Task<IEnumerable<CostCurrentPersonelDTO>> GetLastYearBaseItemsAsync(int yearId);
    }
}
