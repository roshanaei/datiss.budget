using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Enum;
using Datiss.Budget.Common;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Security;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts.Identity;

namespace Datiss.Budget.Services.Identity
{
    public class AppClaimTypeService : IAppClaimTypeService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<AppClaimType> _dbSet;

        public AppClaimTypeService(
            IUnitOfWork uow) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<AppClaimType>();
        }


        private IQueryable<AppClaimType> Query()
            => _dbSet.Where(_ => _.Status != EntityStatus.Deleted).AsNoTracking();

        private IQueryable<AppClaimType> QueryEnabled()
            => _dbSet.Where(_ => _.Status == EntityStatus.Enabled).AsNoTracking();

        public async Task<IEnumerable<AppClaimType>> GetEnabledTypesAsync()
            => await QueryEnabled().ToListAsync();

    }
}
