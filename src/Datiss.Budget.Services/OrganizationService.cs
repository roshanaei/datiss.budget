using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Services
{
    public class OrganizationService
    {
        private readonly IUnitOfWork _uow;

        private DbSet<Organization> _dbSet;

        public OrganizationService(
            IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Organization>();
        }

        public async Task<ValidationResult> AddAsync(AddOrganizationViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new Organization
            {
                ParentId = model.ParentId,
                Title = model.Title,
                IsVillage = model.IsVillage,
                DisplayOrder = model.DisplayOrder
            };

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

            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

    }
}
