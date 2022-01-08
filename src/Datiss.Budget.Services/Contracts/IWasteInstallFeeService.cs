using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWasteInstallFeeService
    {
        Task<WasteInstallFee> GetByIdAsync(int id);

        Task<ValidationResult<WasteInstallFeeDTO>> CreateAsync(CreateWasteInstallFeeDTO model);

        Task<ValidationResult<WasteInstallFeeDTO>> UpdateAsync(UpdateWasteInstallFeeDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WasteInstallFeeDTO>> GetListAsync(WasteInstallFeeFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(WasteInstallFeeFilterDTO filter);

        Task<IEnumerable<WasteInstallFeeDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false);
    }
}
