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

        Task<ValidationResult<WaterSalesSplitDTO>> CreateAsync(CreateWaterSalesSplitDTO model);

        Task<ValidationResult<WaterSalesSplitDTO>> UpdateAsync(UpdateWaterSalesSplitDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WaterSalesSplitDTO>> GetListAsync(WaterSalesSplitFilterDTO filter);

        Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId);

        Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId);

        Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId);

        Task<Stream> ExportExcelAsync(WaterSalesSplitFilterDTO filter);

        Task<IEnumerable<WaterSalesSplitDTO>> GetExportItemsAsync(int yearId, int organizationId);

        Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false);

    }
    
}
