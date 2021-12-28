using System.Threading.Tasks;
using System.Collections.Generic;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Entities;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Contracts
{
    public interface IFinanceYearService
    {
        Task<FinanceYear> GetByIdAsync(int id);
        Task<ValidationResult> CreateAsync(CreateFinanceYearDTO model);
        Task<ValidationResult> UpdateAsync(UpdateFinanceYearDTO model);
        Task<ValidationResult> SoftDeleteAsync(int id);
        Task<ValidationResult> HardDeleteAsync(int id);
        Task<ValidationResult> SetDisbaledAsync(int id);
        Task<IEnumerable<DropDownItem>> GetDropDownDataAsync();
        Task<IEnumerable<DropDownItem>> GetDropDownDataByStatusAsync(EntityStatus entityStatus);
        Task<PagedResult<FinanceYearDTO>> GetListAsync(FinanceYearFilterDTO filter);
        Task<PagedResult<FinanceYearDTO>> GetListDeletedAsync(FinanceYearFilterDTO filter);

    }
}
