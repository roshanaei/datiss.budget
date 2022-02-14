using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class CostCurrentInstallationService :ICostCurrentInstallationService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostCurrentInstalation> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostCurrentInstallationService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentInstalation>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentInstalation> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostCurrentInstalation> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentInstalationDTO>> CreateAsync(CreateCostCurrentInstalationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new CostCurrentInstalation
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                CCInstalationTypeId = model.CCInstalationTypeId,
                ActivityType = model.ActivityType,
                NumberUser = model.NumberUser,
                Cost = model.Cost,
                Income = model.Income
            };

            model.CCInstalationTypeTitle = (await _constSet.FindAsync(model.CCInstalationTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.CCInstalationTypeId, model.ActivityType))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<CostCurrentInstalationDTO>();
                    result.CCInstalationTypeTitle = (await _constSet.FindAsync(model.CCInstalationTypeId)).Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.ActivityType = model.ActivityType;
                    result.NumberUser = model.NumberUser;
                    result.Cost = model.Cost;
                    result.Income = model.Income;

                    return ValidationResult<CostCurrentInstalationDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentInstalationDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentInstalationDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,
                model.CCInstalationTypeTitle, organizationDisplay)
                );


        }
        #region Private Helper Methods
        private async Task<IQueryable<CostCurrentInstalation>> setFilter(
            IQueryable<CostCurrentInstalation> query,
            CostCurrentInstalationFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostCurrentInstalation>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue)
            {
                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);

                foreach (var org in organizations)
                {
                    predicate.Or(_ => _.OrganizationId == org.Id);
                }

                query = query.Where(predicate);
            }

            if (filter.CCInstalationTypeId.HasValue)
                query = query.Where(x => x.CCInstalationTypeId == filter.CCInstalationTypeId.Value);

            if (filter.ActivityType.HasValue)
                query = query.Where(x => x.ActivityType == filter.ActivityType.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         _.CCInstalationType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostCurrentInstalation> setOrder(
           IQueryable<CostCurrentInstalation> query,
           string orderBy = "id",
           bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {

                case "organization":
                    return desc
                        ? query.OrderByDescending(x => x.Organization.Title)
                        : query.OrderBy(x => x.Organization.Title);

                case "ccInstalationtype":
                    return desc
                        ? query.OrderByDescending(x => x.CCInstalationType.DisplayOrder)
                        : query.OrderBy(x => x.CCInstalationType.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.CCInstalationType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.ActivityType)
                                .ThenBy(x => x.CCInstalationType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostCurrentInstalation>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostCurrentInstalation>();

            foreach (var org in children)
            {
                if (await Query()
                            .Where(_ => _.OrganizationId == org.Id)
                            .Where(_ => _.YearId == targetYearId).AnyAsync())
                {
                    throw new CopyDestYearHasDataException();
                }

                var data = await Query()
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .ToListAsync();

                foreach (var item in data)
                {
                    if (!await checkLogicAsync(targetYearId, org.Id, item.CCInstalationTypeId, item.ActivityType))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentInstalation
                    {
                        CCInstalationTypeId = item.CCInstalationTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActivityType = item.ActivityType,
                        NumberUser = item.NumberUser,
                        Cost = item.Cost,
                        Income = item.Income
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<CostCurrentInstalation>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostCurrentInstalation>();
            foreach (var org in children)
            {
                var data = await Query()
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .ToListAsync();

                foreach (var item in data)
                {
                    result.Add(item);
                }
                result.AddRange(await getChildren(org.Id, yearId));
            }
            return result;
        }
        private async Task<bool> hasAnyDataAsync(int orgid, int yearid)
        {
            bool any = await Query().AnyAsync(x => x.OrganizationId == orgid &&
                                                x.YearId == yearid);
            if (any)
            {
                return true;
            }
            else
            {
                var childs = await _organizationService.GetWithChildrenAsync(orgid);
                foreach (var child in childs)
                    if (await Query().AnyAsync(x => x.YearId == yearid && x.OrganizationId == child.Id))
                        return true;
            }

            return false;

        }
        #endregion

        #region Logics
        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int oIFTypeId,
            ActivityType activityType,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.CCInstalationTypeId == oIFTypeId &&
                                                x.ActivityType == activityType)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.CCInstalationTypeId == oIFTypeId &&
                                            x.ActivityType == activityType &&
                                            x.Id != id);
            return !result;
        }
        #endregion

    }
}
