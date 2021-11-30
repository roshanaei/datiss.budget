using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;

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

        public async Task<ValidationResult> CreateAsync(CreateFinanceYearDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            //TODO : check logic
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

        public async Task<ValidationResult> UpdateAsync(UpdateFinanceYearDTO model) {
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

        public Task<IEnumerable<DropDownItem>> GetDropDownStatusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<FinanceYearDTO>> GetListAsync(FinanceYearFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<FinanceYearDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                                    .Select(x => new FinanceYearDTO
                                    {
                                        Id = x.Id,
                                        Title = x.Title,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Year = x.Year,
                                        Status = x.Status
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        #region Private Helper Methods
        private IQueryable<FinanceYear> setOrder(
        IQueryable<FinanceYear> query,
        string orderBy = "id",
        bool desc = true)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "financeyearid":
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }

        #endregion
    }
}