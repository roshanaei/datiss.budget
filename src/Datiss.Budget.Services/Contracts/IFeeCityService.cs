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
    public interface IFeeCityService
    {
        Task<FeeCity> GetByIdAsync(int id);

        //Task<ValidationResult> AddAsync(CreateFeeCityDTO model);

        //Task<ValidationResult> UpdateAsync(UpdateFeeCityDTO model);

        Task HardDeleteAsync(int Id);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task<PagedResult<FeeCityDTO>> GetListAsync(FeeCityFilterDTO filter);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(FeeCityFilterDTO filter);

        Task<IEnumerable<FeeCityDTO>> GetExportItemsAsync(FeeCityFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);
    }
}
