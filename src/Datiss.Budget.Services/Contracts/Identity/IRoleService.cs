using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services.Contracts.Identity
{
    public interface IRoleService {

        Task<IEnumerable<RoleDTO>> GetAllAsync();

        Task<RoleDTO> GetByIdAsync(int id);

        Task<ValidationResult> CreateAsync(CreateRoleDTO model);

        Task<ValidationResult> UpdateAsync(UpdateRoleDTO model);

    }
}
