using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Datiss.Budget.Services.Contracts
{ 
   public interface IIncomeCurrentCofficientService
    {

        Task<IncomeCurrentCofficient> GetByIdAsync(int id);

        Task<ValidationResult<IncomeCurrentCofficientDTO>> CreateAsync(CreateIncomeCurrentCofficientDTO model);

        Task<ValidationResult<IncomeCurrentCofficientDTO>> UpdateAsync(UpdateIncomeCurrentCofficientDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentCofficientDTO>> GetListAsync(IncomeCurrentCofficientFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

        Task<IEnumerable<IncomeCurrentCofficientDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<Stream> ExportExcelAsync(IncomeCurrentCofficientFilterDTO filter);

    }
}
