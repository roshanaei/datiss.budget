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
    public interface IIncomeForcastOtherService
    {
        Task<IncomeForcastOther> GetByIdAsync(int id);

        Task<ValidationResult<IncomeForcastOtherDTO>> CreateAsync(CreateIncomeForcastOtherDTO model);

        Task<ValidationResult<IncomeForcastOtherDTO>> UpdateAsync(UpdateIncomeForcastOtherDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeForcastOtherDTO>> GetListAsync(IncomeForcastOtherFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeForcastOtherFilterDTO filter);

        Task<IEnumerable<IncomeForcastOtherDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false);
    }
}
