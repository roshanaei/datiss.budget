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
using System.Globalization;

namespace Datiss.Budget.Services
{
    public class FinanceYearService: IFinanceYearService
    {
        private readonly IUnitOfWork _uow;

        private DbSet<FinanceYear> _dbSet;

        public FinanceYearService(
            IUnitOfWork uow) {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<FinanceYear>();
        }

        private IQueryable<FinanceYear> Query()
            => _dbSet.AsNoTracking()
                        .Where(x=> x.Status != EntityStatus.Deleted);

        public async Task<ValidationResult> AddAsync(AddFinanceYearViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new FinanceYear {
                Year = model.Year,
                Title = model.Title,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> UpdateAsync(UpdateFinanceYearViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var entity = await _dbSet.FindAsync(model.Id);
            entity.Year = model.Year;
            entity.Title = model.Title;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;

            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> SoftDeleteAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            entity.CheckArgumentIsNull(nameof(entity));

            entity.Status = EntityStatus.Deleted;

            await _uow.SaveChangesAsync();

            return ValidationResult.Success();

        }

        public async Task<IEnumerable<DropDownItem>> GetDropDownDataAsync() 
            => await Query().Select(x => new DropDownItem {
                Id = x.Id,
                Title = x.Year.ToString()
            }).ToListAsync();
        
    }
}