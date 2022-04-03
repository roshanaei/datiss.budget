using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentOtherService
    {

        Task<CostCurrentOther> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentOtherDTO>> CreateAsync(CreateCostCurrentOtherDTO model);

        Task<ValidationResult<CostCurrentOtherDTO>> UpdateAsync(UpdateCostCurrentOtherDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostCurrentOtherDTO>> GetListAsync(CostCurrentOtherFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentOtherFilterDTO filter);

        Task<IEnumerable<CostCurrentOtherDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

    }

}
