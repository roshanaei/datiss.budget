using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.Services.Contracts.Identity
{

    public interface IAppClaimTypeService 
    {

        Task<IEnumerable<AppClaimType>> GetEnabledTypesAsync();
    }
}
