using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWasteSalesSplitService
    {

        Task<WasteSalesSplit> GetByIdAsync(int id);
        Task<ValidationResult<WasteSalesSplitDTO>> CreateAsync(CreateWasteSalesSplitDTO model);
        Task<ValidationResult<WasteSalesSplitDTO>> UpdateAsync(UpdateWasteSalesSplitDTO model);
        Task HardDeleteAsync(int Id);
        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);
        Task<int> CalculationAsync(int yearId, int organizationId);
        Task<PagedResult<WasteSalesSplitDTO>> GetListAsync(WasteSalesSplitFilterDTO filter);
        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);
        Task<Stream> ExportExcelAsync(WasteSalesSplitFilterDTO filter);
        Task<IEnumerable<WasteSalesSplitDTO>> GetExportItemsAsync(int yearId, int organizationId);
        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false);
    }
}
