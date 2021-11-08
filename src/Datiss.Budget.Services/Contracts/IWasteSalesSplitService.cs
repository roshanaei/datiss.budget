using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface  IWasteSalesSplitService
    {

        Task<WasteSalesSplit> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateWasteSalesSplitDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWasteSalesSplitDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WasteSalesSplitDTO>> GetListAsync(WasteSalesSplitFilterDTO filter);

    }
    
}
