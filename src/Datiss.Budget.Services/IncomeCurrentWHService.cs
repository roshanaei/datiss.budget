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

namespace Datiss.Budget.Services
{
    public class IncomeCurrentWHService : IIncomeCurrentWHService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentWH> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;

        public IncomeCurrentWHService(
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentWH>();
            _orgDbSet = _uow.Set<Organization>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentWH> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentWH> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);

            await _uow.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int yearId, int organizationId)
        {
            var items = await _dbSet.Where(_ => _.YearId == yearId)
                                    .Where(_ => _.OrganizationId == organizationId)
                                    .ToListAsync();

            _dbSet.RemoveRange(items);

            await _uow.SaveChangesAsync();
        }
        public async Task<PagedResult<IncomeCurrentWHDTO>> GetListAsync(IncomeCurrentWHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<IncomeCurrentWHDTO>
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
                                    .Select(x => new IncomeCurrentWHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        UsageLayerId = x.UsageLayerId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Cost = x.Cost,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        Income = x.Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        TIncome = x.TIncome,
                                        Diff_ConsWsVolume = x.Diff_ConsWsVolume,
                                        Note2Income = x.Note2Income,
                                        WasteVolume = x.WasteVolume,
                                        Note7Price = x.Note7Price,
                                        Note7Income = x.Note7Income
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();

            var result = new List<IncomeCurrentWH>();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    var entity = new IncomeCurrentWH
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        UsageLayerId = item.UsageLayerId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                        ConsumptionUser = item.ConsumptionUser,
                        Cost = item.Cost,
                        Note3Price = item.Note3Price,
                        Note3Income = item.Note3Income,
                        Income = item.Income,
                        SubscriptionIncome = item.SubscriptionIncome,
                        SeasonalIncome = item.SeasonalIncome,
                        TIncome = item.TIncome,
                        Diff_ConsWsVolume = item.Diff_ConsWsVolume,
                        Note2Income = item.Note2Income,
                        WasteVolume = item.WasteVolume,
                        Note7Price = item.Note7Price,
                        Note7Income = item.Note7Income
                    };
                    result.Add(entity);
                }
            }

            var childrens = await getChildrenData(sourceOrgId, sourceYearId, destYearId);

            if (childrens.Any())
            {
                result.AddRange(childrens);
            }

            _dbSet.AddRange(result);

            await _uow.SaveChangesAsync();
        }

        public async Task ImportExcelAsync(IFormFile fileInfo)
        {
            var data = await _excelService.ImportAsync<IncomeCurrentWHImportModel>(fileInfo);

            var records = data.Adapt<List<IncomeCurrentWH>>();

            int rowIndex = 1;

            foreach (var record in records)
            {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    throw new UserOrganizationAccessException(rowIndex);

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.UserTypeId))
                    throw new ImportExcelFileException(rowIndex);

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<IncomeCurrentWHDTO>> GetExportItemsAsync(IncomeCurrentWHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new IncomeCurrentWHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        UsageLayerId = x.UsageLayerId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Cost = x.Cost,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        Income = x.Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        TIncome = x.TIncome,
                                        Diff_ConsWsVolume = x.Diff_ConsWsVolume,
                                        Note2Income = x.Note2Income,
                                        WasteVolume = x.WasteVolume,
                                        Note7Price = x.Note7Price,
                                        Note7Income = x.Note7Income
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(IncomeCurrentWHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new IncomeCurrentWHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        UsageLayerId = x.UsageLayerId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Cost = x.Cost,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        Income = x.Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        TIncome = x.TIncome,
                                        Diff_ConsWsVolume = x.Diff_ConsWsVolume,
                                        Note2Income = x.Note2Income,
                                        WasteVolume = x.WasteVolume,
                                        Note7Price = x.Note7Price,
                                        Note7Income = x.Note7Income
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<IncomeCurrentWH>> setFilter(
            IQueryable<IncomeCurrentWH> query,
            IncomeCurrentWHFilterDTO filter)
        {

            var predicate = PredicateBuilder.New<IncomeCurrentWH>();

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
            if (filter.UsageLayerId.HasValue)
                query = query.Where(x => x.UsageLayerId == filter.UsageLayerId.Value);

            return query;
        }

        private IQueryable<IncomeCurrentWH> setOrder(
           IQueryable<IncomeCurrentWH> query,
           string orderBy = "id",
           bool desc = false)
        {
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

                case "usertype":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);
                case "usageLayer":
                    return desc
                        ? query.OrderByDescending(x => x.UsageLayer.DisplayOrder)
                        : query.OrderBy(x => x.UsageLayer.DisplayOrder);
                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }

        private async Task<IEnumerable<IncomeCurrentWH>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<IncomeCurrentWH>();

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
                    var entity = new IncomeCurrentWH
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        UsageLayerId = item.UsageLayerId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                        ConsumptionUser = item.ConsumptionUser,
                        Cost = item.Cost,
                        Note3Price = item.Note3Price,
                        Note3Income = item.Note3Income,
                        Income = item.Income,
                        SubscriptionIncome = item.SubscriptionIncome,
                        SeasonalIncome = item.SeasonalIncome,
                        TIncome = item.TIncome,
                        Diff_ConsWsVolume = item.Diff_ConsWsVolume,
                        Note2Income = item.Note2Income,
                        WasteVolume = item.WasteVolume,
                        Note7Price = item.Note7Price,
                        Note7Income = item.Note7Income
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
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
