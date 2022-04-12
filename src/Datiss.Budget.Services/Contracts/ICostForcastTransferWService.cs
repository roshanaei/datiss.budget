using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastTransferWService
    {
        Task<CostForcastTransferW> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastTransferWDTO>> CreateAsync(CreateCostForcastTransferWDTO model);

        Task<ValidationResult<CostForcastTransferWDTO>> UpdateAsync(UpdateCostForcastTransferWDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastTransferWDTO>> GetListAsync(CostForcastTransferWFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostForcastTransferWDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
