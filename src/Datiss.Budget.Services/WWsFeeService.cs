using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class WWsFeeService : IWWsFeeService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<WWsFee> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public WWsFeeService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WWsFee>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<WWsFee> Query()
            => _dbSet.AsNoTracking();

        public async Task<WWsFee> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<WWsFeeDTO>> CreateAsync(CreateWWsFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new WWsFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                ActivityType = model.ActivityType,
                UserTypeId = model.UserTypeId,
                UsageLayerId = model.UsageLayerId,
                P1Fee = model.P1Fee,
                P2Fee = model.P2Fee,
                P1Note3 = model.P1Note3,
                P1Note7 = model.P1Note7,
                P2Note3 = model.P2Note3,
                P2Note7 = model.P2Note7                
            };

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            var usageLayerDisplay = (await _constSet.FindAsync(model.UsageLayerId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId,model.ActivityType, model.UserTypeId, model.UsageLayerId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<WWsFeeDTO>();
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.UsageLayerDisplay = usageLayerDisplay;
                    result.ActivityType = model.ActivityType;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.P1Fee = entity.P1Fee;
                    result.P2Fee = entity.P2Fee;
                    result.P1Note3 = entity.P1Note3;
                    result.P1Note7 = entity.P1Note7;
                    result.P2Note3 = entity.P2Note3;
                    result.P2Note7 = entity.P2Note7;

                    return ValidationResult<WWsFeeDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<WWsFeeDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<WWsFeeDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                model.UserTypeTitle, usageLayerDisplay, organizationDisplay)
                );
        }

        public async Task<ValidationResult<WWsFeeDTO>> UpdateAsync(UpdateWWsFeeDTO model)
        {
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            var usageLayerDisplay = (await _orgDbSet.FindAsync(model.UsageLayerId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ActivityType, model.UserTypeId, model.UsageLayerId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.ActivityType = model.ActivityType;
                    entity.UserTypeId = model.UserTypeId;
                    entity.UsageLayerId = model.UsageLayerId;
                    entity.P1Fee = model.P1Fee;
                    entity.P2Fee = model.P2Fee;
                    entity.P1Note3 = model.P1Note3;
                    entity.P1Note7 = model.P1Note7;
                    entity.P2Note3 = model.P2Note3;
                    entity.P2Note7 = model.P1Note7;

                    await _uow.SaveChangesAsync();

                    var result = new WWsFeeDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        ActivityType = model.ActivityType,
                        UsageLayerId = model.UsageLayerId,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = model.UserTypeTitle,
                        UsageLayerDisplay = usageLayerDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<WWsFeeDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<WWsFeeDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<WWsFeeDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                model.UserTypeTitle, usageLayerDisplay, organizationDisplay)
                );
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

        public async Task<PagedResult<WWsFeeDTO>> GetListAsync(WWsFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<WWsFeeDTO>
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
                                    .Select(x => new WWsFeeDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        UsageLayerId = x.UsageLayerId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        P1Fee = x.P1Fee,
                                        P2Fee = x.P2Fee,
                                        P1Note3 = x.P1Note3,
                                        P2Note3 = x.P2Note3,
                                        P1Note7 = x.P1Note7,
                                        P2Note7 = x.P2Note7
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();

            var result = new List<WWsFee>();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    var entity = new WWsFee
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityType = item.ActivityType,
                        UsageLayerId = item.UsageLayerId,
                        P1Fee = item.P1Fee,
                        P2Fee = item.P2Fee,
                        P1Note3 = item.P1Note3,
                        P2Note3 = item.P2Note3,
                        P1Note7 = item.P1Note7,
                        P2Note7 = item.P2Note7
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
            var data = await _excelService.ImportAsync<WWsFeeImportModel>(fileInfo);

            var records = data.Adapt<List<WWsFee>>();

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

        public async Task<IEnumerable<WWsFeeDTO>> GetExportItemsAsync(WWsFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new WWsFeeDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        UsageLayerId = x.UsageLayerId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        P1Fee = x.P1Fee,
                                        P2Fee = x.P2Fee,
                                        P1Note3 = x.P1Note3,
                                        P2Note3 = x.P2Note3,
                                        P1Note7 = x.P1Note7,
                                        P2Note7 = x.P2Note7
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(WWsFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new WWsFeeDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        UsageLayerId = x.UsageLayerId,
                                        UsageLayerDisplay = x.UserType.Title,
                                        P1Fee = x.P1Fee,
                                        P2Fee = x.P2Fee,
                                        P1Note3 = x.P1Note3,
                                        P2Note3 = x.P2Note3,
                                        P1Note7 = x.P1Note7,
                                        P2Note7 = x.P2Note7
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<WWsFee>> setFilter(
            IQueryable<WWsFee> query,
            WWsFeeFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<WWsFee>();

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
            if (filter.ActivityType.HasValue)
                query = query.Where(x => x.ActivityType == filter.ActivityType);
            if (filter.UsageLayerId.HasValue)
                query = query.Where(x => x.UsageLayerId == filter.UsageLayerId.Value);

            return query;
        }

        private IQueryable<WWsFee> setOrder(
           IQueryable<WWsFee> query,
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
                case "activitytype":
                    return desc
                        ? query.OrderByDescending(x => x.ActivityType)
                        : query.OrderBy(x => x.ActivityType);
                case "usagelayer":
                    return desc
                        ? query.OrderByDescending(x => x.UsageLayer.DisplayOrder)
                        : query.OrderBy(x => x.UsageLayer.DisplayOrder);
                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }

        private async Task<IEnumerable<WWsFee>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<WWsFee>();

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
                    var entity = new WWsFee
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActivityType = item.ActivityType,
                        UsageLayerId = item.UsageLayerId,
                        P1Fee = item.P1Fee,
                        P2Fee = item.P2Fee,
                        P1Note3 = item.P1Note3,
                        P2Note3 = item.P2Note3,
                        P1Note7 = item.P1Note7,
                        P2Note7 = item.P2Note7
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
            ActivityType activityType,
            int userTypeId,
            int UsageLayerId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.ActivityType == activityType &&
                                                x.UserTypeId == userTypeId &&
                                                x.UsageLayerId == UsageLayerId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                              x.OrganizationId == organizationId &&
                                              x.ActivityType == activityType &&
                                              x.UserTypeId == userTypeId &&
                                              x.Id != id);
            return !result;
        }

        #endregion
    }
}
