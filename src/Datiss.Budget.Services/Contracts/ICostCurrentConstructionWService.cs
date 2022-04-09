using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentConstructionWService
    {
        Task<CostCurrentConstructionW> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentConstructionWDTO>> CreateAsync(CreateCostCurrentConstructionWDTO model);

        Task<ValidationResult<CostCurrentConstructionWDTO>> UpdateAsync(UpdateCostCurrentConstructionWDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostCurrentConstructionWDTO>> GetListAsync(CostCurrentConstructionWFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentConstructionWFilterDTO filter);

        Task<IEnumerable<CostCurrentConstructionWDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
