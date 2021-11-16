using Datiss.Budget.Entities.DWH;
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

        //Task<ValidationResult> AddAsync(CreateIncomeForcastWsDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateIncomeForcasWstDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<IncomeForcastWsDTO>> GetListAsync(IncomeForcastWsFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(IncomeForcastWsFilterDTO filter);

        Task<IEnumerable<IncomeForcastWsDTO>> GetExportItemsAsync(IncomeForcastWsFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
