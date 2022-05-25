using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastPipingWsService
    {
        Task<CostForcastPipingWs> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastPipingWsDTO>> CreateAsync(CreateCostForcastPipingWsDTO model);

        Task<ValidationResult<CostForcastPipingWsDTO>> UpdateAsync(UpdateCostForcastPipingWsDTO model);

        Task HardDeleteAsync(int Id);

        Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId);

        Task<PagedResult<CostForcastPipingWsDTO>> GetListAsync(CostForcastPipingWsFilterDTO filter);

        Task CopyAsync(int sourceYearId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId);

        Task<IEnumerable<CostForcastPipingWsDTO>> GetExportItemsAsync(int yearId);

    }
}
