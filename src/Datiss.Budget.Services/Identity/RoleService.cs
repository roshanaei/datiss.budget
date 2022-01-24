using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Services.Identity
{

    public class RoleService : IRoleService
    {

        private readonly IApplicationRoleManager _roleManager;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _dbSet;

        public RoleService(
            IUnitOfWork uow,
            IApplicationRoleManager roleManager) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Role>();
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IEnumerable<RoleDTO>> GetAllAsync() {
            var result = await _dbSet.AsNoTracking()
                .Include(_=> _.Claims)
                .Select(_=> _.Adapt<RoleDTO>())
                .ToListAsync();

            return await Task.FromResult(result);
        }

    }
}
