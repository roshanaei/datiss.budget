using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastConstructionWsService
    {
        Task<CostForcastConstructionWs> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastConstructionWsDTO>> CreateAsync(CreateCostForcastConstructionWsDTO model);

        Task<ValidationResult<CostForcastConstructionWsDTO>> UpdateAsync(UpdateCostForcastConstructionWsDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastConstructionWsDTO>> GetListAsync(CostForcastConstructionWsFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostForcastConstructionWsDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
