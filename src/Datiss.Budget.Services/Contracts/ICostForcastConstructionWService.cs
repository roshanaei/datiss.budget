using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastConstructionWService
    {
        Task<CostForcastConstructionW> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastConstructionWDTO>> CreateAsync(CreateCostForcastConstructionWDTO model);

        Task<ValidationResult<CostForcastConstructionWDTO>> UpdateAsync(UpdateCostForcastConstructionWDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastConstructionWDTO>> GetListAsync(CostForcastConstructionWFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostForcastConstructionWDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
