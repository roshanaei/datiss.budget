using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentBankFeeService
    {
        Task<CostCurrentBankFee> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentBankFeeDTO>> CreateAsync(CreateCostCurrentBankFeeDTO model);

        Task<ValidationResult<CostCurrentBankFeeDTO>> UpdateAsync(UpdateCostCurrentBankFeeDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentBankFeeDTO>> GetListAsync(CostCurrentBankFeeFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostCurrentBankFeeDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
