using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentSharingSetadService
    {
        Task<CostCurrentSharingSetad> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentSharingSetadDTO>> CreateAsync(CreateCostCurrentSharingSetadDTO model);

        Task<ValidationResult<CostCurrentSharingSetadDTO>> UpdateAsync(UpdateCostCurrentSharingSetadDTO model);

        Task HardDeleteAsync(int yearId);

        Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId);

        Task<PagedResult<CostCurrentSharingSetadDTO>> GetListAsync(CostCurrentSharingSetadFilterDTO filter);

        Task CopyAsync(int sourceYearId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentSharingSetadFilterDTO filter);

        Task<IEnumerable<CostCurrentSharingSetadDTO>> GetExportItemsAsync(int yearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId);
    }
}
