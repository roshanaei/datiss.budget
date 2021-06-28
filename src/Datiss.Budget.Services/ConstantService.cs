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
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts;

namespace Datiss.Budget.Services
{
    public class ConstantService: IConstantService
    {
        private readonly IUnitOfWork _uow;

        private DbSet<Constant> _dbSet;

        public ConstantService(
            IUnitOfWork uow) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Constant>();
        }

        public async Task<ValidationResult> AddAsync(AddConstantViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (await ExistByKeyAsync(model.ConstantKey))
                return new ValidationResult {
                    IsValid = false,
                    Message = "نام کلید تکراری است."
                };

            var entity = new Constant {
                ConstantKey = model.ConstantKey,
                DisplayOrder = model.DisplayOrder,
                ParentId = model.ParentId,
                Title = model.Title
            };

            entity.Status = model.Enabled 
                ? EntityStatus.Enabled 
                : EntityStatus.Disbaled;

            await _dbSet.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> UpdateAsync(UpdateConstantViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (await ExistByKeyAsync(model.ConstantKey, model.Id))
                return new ValidationResult {
                    IsValid = false,
                    Message = "نام کلید تکراری است."
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

            return ValidationResult.Success();
        }

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
