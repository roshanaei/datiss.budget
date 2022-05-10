using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentWaterSourcePriceService
    {
        Task<CostCurrentWaterSourcePrice> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentWaterSourcePriceDTO>> CreateAsync(CreateCostCurrentWaterSourcePriceDTO model);

        Task<ValidationResult<CostCurrentWaterSourcePriceDTO>> UpdateAsync(UpdateCostCurrentWaterSourcePriceDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentWaterSourcePriceDTO>> GetListAsync(DefaultFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(DefaultFilterDTO filter);

        Task<IEnumerable<CostCurrentWaterSourcePriceDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
