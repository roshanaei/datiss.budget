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
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }
        public async Task<ValidationResult> CreateAsync(CreateFinanceYearDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new FinanceYear
            {
                Title = "سال "+model.Year,
                Year = model.Year,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = EntityStatus.Enabled
            };

            try
            {
                await checkLogicAsync(model.Year , model.StartDate , model.EndDate);

                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();
                return ValidationResult.Success();
            }
            catch (InvalidEndYearException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearInvalidEndYear);
            }
            catch (InvalidLengthOfYearException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearLengthYear);
            }
            catch (InvalidCopyLengthException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearCopyLength);
            }
            catch (CopyYearException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearDuplicateYear);
            }
            catch (Exception ex)
            {
                return ValidationResult.Failed(ServiceMessages.SystemError);
            }
        }
        public async Task<ValidationResult> UpdateAsync(UpdateFinanceYearDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            try
            {
                await checkLogicAsync(model.Year, model.StartDate, model.EndDate,model.Id);

                var entity = await _dbSet.FindAsync(model.Id);
                entity.Title = model.Title;
                entity.Year = model.Year;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.Status = model.Enable
                                ? EntityStatus.Enabled
                                : EntityStatus.Disbaled;

                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }
            catch (InvalidEndYearException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearInvalidEndYear);
            }
            catch (InvalidLengthOfYearException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearLengthYear);
            }
            catch (InvalidCopyLengthException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearCopyLength);
            }
            catch (CopyYearException)
            {
                return ValidationResult.Failed(ServiceMessages.FinanceYearDuplicateYear);
            }
            catch (Exception ex)
            {
                return ValidationResult.Failed(ServiceMessages.SystemError);
            }
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
                case "financeyear":
                    return desc
                        ? query.OrderByDescending(x => x.Title)
                        : query.OrderBy(x => x.Title);
                default:
                    return desc
                        ? query.OrderByDescending(x => x.Year)
                        : query.OrderBy(x => x.Year);
            }
        }
        private async Task<bool> isInLenght(DateTime startdate, DateTime enddate, int? id = null)
            => id == null
            ? await Query().AnyAsync(x => (x.StartDate <= startdate && x.EndDate >= startdate) ||
                                          (x.StartDate <= enddate && x.EndDate >= enddate))
            : await Query().Where(x=>x.Id!=id)
                           .AnyAsync(x => (x.StartDate <= startdate && x.EndDate >= startdate) ||
                                    (x.StartDate <= enddate && x.EndDate >= enddate));

        #endregion
        #region Logics

        private async Task checkLogicAsync(
            int year,
            DateTime startDate,
            DateTime endDate,
            int? id = null)
        {
            if (endDate <= startDate)
                throw new InvalidEndYearException();

            if (DateTime.IsLeapYear(startDate.Year))
            {
                if ((endDate - startDate).Days != 365)
                    throw new InvalidLengthOfYearException();
            }
            else
            {
                if ((endDate - startDate).Days != 364)
                    throw new InvalidLengthOfYearException();
            }

            if (await isInLenght(startDate, endDate, id))
                throw new InvalidCopyLengthException();

            if (id.HasValue)
            {
                if (await Query().AnyAsync(x => x.Year == year &&
                                             x.Id != id))
                    throw new CopyYearException();

            }
            else
            {
                if (await Query().AnyAsync(x => x.Year == year))
                    throw new CopyYearException();
            }
        }

        #endregion
    }
}