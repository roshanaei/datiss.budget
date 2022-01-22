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
    public interface IWWsFeeService
    { 
        Task<WWsFee> GetByIdAsync(int id);

        Task<ValidationResult<WWsFeeDTO>> CreateAsync(CreateWWsFeeDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateWWsFeeDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<WWsFeeDTO>> GetListAsync(WWsFeeFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(WWsFeeFilterDTO filter);

        Task<IEnumerable<WWsFeeDTO>> GetExportItemsAsync(WWsFeeFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
