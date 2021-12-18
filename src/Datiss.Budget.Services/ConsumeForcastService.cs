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
using Datiss.Budget.Security;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{
    public class ConsumeForcastService : IConsumeForcastService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<ConsumeForcast> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _ConstSet;

        public ConsumeForcastService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<ConsumeForcast>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _ConstSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<ConsumeForcast> Query()
            => _dbSet.AsNoTracking();

        public async Task<ConsumeForcast> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<ConsumeForcastDTO>> CreateAsync(CreateConsumeForcastDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new ConsumeForcast
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                UsageLayerId = model.UsageLayerId,
                CountUser = model.CountUser,
                UnitUser = model.UnitUser,
                ConsumeUser = model.ConsumeUser,
                AvgConsumeUser = model.AvgConsumeUser
            };

            if(await checkLogitcAsync(model.YearId, model.OrganizationId, model.UserTypeId,model.UsageLayerId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                var result = entity.Adapt<ConsumeForcastDTO>();
                result.UsageLayerTitle = (await _ConstSet.FindAsync(model.UsageLayerId)).Title;
                result.UserTypeTitle = (await _ConstSet.FindAsync(model.UserTypeId)).Title;
                result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                result.CountUser = entity.CountUser;
                result.UnitUser = entity.UnitUser;
                result.ConsumeUser = model.ConsumeUser;
                result.AvgConsumeUser = model.AvgConsumeUser;

                return ValidationResult<ConsumeForcastDTO>.Success(result);
            }

            return ValidationResult<ConsumeForcastDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumeForcast,
                                                model.UserTypeTitle,
                                                model.UsageLayerTitle)
                );
        }

        public async Task<ValidationResult<ConsumeForcastDTO>> UpdateAsync(UpdateConsumeForcastDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if(await checkLogitcAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.YearId = model.YearId;
                entity.OrganizationId = model.OrganizationId;
                entity.UserTypeId = model.UserTypeId;
                entity.UsageLayerId = model.UsageLayerId;
                entity.UnitUser = model.UnitUser;
                entity.CountUser = model.CountUser;
                entity.ConsumeUser = model.ConsumeUser;
                entity.AvgConsumeUser = model.AvgConsumeUser;
                entity.ConsumeUserForcast = model.ConsumeUserForcast;

                await _uow.SaveChangesAsync();

                var result = new ConsumeForcastDTO
                {
                    YearId = model.YearId,
                    OrganizationId = model.OrganizationId,
                    UserTypeId = model.UserTypeId,
                    UsageLayerId = model.UsageLayerId,
                    CountUser = model.CountUser,
                    UnitUser = model.UnitUser,
                    ConsumeUser = model.ConsumeUser,
                    AvgConsumeUser = model.AvgConsumeUser,
                    ConsumeUserForcast = model.ConsumeUserForcast,
                    Year = (await _yearSet.FindAsync(model.YearId)).Year,
                    OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                    UserTypeTitle = (await _ConstSet.FindAsync(model.UserTypeId)).Title,
                    UsageLayerTitle = (await _ConstSet.FindAsync(model.UsageLayerId)).Title
                };

                return ValidationResult<ConsumeForcastDTO>.Success(result);
            }

            return ValidationResult<ConsumeForcastDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumeForcast,
                                    model.UserTypeTitle,
                                    model.UsageLayerTitle)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);

            await _uow.SaveChangesAsync();
        }

        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId,int organizationId)
        {
            var organization = await _orgDbSet.FindAsync(organizationId);
            organization.CheckReferenceIsNull(nameof(organization));

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            var self = await _dbSet.Where(x => x.YearId == yearId)
                                   .Where(x => x.OrganizationId == organizationId)
                                   .ToListAsync();

            if (self.Count == 0)
                throw new DeleteNullRecordException();

            _dbSet.RemoveRange(self);
            var childrens = await getChildren(organizationId, yearId);
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

        public async Task<int> CalculationAsync(int yearId,int organizationId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter ("YearId",yearId),
                new SqlParameter ("OrganizationId",organizationId)
            };

            var result = await _uow.ExecuteScalarAsync<int>(
                "[dbo].[WaterInstallFees_Cal1] @YearId, @OrganizationId",
                parameters: sqlParams.ToArray());

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<ConsumeForcastDTO>> GetListAsync(ConsumeForcastFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<ConsumeForcastDTO>
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
                                        .Include(x => x.UsageLayer)
                                        .Select(x => new ConsumeForcastDTO
                                        {
                                            Id = x.Id,
                                            YearId = x.YearId,
                                            Year = x.FinanceYear.Year,
                                            OrganizationId = x.OrganizationId,
                                            OrganizationDisplay = x.Organization.Title,
                                            UserTypeId = x.UserTypeId,
                                            UserTypeTitle = x.UserType.Title,
                                            UsageLayerId = x.UsageLayerId,
                                            UsageLayerTitle = x.UsageLayer.Title,
                                            CountUser = x.CountUser,
                                            UnitUser = x.UnitUser,
                                            ConsumeUser = x.ConsumeUser,
                                            AvgConsumeUser = x.AvgConsumeUser,
                                            ConsumeUserForcast = x.ConsumeUserForcast
                                        }).ToListAsync();
            return await Task.FromResult(result);
        }

        #region Privte Helper Methods
        private async Task<IQueryable<ConsumeForcast>> setFilter(
            IQueryable<ConsumeForcast> query,
            ConsumeForcastFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<ConsumeForcast>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue)
            {
                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);

                foreach(var org in organizations)
                {
                    predicate.Or(x => x.OrganizationId == org.Id);
                }

                query = query.Where(predicate);
            }

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(x => x.Organization.Title.ToUpper().Contains(filter.Search) || 
                                         x.UserType.Title.ToUpper().Contains(filter.Search));   
            }

            return query;
        }

        private IQueryable<ConsumeForcast> setOrder(
            IQueryable<ConsumeForcast> query,
            string orderBy = "id",
            bool desc = false){
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "year":
                    return desc
                        ? query.OrderByDescending(x => x.FinanceYear.Year)
                        : query.OrderBy(x => x.FinanceYear.Year);

                case "organization":
                    return desc
                        ? query.OrderByDescending(x => x.Organization.Title)
                        : query.OrderBy(x => x.Organization.Title);

                case "UserType":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);

                case "UsageLayer":
                    return desc
                        ? query.OrderByDescending(x => x.UsageLayer.DisplayOrder)
                        : query.OrderBy(x => x.UsageLayer.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .Include(x => x.UsageLayer)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.UsageLayer.DisplayOrder);
            }
        }

        private async Task<IEnumerable<ConsumeForcast>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId){

            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<ConsumeForcast>();

            foreach(var org in children){
                if (await Query()
                            .Where(x => x.OrganizationId == org.Id)
                            .Where(x => x.YearId == targetYearId).AnyAsync())
                {
                    throw new CopyDestYearHasDataException();
                }

                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .ToListAsync();

                foreach(var item in data){
                    if (!await checkLogitcAsync(targetYearId, org.Id, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new ConsumeForcast
                    {
                        UserTypeId = item.UserTypeId,
                        UsageLayerId = item.UsageLayerId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        CountUser = item.CountUser,
                        UnitUser = item.UnitUser,
                        ConsumeUser = item.ConsumeUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }

        private async Task<IEnumerable<ConsumeForcast>> getChildren( 
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<ConsumeForcast>();
            foreach(var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .ToListAsync();

                foreach(var item in data)
                {
                    result.Add(item);
                }
                result.AddRange(await getChildren(org.Id, yearId));
            }
            return result;
        }

        private async Task<bool> hasAnyDataAsync(int orgid,int yearid)
        {
            bool any = await Query().AnyAsync(x => x.OrganizationId == orgid &&
                                                   x.YearId == yearid);

            if(any)
            {
                return true;
            }
            else
            {
                if(await Query().Include(x => x.Organization)
                                .AnyAsync(x => x.Organization.ParentId == orgid &&
                                                x.YearId == yearid))
                {
                    return true;
                }
                var childs = await _orgDbSet.Where(x => x.ParentId == orgid).ToListAsync();
                foreach (var child in childs)
                    return await hasAnyDataAsync(child.Id, yearid);
            }
            return false;
        }

        #endregion

        #region Logics
        private async Task<bool> checkLogitcAsync(
             int yearId,
             int organizationId,
             int userTypeId,
             int usageLayerId,
             int? id = null) {
            var result = id == null
                   ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                   x.OrganizationId == organizationId &&
                                                   x.UserTypeId == userTypeId &&
                                                   x.UsageLayerId == usageLayerId)

                   : await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId &&
                                                x.UsageLayerId == usageLayerId &&
                                                x.Id != id);

            return !result;
        }
        
        #endregion
    }
}
