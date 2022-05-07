using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface ICostForcastFinanceService
    {

        Task<CostForcastFinance> GetByIdAsync(int id);

        Task<ValidationResult<CostForcastFinanceDTO>> CreateAsync(CreateCostForcastFinanceDTO model);

        Task<ValidationResult<CostForcastFinanceDTO>> UpdateAsync(UpdateCostForcastFinanceDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<CostForcastFinanceDTO>> GetListAsync(CostForcastFinanceFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(CostForcastFinanceFilterDTO filter);

        Task<IEnumerable<CostForcastFinanceDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

    }

}
