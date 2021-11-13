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

        #region Private Methods

        private async Task<bool> ExistByKeyAsync(string contantKey, int? id = null)
            => id == null
                ? await _dbSet.FirstOrDefaultAsync
                    (_ => _.ConstantKey.ToUpper() == contantKey.ToUpper()) != null
                : await _dbSet.FirstOrDefaultAsync
                    (_ => _.Id != id.Value && _.ConstantKey.ToUpper() == contantKey.ToUpper()) != null;

        #endregion
    }
}
