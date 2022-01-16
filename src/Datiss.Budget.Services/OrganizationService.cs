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
using Datiss.Budget.Extensions;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Resources;

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


        private IQueryable<Organization> TrackedQuery()
            => _dbSet.Where(x => x.Status != EntityStatus.Deleted);


        public async Task<Organization> GetByIdAsync(int id)
        {
            var year = await _dbSet.FindAsync(id);
            year.CheckReferenceIsNull(nameof(year));

            return await Task.FromResult(year);
        }

        public async Task<ValidationResult> CreateAsync(CreateOrganizationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var displayOrder = await getDisplayOrder(model.Type, model.ParentId);
            var entity = new Organization
            {
                Type = model.Type,
                DisplayOrder = displayOrder,
                ParentId = model.ParentId,
                Title = model.Title,
                SewageStatus = model.SewageStatus,
                Status = EntityStatus.Enabled
            };

            if (await checkLogicAsync(model.Title, entity.ParentId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();
                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_OrganizationDuplicate, model.Title)
                );

        }

        public async Task<ValidationResult> UpdateAsync(UpdateOrganizationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.Title, model.ParentId, model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.ParentId = model.ParentId;
                entity.Title = model.Title;
                entity.Type = model.Type;
                entity.SewageStatus = model.SewageStatus;
                entity.Status = model.Enabled
                    ? EntityStatus.Enabled
                    : EntityStatus.Disbaled;
                await setDisplayOrderForChild(entity.Type, entity.Id, model.DisplayOrder);
                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }
            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_OrganizationDuplicate, model.Title)
                );
        }

        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            entity.CheckArgumentIsNull(nameof(entity));
            entity.Status = EntityStatus.Deleted;
            await checkChild(id);
            await _uow.SaveChangesAsync();
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

        public async Task<IEnumerable<Organization>> GetAllDescendentsAsync(int? parentId)
        {
            var result = new List<Organization>();

            var query = Query();

            var childs = await query.Where(_ => _.ParentId == parentId).ToListAsync();
            if (childs.Any())
            {
                result.AddRange(childs);
                foreach (var child in childs)
                {
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

        public async Task<IEnumerable<DropDownItem>> GetDropDownDataAsync(bool input = false)
            => _userContext.OrganizationId.HasValue

                ? (await getWithChildrenAsync(_userContext.OrganizationId.Value, input))
                    .Select(x => new DropDownItem
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Selected = x.Id == _userContext.OrganizationId
                    }).ToList()

                : (await getByParnetIdAsync(_userContext.OrganizationId, input))
                    .Select(x => new DropDownItem
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Selected = x.Id == _userContext.OrganizationId
                    }).ToList();
        public async Task<IEnumerable<DropDownItem>> GetDropDownTypeOrgDataAsync(OrganizationType type)
            => await Query().Where(x => x.Type == type)
            .Select(x => new DropDownItem
            {
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();

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

        private IQueryable<Organization> setFilter(IQueryable<Organization> query, OrganizationFilterDTO filter)
        {
            if (filter.OrganizationId.HasValue)
                query = query.Where(x => x.ParentId == filter.OrganizationId.Value);

            if (filter.Type.HasValue)
                query = query.Where(x => x.Type == filter.Type.Value);

            if (filter.SewageStatus.HasValue)
                query = query.Where(x => x.SewageStatus == filter.SewageStatus.Value);

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<Organization> setOrder(
                IQueryable<Organization> query,
                string orderBy = "id",
                bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "organizationid":
                    return desc
                        ? query.OrderByDescending(x => x.Parent.Id)
                        : query.OrderBy(x => x.Parent.Id);

                default:
                    return query.OrderBy(x => x.DisplayOrder)
                                .ThenBy(x => x.Type)
                                .ThenBy(x => x.ParentId);
            }
        }

        private async Task<IEnumerable<Organization>> getByParnetIdAsync(int? parentId, bool input = false, bool queryTracked = false)
        {
            var firstLevel = new List<Organization>();

            if (queryTracked)
            {
                firstLevel = await TrackedQuery()
                   .Include(x => x.Childrens)
                   .Where(x => x.ParentId == parentId)
                   .ToListAsync();
            }
            else
            {
                firstLevel = await Query()
                   .Include(x => x.Childrens)
                   .Where(x => x.ParentId == parentId)
                   .ToListAsync();
            }

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

            foreach (var item in firstLevel)
            {
                foreach (var child in item.Childrens)
                {
                    if (input)
                    {
                        if (child.Type != OrganizationType.Root && child.Type != OrganizationType.County)
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

        private async Task<IEnumerable<Organization>> getWithChildrenAsync(int? organizationId, bool input = false, bool queryTracked = false)
        {
            if (!organizationId.HasValue)
                organizationId = _userContext.OrganizationId;

            var result = new List<Organization>();
            var myself = await _dbSet.FirstOrDefaultAsync(_ => _.Id == organizationId);

            if (myself != null)
            {
                if (input)
                {
                    if (myself.UserCanInput)
                        result.Add(myself);
                }
                else
                {
                    result.Add(myself);
                }
            }

            var children = await getByParnetIdAsync(myself != null ? myself.Id : null, input , queryTracked);

            if (input)
            {
                foreach (var item in children)
                {
                    if (item.UserCanInput)
                        result.Add(item);
                }
            }
            else
            {
                result.AddRange(children);
            }

            return await Task.FromResult(result);
        }


        private async Task<bool> isChildOfAsync(int parentId, int targetOrganizationId)
            => await Query().AnyAsync(_ => _.ParentId == parentId && _.Id == targetOrganizationId);

        private async Task<int> getDisplayOrder(OrganizationType type, int? parentId)
        {
            if (type == OrganizationType.County)
            {
                var record = await Query().OrderBy(x => x.DisplayOrder).LastOrDefaultAsync();
                return record.DisplayOrder + 1;
            }
            else if (type == OrganizationType.Root)
            {
                return 0;
            }
            else
            {
                var reord = await _dbSet.FindAsync(parentId);
                return reord.DisplayOrder;
            }
        }
        private async Task setDisplayOrderForChild(OrganizationType type, int orgId, int displayOrder)
        {
            if (type == OrganizationType.County)
            {
                var child = await getWithChildrenAsync(orgId,queryTracked : true);
                foreach (var item in child)
                {
                    item.DisplayOrder = displayOrder;
                }
                await _uow.SaveChangesAsync();
            }
        }

        private async Task checkChild(int orgId)
        {
            var child = await getByParnetIdAsync(orgId);
            if (child.Any())
                throw new OrganizationHasChildException();
        }

        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            string title,
            int? parentId,
            int? id = null)
        {
            var result = id == null
                ? await Query().AnyAsync(x => x.Title.Trim() == title.Trim() &&
                                              x.ParentId == parentId)

                : await Query().AnyAsync(x => x.Title.Trim() == title.Trim() &&
                                              x.ParentId == parentId &&
                                              x.Id != id);
            return !result;
        }

        #endregion


    }
}

