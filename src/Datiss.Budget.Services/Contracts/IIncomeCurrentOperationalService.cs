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
    public interface IIncomeCurrentOperationalService
    {
        Task<IncomeCurrentOperational> GetByIdAsync(int id);

        Task<ValidationResult<IncomeCurrentOperationalDTO>> CreateAsync(CreateIncomeCurrentOperationalDTO model);

        Task<ValidationResult<IncomeCurrentOperationalDTO>> UpdateAsync(UpdateIncomeCurrentOperationalDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentOperationalDTO>> GetListAsync(IncomeCurrentOperationalFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeCurrentOperationalFilterDTO filter);

        Task<IEnumerable<IncomeCurrentOperationalDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
