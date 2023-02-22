using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentOtherCofficientService
    {

        Task<CostCurrentOtherCofficient> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentOtherCofficientDTO>> CreateAsync(CreateCostCurrentOtherCofficientDTO model);

        Task<ValidationResult<CostCurrentOtherCofficientDTO>> UpdateAsync(UpdateCostCurrentOtherCofficientDTO model);

        Task HardDeleteAsync(int Id);

        Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId);

        Task<PagedResult<CostCurrentOtherCofficientDTO>> GetListAsync(CostCurrentOtherCofficientFilterDTO filter);

        Task CopyAsync(int sourceYearId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentOtherCofficientFilterDTO filter);

        Task<IEnumerable<CostCurrentOtherCofficientDTO>> GetExportItemsAsync(int yearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId);
    }

}
