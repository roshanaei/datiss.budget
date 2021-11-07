using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface  IWaterSalesSplitService
    {

        Task<WaterSalesSplit> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateWaterSalesSplitDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWaterSalesSplitDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WaterSalesSplitDTO>> GetListAsync(WaterSalesSplitFilterDTO filter);

    }
    
}
