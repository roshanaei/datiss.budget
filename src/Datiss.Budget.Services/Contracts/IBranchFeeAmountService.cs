using System.Threading.Tasks;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services.Contracts
{
    public interface IBranchFeeAmountService
    {
        Task<BranchFeeAmount> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateBranchFeeAmountDTO model);

        Task<ValidationResult> UpdateAsync(UpdateBranchFeeAmountDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<BranchFeeAmountDTO>> GetListAsync(BranchFeeAmountFilterDTO filter);

    }
}
