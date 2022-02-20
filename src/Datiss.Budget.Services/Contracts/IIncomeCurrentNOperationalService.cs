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
    public interface IIncomeCurrentNOperationalService
    {
        Task<IncomeCurrentNOperational> GetByIdAsync(int id);

        Task<ValidationResult<IncomeCurrentNOperationalDTO>> CreateAsync(CreateIncomeCurrentNOperationalDTO model);

        Task<ValidationResult<IncomeCurrentNOperationalDTO>> UpdateAsync(UpdateIncomeCurrentNOperationalDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentNOperationalDTO>> GetListAsync(IncomeCurrentNOperationalFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeCurrentNOperationalFilterDTO filter);

        Task<IEnumerable<IncomeCurrentNOperationalDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);

    }
}
