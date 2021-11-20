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

        //Task<ValidationResult> AddAsync(CreateIncomeCurrentWHDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateIncomeCurrentWHDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentWHDTO>> GetListAsync(IncomeCurrentWHFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeCurrentWHFilterDTO filter);

        Task<IEnumerable<IncomeCurrentWHDTO>> GetExportItemsAsync(IncomeCurrentWHFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
