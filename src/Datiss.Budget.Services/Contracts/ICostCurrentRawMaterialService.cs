using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentRawMaterialService
    {
        Task<CostCurrentInstalation> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentInstalationDTO>> CreateAsync(CreateCostCurrentInstalationDTO model);

        Task<ValidationResult<CostCurrentInstalationDTO>> UpdateAsync(UpdateCostCurrentInstalationDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentInstalationDTO>> GetListAsync(CostCurrentInstalationFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostCurrentInstalationDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
