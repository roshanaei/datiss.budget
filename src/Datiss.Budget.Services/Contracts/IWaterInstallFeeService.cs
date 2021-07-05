using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWaterInstallFeeService
    {
        Task<ValidationResult> AddAsync(AddWaterInstallFeeViewModel model);

        Task<ValidationResult> UpdateAsync(UpdateWaterInstallFeeViewModel model);

        Task<ValidationResult> HardDeleteAsync(int Id);

        Task<PagedResult<WaterInstallFeeViewModel>> GetListAsync(WaterInstallFeeFilter filter);
    }
}
