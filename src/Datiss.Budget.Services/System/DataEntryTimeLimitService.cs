using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Entities;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Common.GuardToolkit;

namespace Datiss.Budget.Services.System
{
    public class DataEntryTimeLimitService
    {

        private readonly IUnitOfWork _uow;
        private readonly DbSet<DataEntryTimeLimit> _dbSet;

        public DataEntryTimeLimitService(IUnitOfWork uow) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<DataEntryTimeLimit>();
        }


    }
}
