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
using Datiss.Budget.Security;

namespace Datiss.Budget.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserContext _userContext;

        private DbSet<Organization> _dbSet;

        public OrganizationService(
            IUnitOfWork uow,
            IUserContext userContext)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
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
                Type = model.Type,
                DisplayOrder = model.DisplayOrder,
                ParentId = model.ParentId,
                Title = model.Title,
                SewageStatus = model.SewageStatus
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
            entity.Type = model.Type;
            entity.DisplayOrder = model.DisplayOrder;
            entity.SewageStatus = model.SewageStatus;
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


        private async Task<IEnumerable<Organization>> getWithChildrenAsync(int organizationId) {
            var result = new List<Organization>();
            var myself = await _dbSet.FirstOrDefaultAsync(_ => _.Id == organizationId);
            result.Add(myself);

            var children = await getByParnetIdAsync(myself.Id);
            result.AddRange(children);

            return await Task.FromResult(result);
        }

        private async Task<IEnumerable<Organization>> getByParnetIdAsync(int? parentId) {
            
            var firstLevel = await Query()
                .Include(x=> x.Childrens)
                .Where(x => x.ParentId == parentId).ToListAsync();

            var result = new List<Organization>();
            result.AddRange(firstLevel);

            foreach (var item in firstLevel) {
                foreach (var child in item.Childrens) {
                    result.Add(child);
                    result.AddRange(await getByParnetIdAsync(child.Id));
                }
            }

            return result;
        }

        public async Task<bool> IsDescendentAsync(int orgId) {
            var query = Query();

            var any = await query.CountAsync(x => x.Id == orgId || x.ParentId == orgId) > 0;

            if(any) {
                return true;
            }
            else {
                var childs = await query.Where(x => x.ParentId == orgId).ToListAsync();
                foreach (var child in childs)
                    return await IsDescendentAsync(child.Id);
            }

            return false;
        }

        public async Task<IEnumerable<DropDownItem>> GetDropDownDataAsync() 
            => _userContext.OrganizationId.HasValue

                ? (await getWithChildrenAsync(_userContext.OrganizationId.Value))
                    .Select(x => new DropDownItem {
                        Id = x.Id,
                        Title = x.Title,
                        Selected = x.Id == _userContext.OrganizationId
                    }).ToList()

                : (await getByParnetIdAsync(_userContext.OrganizationId))
                    .Select(x => new DropDownItem {
                        Id = x.Id,
                        Title = x.Title,
                        Selected = x.Id == _userContext.OrganizationId
                    }).ToList();

    }
}
