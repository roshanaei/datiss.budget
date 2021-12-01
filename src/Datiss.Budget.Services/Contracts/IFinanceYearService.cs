using System.Threading.Tasks;
using System.Collections.Generic;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services.Contracts
{
    public interface IFinanceYearService
    {
        Task CreateAsync(CreateFinanceYearDTO model);
        Task<ValidationResult<FinanceYearDTO>> UpdateAsync(UpdateFinanceYearDTO model);
        Task<ValidationResult> SoftDeleteAsync(int id);
        Task<IEnumerable<DropDownItem>> GetDropDownDataAsync();
        Task<IEnumerable<DropDownItem>> GetDropDownStatusAsync();
        Task<PagedResult<FinanceYearDTO>> GetListAsync(FinanceYearFilterDTO filter);
    }
}
