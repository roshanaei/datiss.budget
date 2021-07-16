using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWasteInstallFeeService
    {
        Task<WasteInstallFee> GetByIdAsync(int id);

        Task<ValidationResult> AddAsync(CreateWasteInstallFeeDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWasteInstallFeeViewModel model);

        Task<ValidationResult> HardDeleteAsync(int Id);

        Task<PagedResult<WasteInstallFeeViewModel>> GetListAsync(WasteInstallFeeFilter filter);
    }
}
