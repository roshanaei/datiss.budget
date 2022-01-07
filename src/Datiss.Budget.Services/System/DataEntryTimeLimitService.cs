using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Entities;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common;
using Datiss.Budget.Security;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Services
{
    public class DataEntryTimeLimitService : IDataEntryTimeLimitService
    {

        private readonly IDateService _dateService;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DataEntryTimeLimit> _dbSet;
        private readonly IUserContext _userContext;

        public DataEntryTimeLimitService(
            IUnitOfWork uow,
            IDateService dateService,
            IUserContext userContext) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<DataEntryTimeLimit>();
            _dateService = dateService ?? throw new ArgumentNullException(nameof(dateService));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        private IQueryable<DataEntryTimeLimit> query() => _dbSet.AsNoTracking();

        
        public async Task CreateAsync(CreateDataEntryTimeLimitDTO model) 
        {
            model.CheckArgumentIsNull(nameof(model));

            if (model.FinishDate > model.StartDate)
                throw new InvalidDateTimeRangeException();

            var entity = model.Adapt<DataEntryTimeLimit>();
            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task CheckTimeLimitAsync(int? organizationId, int? yearId) 
        {
            var timeLimit = await query()
                        .WithOrganization(organizationId)
                        .WithYear(yearId)
                        .FirstOrDefaultAsync();
            if(timeLimit != null && !timeLimit.CheckTimeLimit(_dateService.Now)) 
                throw new DataEntryTimeLimitException();
            
            var commonTimeLimit = await getCommonTimeLimit();
            if (commonTimeLimit != null && !commonTimeLimit.CheckTimeLimit(_dateService.Now))
                throw new DataEntryTimeLimitException();

            var currentUserTimeLimit = await getCurrentUserTimeLimit(organizationId, yearId);
            if(currentUserTimeLimit != null && !currentUserTimeLimit.CheckTimeLimit(_dateService.Now))
                throw new DataEntryTimeLimitException();
        }

        #region private helper methods

        private async Task<DataEntryTimeLimit> getCommonTimeLimit() 
            => await query()
                .WithOrganization(null)
                .WithYear(null)
                .FirstOrDefaultAsync();

        private async Task<DataEntryTimeLimit> getCurrentUserTimeLimit(int? organizationId, int? yearId)
            => await query()
                .WithOrganization(organizationId)
                .WithYear(yearId)
                .WithUser(_userContext.UserId)
                .FirstOrDefaultAsync();

        #endregion
    }

    public static class DataEntryTimeLimitExts
    {

        public static IQueryable<DataEntryTimeLimit> WithOrganization(this IQueryable<DataEntryTimeLimit> query, int? organizationId)
            => query.Where(_ => _.OrganizationId == organizationId);

        public static IQueryable<DataEntryTimeLimit> WithYear(this IQueryable<DataEntryTimeLimit> query, int? yearId)
            => query.Where(_ => _.YearId == yearId);

        public static IQueryable<DataEntryTimeLimit> WithUser(this IQueryable<DataEntryTimeLimit> query, int? userId)
            => query.Where(_ => _.UserId == userId);

        public static IQueryable<DataEntryTimeLimit> HasTimeLimit(this IQueryable<DataEntryTimeLimit> query, DateTime startDate, DateTime finishDate)
            => query.Where(_ => _.StartDate >= startDate).Where(_ => _.FinishDate <= finishDate);

        public static IQueryable<DataEntryTimeLimit> IsBetween(this IQueryable<DataEntryTimeLimit> query, DateTime when)
            => query.HasTimeLimit(startDate: when, finishDate: when);

        public static bool CheckTimeLimit(this DataEntryTimeLimit limit, DateTime when)
            => (limit.StartDate >= when && limit.FinishDate <= when);
    }
}
