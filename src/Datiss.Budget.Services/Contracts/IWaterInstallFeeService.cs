using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWaterInstallFeeService
    {
        Task<WaterInstallFee> GetByIdAsync(int id);

        Task<ValidationResult<WaterInstallFeeDTO>> CreateAsync(CreateWaterInstallFeeDTO model);

        Task<ValidationResult<WaterInstallFeeDTO>> UpdateAsync(UpdateWaterInstallFeeDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<WaterInstallFeeDTO>> GetListAsync(WaterInstallFeeFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(WaterInstallFeeFilterDTO filter);

        Task<IEnumerable<WaterInstallFeeDTO>> GetExportItemsAsync(WaterInstallFeeFilterDTO filter);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false);
    }
}
