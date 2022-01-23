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
    public interface IIncomeCurrentWHService
    {
        Task<IncomeCurrentWH> GetByIdAsync(int id);

        Task<ValidationResult<IncomeCurrentWHDTO>> CreateAsync(CreateIncomeCurrentWHDTO model);

        Task<ValidationResult<IncomeCurrentWHDTO>> UpdateAsync(UpdateIncomeCurrentWHDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentWHDTO>> GetListAsync(IncomeCurrentWHFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

        Task<IEnumerable<IncomeCurrentWHDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<Stream> ExportExcelAsync(IncomeCurrentWHFilterDTO filter);
    }
}
