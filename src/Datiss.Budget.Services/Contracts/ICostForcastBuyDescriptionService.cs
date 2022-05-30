using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastBuyDescriptionService
    {
        Task<CostForcastBuyDescription> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastBuyDescriptionDTO>> CreateAsync(CreateCostForcastBuyDescriptionDTO model);

        Task<ValidationResult<CostForcastBuyDescriptionDTO>> UpdateAsync(UpdateCostForcastBuyDescriptionDTO model);

        Task HardDeleteAsync(int Id);

        Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId);

        Task<PagedResult<CostForcastBuyDescriptionDTO>> GetListAsync(CostForcastBuyDescriptionFilterDTO filter);

        Task CopyAsync(int sourceYearId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId);

        Task<IEnumerable<CostForcastBuyDescriptionDTO>> GetExportItemsAsync(int yearId);

    }
}
