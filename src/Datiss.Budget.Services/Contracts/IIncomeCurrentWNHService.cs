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
    public interface IIncomeCurrentWNHService
    {
        Task<IncomeCurrentWNH> GetByIdAsync(int id);

        //Task<ValidationResult> AddAsync(CreateIncomeCurrentWNHDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateIncomeCurrentWNHDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeCurrentWNHDTO>> GetListAsync(IncomeCurrentWNHFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeCurrentWNHFilterDTO filter);

        Task<IEnumerable<IncomeCurrentWNHDTO>> GetExportItemsAsync(IncomeCurrentWNHFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
