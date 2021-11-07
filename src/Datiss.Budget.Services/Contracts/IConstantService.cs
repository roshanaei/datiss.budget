using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts
{
    public interface IConstantService
    {

        Task<ValidationResult> CreateAsync(CreateConstantDTO model);

        Task<ValidationResult> UpdateAsync(UpdateConstantDTO model);

        Task<ValidationResult> SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetParentsAsync();

        Task<IEnumerable<DropDownItem>> GetByConstantKeyAsync(string key);
    }
}
