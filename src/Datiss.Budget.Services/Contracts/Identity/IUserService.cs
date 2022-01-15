using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts.Identity
{
    public interface IUserService
    {

        Task<ValidationResult<UserResultDto>> CreateAsync(CreateUserDto model);


        Task<ValidationResult<UserResultDto>> UpdateAsync(UpdateUserDto model);
        

        Task<bool> HasAccessToOrganizationAsync(int organizationId);
    }
}
