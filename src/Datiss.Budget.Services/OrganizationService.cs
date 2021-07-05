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
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _uow;

        private DbSet<Organization> _dbSet;

        public OrganizationService(
            IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Organization>();
        }

        private IQueryable<Organization> Query()
            => _dbSet.AsNoTracking()
                        .Where(x => x.Status != EntityStatus.Deleted);

        public async Task<ValidationResult> AddAsync(AddOrganizationViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new Organization
            {
                IsVillage = model.IsVillage,
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

        public async Task<ValidationResult> UpdateAsync(UpdateOrganizationViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = await _dbSet.FindAsync(model.Id);
            entity.ParentId = model.ParentId;
            entity.Title = model.Title;
            entity.IsVillage = model.IsVillage;
            entity.DisplayOrder = model.DisplayOrder;
            entity.Status = model.Enabled
                ? EntityStatus.Enabled
                : EntityStatus.Disbaled;

            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            entity.CheckArgumentIsNull(nameof(entity));

            entity.Status = EntityStatus.Deleted;

            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        public async Task<IEnumerable<DropDownItem>> GetParentsAsync()
            => await _dbSet
                .Where(x => x.ParentId == null)
                .Select(x => new DropDownItem
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToListAsync();


        private async Task<IEnumerable<Organization>> getByParnetIdAsync(int? parentId) {
            var firstLevel = await Query()
                .Where(x => x.ParentId == parentId).ToListAsync();

            foreach (var item in firstLevel) {
                foreach (var child in item.Childrens) {
                    firstLevel.AddRange(await getByParnetIdAsync(child.Id));
                }
            }

            return firstLevel;
        }

        public async Task<IEnumerable<DropDownItem>> GetDropDownDataAsync(int? parentId) 
            => (await getByParnetIdAsync(parentId))
                .Select(x => new DropDownItem {
                    Id = x.Id,
                    Title = x.Title,
                    Selected = x.Id == parentId
                }).ToList();

    }
}
