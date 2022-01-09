using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IBranchingRateIncreaseService
    {
        Task<BranchingRateIncrease> GetByIdAsync(int id);

        Task<ValidationResult<BranchingRateIncreaseDTO>> CreateAsync(CreateBranchingRateIncreaseDTO model);

        Task<ValidationResult<BranchingRateIncreaseDTO>> UpdateAsync(UpdateBranchingRateIncreaseDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<BranchingRateIncreaseDTO>> GetListAsync(BranchingRateIncreaseFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(BranchingRateIncreaseFilterDTO filter);

        Task<IEnumerable<BranchingRateIncreaseDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
