using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;

namespace Datiss.Budget.Services.Contracts
{
    public interface IWasteInstallFeeService
    {
        Task<WasteInstallFee> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateWasteInstallFeeDTO model);

        Task<ValidationResult> UpdateAsync(UpdateWasteInstallFeeDTO model);

        Task HardDeleteAsync(int Id);

        Task<PagedResult<WasteInstallFeeDTO>> GetListAsync(WasteInstallFeeFilterDTO filter);
    }
}
