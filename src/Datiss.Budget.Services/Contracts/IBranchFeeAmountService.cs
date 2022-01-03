using System.Threading.Tasks;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface IBranchFeeAmountService
    {
        Task<BranchFeeAmount> GetByIdAsync(int id);

        Task<ValidationResult<BranchFeeAmountDTO>> CreateAsync(CreateBranchFeeAmountDTO model);

        Task<ValidationResult<BranchFeeAmountDTO>> UpdateAsync(UpdateBranchFeeAmountDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<BranchFeeAmountDTO>> GetListAsync(BranchFeeAmountFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false);

        Task<Stream> ExportExcelAsync(BranchFeeAmountFilterDTO filter);

        Task<IEnumerable<BranchFeeAmountDTO>> GetExportItemsAsync(BranchFeeAmountFilterDTO filter);

    }
}
