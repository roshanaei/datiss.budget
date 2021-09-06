using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts.Identity
{
    public interface IUserService
    {

        Task<bool> HasAccessToOrganizationAsync(int organizationId);
    }
}
