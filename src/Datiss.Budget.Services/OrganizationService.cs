using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly DbSet<Organization> _dbSet;

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

        public async Task<ValidationResult> CreateAsync(CreateOrganizationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            //TODO : check logic
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

        public async Task<ValidationResult> UpdateAsync(UpdateOrganizationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            //TODO : check logic
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

        public async Task<IEnumerable<Organization>> GetWithChildrenAsync(int? organizationId, bool input = false)
            => await getWithChildrenAsync(organizationId, input);

        public async Task<IEnumerable<Organization>> GetAllDescendentsAsync(int? parentId) {
            var result = new List<Organization>();

            var query = Query();

            var childs = await query.Where(_ => _.ParentId == parentId).ToListAsync();
            if (childs.Any()) {
                result.AddRange(childs);
                foreach(var child in childs) {
                    result.AddRange(await GetAllDescendentsAsync(child.Id));
                }
            }
            
            return await Task.FromResult(result);
        }

        public async Task<bool> IsDescendentOfAsync(int parentId, int targetOrganizationId) 
        {
            var query = Query();

            var targetOrg = await _dbSet.FindAsync(targetOrganizationId);
            targetOrg.CheckReferenceIsNull(nameof(targetOrg));

            if (targetOrg.ParentId == null)
                return false;
            
            if (targetOrg.ParentId == parentId)
                return true;

            return await IsDescendentOfAsync(parentId, targetOrg.ParentId.Value);
        }


        private async Task<bool> isChildOfAsync(int parentId, int targetOrganizationId)
            => await Query().AnyAsync(_ => _.ParentId == parentId && _.Id == targetOrganizationId);
        

        public async Task<IEnumerable<DropDownItem>> GetDropDownDataAsync(bool input=false)
            =>_userContext.OrganizationId.HasValue

                ? (await getWithChildrenAsync(_userContext.OrganizationId.Value,input))
                    .Select(x => new DropDownItem
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Selected = x.Id == _userContext.OrganizationId
                    }).ToList()

                : (await getByParnetIdAsync(_userContext.OrganizationId,input))
                    .Select(x => new DropDownItem
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Selected = x.Id == _userContext.OrganizationId
                    }).ToList();

        public async Task<PagedResult<OrganizationDTO>> GetListAsync(OrganizationFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<OrganizationDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            query = setFilter(query, filter);

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                                    .Include(x => x.Parent)
                                    .Include(x => x.Childrens)
                                    .Select(x => new OrganizationDTO 
                                    {
                                        Id = x.Id,
                                        ParentId = x.ParentId,
                                        Title = x.Title,
                                        DisplayOrder = x.DisplayOrder,
                                        Type = x.Type,
                                        SewageStatus = x.SewageStatus,
                                        Status = x.Status
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        #region Private Helper Methods

        private IQueryable<Organization> setFilter(IQueryable<Organization> query, OrganizationFilterDTO filter) {
            if (filter.ParentId.HasValue)
                query = query.Where(x => x.ParentId == filter.ParentId.Value);

            if (filter.Type.HasValue)
                query = query.Where(x => x.Type == filter.Type.Value);

            if (filter.SewageStatus.HasValue)
                query = query.Where(x => x.SewageStatus == filter.SewageStatus.Value);

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status.Value);

            return query;
        }

        private IQueryable<Organization> setOrder(
                IQueryable<Organization> query,
                string orderBy = "id",
                bool desc = false) {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy) {
                case "parent":
                    return desc
                        ? query.OrderByDescending(x => x.Parent.Id)
                        : query.OrderBy(x => x.Parent.Id);

                case "title":
                    return desc
                        ? query.OrderByDescending(x => x.Title)
                        : query.OrderBy(x => x.Title);

                case "sewagestatus":
                    return desc
                        ? query.OrderByDescending(x => x.SewageStatus)
                        : query.OrderBy(x => x.SewageStatus);

                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }

        private async Task<IEnumerable<Organization>> getByParnetIdAsync(int? parentId,bool input=false) {

            var firstLevel = await Query()
                .Include(x => x.Childrens)
                .Where(x => x.ParentId == parentId).ToListAsync();

            var result = new List<Organization>();
            if (input)
            {
                foreach (var item in firstLevel)
                {
                    if (item.Type != OrganizationType.Root && item.Type != OrganizationType.County)
                        result.Add(item);
                }
            }
            else
            {
                result.AddRange(firstLevel);
            }

            foreach (var item in firstLevel) {
                foreach (var child in item.Childrens) {
                    if (input)
                    {                       
                        if (item.Type != OrganizationType.Root && item.Type != OrganizationType.County)
                            result.Add(child);
                    }
                    else
                    {
                        result.Add(child);
                    }
                    result.AddRange(await getByParnetIdAsync(child.Id, input));
                }
            }

            return result;
        }

        private async Task<IEnumerable<Organization>> getWithChildrenAsync(int? organizationId, bool input=false)
        {
            if (!organizationId.HasValue)
                organizationId = _userContext.OrganizationId;

            var result = new List<Organization>();
            var myself = await _dbSet.FirstOrDefaultAsync(_ => _.Id == organizationId);

            if(myself != null && !input)
                result.Add(myself);

            var children = await getByParnetIdAsync(myself != null ? myself.Id : null, input);

            if (input)
            {
                foreach (var item in children)
                {
                    if (item.Type != OrganizationType.Root && item.Type != OrganizationType.County)
                        result.Add(item);
                }
            }   
            else
            {
                result.AddRange(children);
            }

            return await Task.FromResult(result);
        }


        #endregion


    }
}

