using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Services.Contracts
{
    public interface IOrganizationService
    {
        Task<ValidationResult> AddAsync(AddOrganizationViewModel model);

        Task<ValidationResult> UpdateAsync(UpdateOrganizationViewModel model);

        Task<ValidationResult> SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetParentsAsync();
    }
}
