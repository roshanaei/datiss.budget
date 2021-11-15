using System.Threading.Tasks;
using System.Collections.Generic;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services.Contracts
{
    public interface IFinanceYearService
    {
        Task<ValidationResult> CreateAsync(CreateFinanceYearDTO model);
        
        Task<ValidationResult> UpdateAsync(UpdateFinanceYearDTO model);

        Task<ValidationResult> SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetDropDownDataAsync();
        Task<PagedResult<FinanceYearDTO>> GetListAsync(FinanceYearFilterDTO filter);

    }
}
