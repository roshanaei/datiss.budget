using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastPipingWService
    {
        Task<CostForcastPipingW> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastPipingWDTO>> CreateAsync(CreateCostForcastPipingWDTO model);

        Task<ValidationResult<CostForcastPipingWDTO>> UpdateAsync(UpdateCostForcastPipingWDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastPipingWDTO>> GetListAsync(CostForcastPipingWFilterDTO filter);

        Task CopyAsync(int sourceYearId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId);

        Task<IEnumerable<CostForcastPipingWDTO>> GetExportItemsAsync(int yearId);

    }
}
