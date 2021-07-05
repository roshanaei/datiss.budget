using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Services.Contracts
{
    public interface IFinanceYearService
    {
        Task<ValidationResult> AddAsync(AddFinanceYearViewModel model);
        
        Task<ValidationResult> UpdateAsync(UpdateFinanceYearViewModel model);

        Task<ValidationResult> SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetDropDownDataAsync();
    }
}
