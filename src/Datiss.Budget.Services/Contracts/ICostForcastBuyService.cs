using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastBuyService
    {
        Task<CostForcastBuy> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastBuyDTO>> CreateAsync(CreateCostForcastBuyDTO model);

        Task<ValidationResult<CostForcastBuyDTO>> UpdateAsync(UpdateCostForcastBuyDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastBuyDTO>> GetListAsync(CostForcastBuyFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<IEnumerable<CostForcastBuyDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}