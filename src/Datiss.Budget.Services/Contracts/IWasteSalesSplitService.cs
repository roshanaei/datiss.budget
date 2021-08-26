using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface  IWasteSalesSplitService
    {

        Task<WasteSalesSplit> GetByIdAsync(int id);

        Task<ValidationResult> AddAsync(CreateWasteSalesSplitDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWasteSalesSplitViewModel model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WasteSalesSplitViewModel>> GetListAsync(WasteSalesSplitFilter filter);

    }
    
}
