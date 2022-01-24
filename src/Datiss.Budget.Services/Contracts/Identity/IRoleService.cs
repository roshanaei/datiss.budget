using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services.Contracts.Identity
{
    public interface IRoleService {

        Task<IEnumerable<RoleDTO>> GetAllAsync();

    }
}
