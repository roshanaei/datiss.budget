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
    public interface IIncomeCurrentWsHService
    {
        Task<IncomeCurrentWsH> GetByIdAsync(int id);

        Task<ValidationResult<IncomeCurrentWsHDTO>> CreateAsync(CreateIncomeCurrentWsHDTO model);

        Task<ValidationResult<IncomeCurrentWsHDTO>> UpdateAsync(UpdateIncomeCurrentWsHDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentWsHDTO>> GetListAsync(IncomeCurrentWsHFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

        Task<IEnumerable<IncomeCurrentWsHDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<Stream> ExportExcelAsync(IncomeCurrentWsHFilterDTO filter);
    }
}
