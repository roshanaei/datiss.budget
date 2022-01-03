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
    public interface IIncomeForcastWsService
    {
        Task<IncomeForcastWs> GetByIdAsync(int id);

        Task<ValidationResult<IncomeForcastWsDTO>> CreateAsync(CreateIncomeForcastWsDTO model);

        Task<ValidationResult<IncomeForcastWsDTO>> UpdateAsync(UpdateIncomeForcastWsDTO model);

        Task HardDeleteAsync(int Id);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeForcastWsDTO>> GetListAsync(IncomeForcastWsFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeForcastWsFilterDTO filter);

        Task<IEnumerable<IncomeForcastWsDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false);

    }
}
