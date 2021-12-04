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
        public async Task<FinanceYear> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }
        public async Task CreateAsync(CreateFinanceYearDTO model)
        { 
            var entity = new FinanceYear
            {
                Title = model.Title,
                Year = model.Year,
                StartDate= model.StartDate,
                EndDate=model.EndDate,
                Status = EntityStatus.Enabled
            };

            if (await checkLogicAsync(model.Title))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();
                var result = entity.Adapt<FinanceYearDTO>();
                result.Title = entity.Title;
                result.Year = entity.Year;
                result.Status = entity.Status;
                result.StartDate = entity.StartDate;
                result.EndDate = entity.EndDate;

                //return ValidationResult<FinanceYearDTO>.Success(result);
            }
        }
        public async Task<ValidationResult<FinanceYearDTO>> UpdateAsync(UpdateFinanceYearDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.Title, model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.Title = model.Title;
                entity.Year = model.Year;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.Status = model.Enabled
                                ? EntityStatus.Enabled
                                : EntityStatus.Disbaled;

                await _uow.SaveChangesAsync();

                var result = new FinanceYearDTO
                {
                    Title = model.Title,
                    Year = model.Year,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = model.Enabled
                                ? EntityStatus.Enabled
                                : EntityStatus.Disbaled
                };

                return ValidationResult<FinanceYearDTO>.Success(result);
            }

            return ValidationResult<FinanceYearDTO>.Failed(
                string.Format(ServiceMessages.DuplicateNames,
                                model.Title)
                );
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

        public async Task<IEnumerable<DropDownItem>> GetDropDownStatusAsync()
            => (from EntityStatus entitystatus in EntityStatus.GetValues(typeof(EntityStatus))
                select new DropDownItem
                {
                    Id = (int)entitystatus,
                    Title = entitystatus.ToDisplay()
                }).Where(x => x.Id != -1)
                .OrderByDescending(x => x.Id);

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
            int? id = null)
        {
            var result = id == null
                ? await Query().AnyAsync(x => x.Title==title)

                : await Query().AnyAsync(x => x.Title==title &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}