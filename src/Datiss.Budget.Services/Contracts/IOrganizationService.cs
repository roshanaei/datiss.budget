using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Entities;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts
{
    public interface IOrganizationService
    {
        Task<Organization> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateOrganizationDTO model);

        Task<ValidationResult> UpdateAsync(UpdateOrganizationDTO model);

        Task SoftDeleteAsync(int id);

        Task<IEnumerable<DropDownItem>> GetParentsAsync();

        Task<IEnumerable<Organization>> GetWithChildrenAsync(int? organizationId, bool input = false);

        Task<IEnumerable<DropDownItem>> GetDropDownDataAsync(bool input = false, OrganizationType? orgType = null);

        Task<IEnumerable<DropDownItem>> GetDropDownInputDataAsync(int? organizationId, bool? sewageStatus = null);

        Task<IEnumerable<DropDownItem>> GetDropDownTypeOrgDataAsync(OrganizationType type, bool none = false);

        Task<bool> IsDescendentOfAsync(int parentId, int targetOrganizationId);

        Task<IEnumerable<Organization>> GetAllDescendentsAsync(int? parentId);

        Task<PagedResult<OrganizationDTO>> GetListAsync(OrganizationFilterDTO filter);

    }
}
