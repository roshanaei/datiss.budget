using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWaterInstallFeeService
    {
        Task<WaterInstallFee> GetByIdAsync(int id);

        Task<ValidationResult> AddAsync(CreateWaterInstallFeeDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWaterInstallFeeViewModel model);

        Task<ValidationResult> HardDeleteAsync(int Id);

        Task<PagedResult<WaterInstallFeeViewModel>> GetListAsync(WaterInstallFeeFilter filter);
    }
}
