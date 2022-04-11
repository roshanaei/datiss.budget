using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastTransferWsService
    {
        Task<CostForcastTransferWs> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastTransferWsDTO>> CreateAsync(CreateCostForcastTransferWsDTO model);

        Task<ValidationResult<CostForcastTransferWsDTO>> UpdateAsync(UpdateCostForcastTransferWsDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastTransferWsDTO>> GetListAsync(CostForcastTransferWsFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostForcastTransferWsDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}