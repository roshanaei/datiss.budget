using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface IBranchFeeAmountService
    {
        Task<BranchFeeAmount> GetByIdAsync(int id);

        Task<ValidationResult> AddAsync(CreateBranchFeeAmountDTO model);

        Task<ValidationResult> UpdateAsync(UpdateBranchFeeAmountViewModel model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<BranchFeeAmountViewModel>> GetListAsync(BranchFeeAmountFilterDTO filter);

    }
}
