using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Services.Contracts
{
    public interface  IWaterSalesSplitService
    {

        Task<WaterSalesSplit> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateWaterSalesSplitDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWaterSalesSplitDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WaterSalesSplitDTO>> GetListAsync(WaterSalesSplitFilterDTO filter);

        Task HardDeleteAsync(int yearId, int organizationId);

        //Task<int> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(WaterSalesSplitFilterDTO filter);

        Task<IEnumerable<WaterSalesSplitDTO>> GetExportItemsAsync(WaterSalesSplitFilterDTO filter);

        Task ImportExcelAsync(IFormFile fileInfo);

    }
    
}
