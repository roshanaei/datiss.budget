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
using Datiss.Budget.ViewModels;
using Mapster;
using Datiss.Budget.Resources;
using Datiss.Budget.Common.Exceptions;

namespace Datiss.Budget.Services
{
    public class FinanceYearService : IFinanceYearService
    {
        private readonly IUnitOfWork _uow;

        private DbSet<FinanceYear> _dbSet;

        public FinanceYearService(
            IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<FinanceYear>();
        }

        private IQueryable<FinanceYear> Query()
            => _dbSet.AsNoTracking()
                        .Where(x => x.Status != EntityStatus.Deleted);
        public async Task<FinanceYear> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }
        public async Task CreateAsync(CreateFinanceYearDTO model)
        {
            if (model.EndDate <= model.StartDate || (model.EndDate - model.StartDate).TotalDays != 364)
                throw new FinanceYearInvalidYearException();
            if (await checkLogicAsync(model.Title, model.Year, model.StartDate,model.EndDate))
                throw new FinanceYearInvalidCopyDataException();

            var entity = new FinanceYear
            {
                Title = model.Title,
                Year = model.Year,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = EntityStatus.Enabled
            };

            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();
        }
        public async Task UpdateAsync(UpdateFinanceYearDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            if (model.EndDate <= model.StartDate || (model.EndDate - model.StartDate).TotalDays != 364)
                throw new FinanceYearInvalidYearException();
            if (await checkLogicAsync(model.Title, model.Year, model.StartDate, model.EndDate))
                throw new FinanceYearInvalidCopyDataException();

            var entity = new FinanceYear
            {
                Title = model.Title,
                Year = model.Year,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Enabled
                                ? EntityStatus.Enabled
                                : EntityStatus.Disbaled
            };

            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task<ValidationResult> SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            entity.CheckArgumentIsNull(nameof(entity));
            entity.Status = EntityStatus.Deleted;
            await _uow.SaveChangesAsync();
            return ValidationResult.Success();
        }

        public async Task<IEnumerable<DropDownItem>> GetDropDownDataAsync()
            => await Query().Select(x => new DropDownItem
            {
                Id = x.Id,
                Title = x.Year.ToString()
            }).ToListAsync();

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
        #region Logics

        private async Task<bool> checkLogicAsync(
            string title,
            int year,
            DateTime startDate,
            DateTime endDate,
            int? id = null)
        {
            var result = id == null
                ? await Query().AnyAsync(x => x.Title == title ||
                                                x.Year == x.Year ||
                                                x.StartDate == startDate ||
                                                x.EndDate == endDate)

                : await Query().AnyAsync(x => x.Title == title ||
                                                x.Year == x.Year ||
                                                x.StartDate == startDate ||
                                                x.EndDate == endDate ||
                                                x.Id != id);
            return !result;
        }

        #endregion
    }
}