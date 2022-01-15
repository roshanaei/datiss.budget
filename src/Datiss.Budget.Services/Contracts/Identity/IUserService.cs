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

        Task<UserResultDTO> GetByIdAsync(int id);

        Task<ValidationResult<UserResultDTO>> CreateAsync(CreateUserDTO model);

        Task<ValidationResult<UserResultDTO>> UpdateAsync(UpdateUserDTO model);

        Task<PagedResult<UserResultDTO>> GetListAsync(UserFilterDTO filter);

        Task<bool> HasAccessToOrganizationAsync(int organizationId);
    }
}
