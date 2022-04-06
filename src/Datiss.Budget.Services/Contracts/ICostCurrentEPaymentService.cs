using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostCurrentEPaymentService
    {
        Task<CostCurrentEPayment> GetByIdAsync(int id);

        Task<ValidationResult<CostCurrentEPaymentDTO>> CreateAsync(CreateCostCurrentEPaymentDTO model);

        Task<ValidationResult<CostCurrentEPaymentDTO>> UpdateAsync(UpdateCostCurrentEPaymentDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<CostCurrentEPaymentDTO>> GetListAsync(CostCurrentEPaymentFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostCurrentEPaymentFilterDTO filter);

        Task<IEnumerable<CostCurrentEPaymentDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
