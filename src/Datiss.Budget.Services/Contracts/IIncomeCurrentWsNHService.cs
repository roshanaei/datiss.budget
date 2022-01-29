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
    public interface IIncomeCurrentWsNHService
    {
        Task<IncomeCurrentWsNH> GetByIdAsync(int id);

        Task<ValidationResult<IncomeCurrentWsNHDTO>> CreateAsync(CreateIncomeCurrentWsNHDTO model);

        Task<ValidationResult<IncomeCurrentWsNHDTO>> UpdateAsync(UpdateIncomeCurrentWsNHDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<IncomeCurrentWsNHDTO>> GetListAsync(IncomeCurrentWsNHFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeCurrentWsNHFilterDTO filter);

        Task<IEnumerable<IncomeCurrentWsNHDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

    }
}
