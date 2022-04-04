using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentContractualService
    {
        Task<CostCurrentContractual> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentContractualDTO>> CreateAsync(CreateCostCurrentContractualDTO model);

        Task<ValidationResult<CostCurrentContractualDTO>> UpdateAsync(UpdateCostCurrentContractualDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentContractualDTO>> GetListAsync(CostCurrentContractualFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentContractualFilterDTO filter);

        Task<IEnumerable<CostCurrentContractualDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
