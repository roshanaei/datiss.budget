using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Security;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Datiss.Budget.Services.Identity
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _uow;
        private DbSet<User> _dbSet;
        private readonly IUserContext _userContext;
        private readonly IOrganizationService _organizationService;

        public UserService(
            IUnitOfWork uow, 
            UserContext userContext,
            IOrganizationService organizationService) {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _dbSet = _uow.Set<User>();
        }

        public async Task<bool> HasAccessToOrganizationAsync(int organizationId) {
            if (_userContext.OrganizationId == organizationId)
                return true;

            return await _organizationService.IsDescendentAsync(organizationId);
        }
    }
}
