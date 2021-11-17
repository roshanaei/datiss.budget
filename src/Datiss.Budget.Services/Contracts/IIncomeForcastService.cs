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
    public interface IIncomeForcastService
    {
        Task<IncomeForcast> GetByIdAsync(int id);

        //Task<ValidationResult> AddAsync(CreateIncomeForcastDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateIncomeForcastDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeForcastDTO>> GetListAsync(IncomeForcastFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeForcastFilterDTO filter);

        Task<IEnumerable<IncomeForcastDTO>> GetExportItemsAsync(IncomeForcastFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
