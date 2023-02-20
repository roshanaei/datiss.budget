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

        Task<ValidationResult<CostCurrentOtherCofficientDTO>> CreateAsync(CreateCostCurrentOtherDTO model);

        Task<ValidationResult<CostCurrentOtherCofficientDTO>> UpdateAsync(UpdateCostCurrentOtherDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostCurrentOtherCofficientDTO>> GetListAsync(CostCurrentOtherFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentOtherCofficientFilterDTO filter);

        Task<IEnumerable<CostCurrentOtherCofficientDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

    }

}
