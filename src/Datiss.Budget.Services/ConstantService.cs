using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.PersianToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using DNTPersianUtils.Core;
using Mapster;

namespace Datiss.Budget.Services
{
    public class ConstantService: IConstantService
    {
        private readonly IUnitOfWork _uow;

        private readonly DbSet<Constant> _dbSet;

        public ConstantService(
            IUnitOfWork uow) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Constant>();
        }

        private IQueryable<Constant> Query() 
           => _dbSet.AsNoTracking()
                    .Where(_ => _.Status != EntityStatus.Deleted);

        public async Task<Constant> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult> CreateAsync(CreateConstantDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            if (await ExistByKeyAsync(model.ConstantKey))
                return new ValidationResult {
                    IsValid = false,
                    Message = "نام کلید تکراری است." //TODO : move this to resource
                };

            var entity = new Constant {
                ConstantKey = model.ConstantKey.Trim(),
                DisplayOrder = model.DisplayOrder,
                ParentId = model.ParentId,
                Title = model.Title.Trim().ApplyCorrectYeKe()
            };

            entity.Status = model.Enabled 
                ? EntityStatus.Enabled 
                : EntityStatus.Disbaled;

            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> UpdateAsync(UpdateConstantDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            if (await ExistByKeyAsync(model.ConstantKey, model.Id))
                return new ValidationResult {
                    IsValid = false,
                    Message = "نام کلید تکراری است." //TODO : move this to resource
                };

            var entity = await _dbSet.FindAsync(model.Id);
            entity.ParentId = model.ParentId;
            entity.Title = model.Title;
            entity.ConstantKey = model.ConstantKey;
            entity.DisplayOrder = model.DisplayOrder;
            entity.Status = model.Enabled
                ? EntityStatus.Enabled
                : EntityStatus.Disbaled;

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

        public async Task<IEnumerable<DropDownItem>> GetParentsAsync() 
            => await _dbSet
                .Where(x => x.ParentId == null)
                .Select(x => new DropDownItem {
                    Id = x.Id,
                    Title = x.Title
                }).ToListAsync();

        public async Task<IEnumerable<DropDownItem>> GetByConstantKeyAsync(string key)
            => await _dbSet
                        .Include(x=> x.Parent)
                        .Where(x => x.Parent.ConstantKey.ToUpper() == key.ToUpper())
                        .OrderBy(x=> x.DisplayOrder)
                        .Select(x => new DropDownItem {
                            Id = x.Id,
                            Title = x.Title
                        }).ToListAsync();
        public async Task<IEnumerable<DropDownItem>> GetByKeyAsync(string key,string parentkey,bool none = false)
            => none 
            ? await _dbSet
                        .Include(x => x.Parent)
                        .Where(x => x.ConstantKey.ToUpper() != key.ToUpper() &&
                                    x.Parent.ConstantKey.ToUpper() == parentkey.ToUpper())
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => new DropDownItem
                        {
                            Id = x.Id,
                            Title = x.Title
                        }).ToListAsync()
            : await _dbSet
                        .Include(x=>x.Parent)
                        .Where(x => x.ConstantKey.ToUpper() == key.ToUpper() &&
                                    x.Parent.ConstantKey.ToUpper() == parentkey.ToUpper())
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => new DropDownItem
                        {
                            Id = x.Id,
                            Title = x.Title
                        }).ToListAsync();

        public async Task<IEnumerable<DropDownItem>> GetCofficientByKeysAsync(string key, string parentkey)
            => await _dbSet
                        .Include(x => x.Parent)
                        .Where(x => x.Parent.ConstantKey.ToUpper() == parentkey.ToUpper() &&
                                    x.ConstantKey.ToUpper().Contains(key.ToUpper()))
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => new DropDownItem
                        {
                            Id = x.Id,
                            Title = x.Title
                        }).ToListAsync();


        public async Task<IEnumerable<ConstantDTO>> GetDataByKeyAsync(string key)
            => await _dbSet
                        .Include(x => x.Parent)
                        .Where(x => x.Parent.ConstantKey.ToUpper() == key.ToUpper())
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => x.Adapt<ConstantDTO>())
                        .ToListAsync();
        
        public async Task<PagedResult<ConstantDTO>> GetListAsync(ConstantFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<ConstantDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            query = setFilter(query, filter);

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy , filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                                    .Select(x => new ConstantDTO
                                    {
                                        Id = x.Id,
                                        ParentId = x.ParentId,
                                        Title = x.Title,
                                        ConstantKey = x.ConstantKey,
                                        Status = x.Status,
                                        DisplayOrder = x.DisplayOrder
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        #region Private Methods
        private IQueryable<Constant> setFilter(IQueryable<Constant> query, ConstantFilterDTO filter)
        {
            if (filter.ParentId.HasValue)
                query = query.Where(x => x.ParentId == filter.ParentId.Value);
            return query;
        }
        private IQueryable<Constant> setOrder(
             IQueryable<Constant> query,
             string orderBy = "id",
             bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "displayorder":
                    return desc
                                ? query.OrderByDescending(x => x.DisplayOrder)
                                : query.OrderBy(x => x.DisplayOrder);

                default:
                    return desc
                                ? query.OrderByDescending(x => x.Id)
                                : query.OrderBy(x => x.Id);
            }
                    
        }
        private async Task<bool> ExistByKeyAsync(string contantKey, int? id = null)
            => id == null
                ? await _dbSet.AnyAsync
                    (_ => _.ConstantKey.ToUpper() == contantKey.ToUpper()) != null
                : await _dbSet.AnyAsync
                    (_ => _.Id != id.Value && _.ConstantKey.ToUpper() == contantKey.ToUpper()) != null;

        #endregion
    }
}
