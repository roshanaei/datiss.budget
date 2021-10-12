using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services
{
    public class ReportService : IReportService
    {

        private readonly IUnitOfWork _uow;

        private readonly DbSet<Report> _dbSet;

        public ReportService(IUnitOfWork uow) {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }


        public async Task<Report> GetAsync(int id) {
            var report = await _dbSet
                .Include(_ => _.Params)
                .SingleOrDefaultAsync(x => x.Id == id);

            report.CheckArgumentIsNull(nameof(report));

            return report;
        }


        public async Task<Report> GetAsync(string name) {
            var report = await _dbSet
               .Include(_ => _.Params)
               .SingleOrDefaultAsync(x => x.Name.ToUpper() == name.ToUpper());

            report.CheckArgumentIsNull(nameof(report));

            return report;
        }

    }
}
