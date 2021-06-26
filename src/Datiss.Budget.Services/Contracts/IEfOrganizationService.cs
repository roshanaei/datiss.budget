using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IEfOrganizationService
    {
        void AddNewOrganization(Organization organization);
        IList<Organization> GetAllOrganizations();

        void EditOrganization(Organization model);
        Task<IList<Organization>> GetAllOrganizationsAsync();



        //Task<ServiceActionResult<Organization>> AddApiAsync(Organization model);
        //Task<List<OrganizationApiModel>> GetAllApiAsync();
    }

}
