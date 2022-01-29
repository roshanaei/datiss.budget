using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Resources;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Entities;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Services.Contracts.Identity;
using Mapster;
using LinqKit;
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Security;
using Datiss.Budget.Enum;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Common;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{
    public class IncomeCurrentWsNHService : IIncomeCurrentWsNHService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentWsNH> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeCurrentWsNHService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentWsNH>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentWsNH> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentWsNH> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeCurrentWsNHDTO>> CreateAsync(CreateIncomeCurrentWsNHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeCurrentWsNH
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                NumberUser = model.NumberUser,
                UnitUser = model.UnitUser,
                AvgConsumeUser = model.AvgConsumeUser,
                ConsumptionUser = model.ConsumptionUser,
                Capacity = model.Capacity,
                Cost = model.Cost,
                Income = model.Income,
                ExcessIncome = model.ExcessIncome,
                SeasonalIncome = model.SeasonalIncome,
                Note3Price = model.Note3Price,
                Note3Income = model.Note3Income,
                SubscriptionIncome = model.SubscriptionIncome,
                TotalIncome = model.TotalIncome,
                Note7Price = model.Note7Price,
                Note7Income = model.Note7Income
            };

            var usertypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentWsNHDTO>();
                    result.UserTypeDisplay = usertypeDisplay;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberUser = model.NumberUser;
                    result.UnitUser = model.UnitUser;
                    result.AvgConsumeUser = model.AvgConsumeUser;
                    result.ConsumptionUser = model.ConsumptionUser;
                    result.Capacity = model.Capacity;
                    result.Cost = model.Cost;
                    result.Income = model.Income;
                    result.ExcessIncome = model.ExcessIncome;
                    result.SeasonalIncome = model.SeasonalIncome;
                    result.Note3Price = model.Note3Price;
                    result.Note3Income = model.Note3Income;
                    result.SubscriptionIncome = model.SubscriptionIncome;
                    result.TotalIncome = model.TotalIncome;
                    result.Note7Price = model.Note7Price;
                    result.Note7Income = model.Note7Income;

                    return ValidationResult<IncomeCurrentWsNHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsNHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWsNHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                usertypeDisplay, organizationDisplay)
                );
        }

        public async Task<ValidationResult<IncomeCurrentWsNHDTO>> UpdateAsync(UpdateIncomeCurrentWsNHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var usertypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.NumberUser = model.NumberUser;
                    entity.UnitUser = model.UnitUser;
                    entity.AvgConsumeUser = model.AvgConsumeUser;
                    entity.ConsumptionUser = model.ConsumptionUser;
                    entity.Capacity = model.Capacity;
                    entity.Cost = model.Cost;
                    entity.Income = model.Income;
                    entity.ExcessIncome = model.ExcessIncome;
                    entity.SeasonalIncome = model.SeasonalIncome;
                    entity.Note3Price = model.Note3Price;
                    entity.Note3Income = model.Note3Income;
                    entity.SubscriptionIncome = model.SubscriptionIncome;
                    entity.TotalIncome = model.TotalIncome;
                    entity.Note7Price = model.Note7Price;
                    entity.Note7Income = model.Note7Price;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentWsNHDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        NumberUser = model.NumberUser,
                        UnitUser = model.UnitUser,
                        AvgConsumeUser = model.AvgConsumeUser,
                        ConsumptionUser = model.ConsumptionUser,
                        Capacity = model.Capacity,
                        Cost = model.Cost,
                        Income = model.Income,
                        ExcessIncome = model.ExcessIncome,
                        SeasonalIncome = model.SeasonalIncome,
                        Note3Price = model.Note3Price,
                        Note3Income = model.Note3Income,
                        SubscriptionIncome = model.SubscriptionIncome,
                        TotalIncome = model.TotalIncome,
                        Note7Price = model.Note7Income,
                        Note7Income = model.Note7Income,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = usertypeDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<IncomeCurrentWsNHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsNHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWsNHDTO>.Failed(
                string.Format(ServiceMessages.Logic_DiameterPipeOrgDuplicate,
                usertypeDisplay, organizationDisplay)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckReferenceIsNull(nameof(entity));

            var year = await _yearSet.FindAsync(entity.YearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();
            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);

            await _uow.SaveChangesAsync();
        }


        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId)
        {
            var organization = await _orgDbSet.FindAsync(organizationId);
            organization.CheckReferenceIsNull(nameof(organization));

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var self = await _dbSet.Where(_ => _.YearId == yearId)
                                    .Where(_ => _.OrganizationId == organizationId)
                                    .ToListAsync();
            var childrens = await getChildren(organizationId, yearId);

            if (self.Count() == 0 && childrens.Count() == 0)
                throw new DeleteNullRecordException();
            _dbSet.RemoveRange(self);

            _dbSet.RemoveRange(childrens);

            var result = new OrganizationDeleteDataResult
            {
                OrganizationTitle = organization.Title,
                Year = year.Year,
                YearTitle = year.Title
            };

            await _uow.SaveChangesAsync();

            return await Task.FromResult(result);
        }


        public async Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId)
        {
            var result = new List<CalculationItemData>();
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsNH_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[IncomeCurrentWsNH_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsNH_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[IncomeCurrentWsNH_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<IncomeCurrentWsNHDTO>> GetListAsync(IncomeCurrentWsNHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<IncomeCurrentWsNHDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            query = await setFilter(query, filter);

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query.Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new IncomeCurrentWsNHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Capacity = x.Capacity,
                                        Cost = x.Cost,
                                        Income = x.Income,
                                        ExcessIncome = x.ExcessIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        TotalIncome = x.TotalIncome,
                                        Note7Price = x.Note7Price,
                                        Note7Income = x.Note7Income,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }


        #region Private Helper Methods
        private async Task<IQueryable<IncomeCurrentWsNH>> setFilter(
    IQueryable<IncomeCurrentWsNH> query,
    IncomeCurrentWsNHFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<IncomeCurrentWsNH>();

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

            if (filter.UserTypeId.HasValue)
                query = query.Where(x => x.UserTypeId == filter.UserTypeId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ =>  _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                          _.UserType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<IncomeCurrentWsNH> setOrder(
           IQueryable<IncomeCurrentWsNH> query,
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

                case "usertype":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.UserType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<IncomeCurrentWsNH>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<IncomeCurrentWsNH>();
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

        #endregion

        #region Logics
        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int userTypeId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == userTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion

    }
}
