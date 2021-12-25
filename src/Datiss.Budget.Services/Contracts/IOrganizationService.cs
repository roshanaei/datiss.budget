using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts
{
    public interface IOrganizationService
    {
        Task<ValidationResult> CreateAsync(CreateOrganizationDTO model);

        Task<ValidationResult> UpdateAsync(UpdateOrganizationDTO model);

        Task<ValidationResult> SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetParentsAsync();

        Task<IEnumerable<Organization>> GetWithChildrenAsync(int organizationId);

        Task<IEnumerable<DropDownItem>> GetDropDownDataAsync(bool input = false);

        Task<bool> IsDescendentOfAsync(int parentId, int targetOrganizationId);

        Task<IEnumerable<Organization>> GetAllDescendentsAsync(int? parentId);

        Task<PagedResult<OrganizationDTO>> GetListAsync(OrganizationFilterDTO filter);

    }
}
