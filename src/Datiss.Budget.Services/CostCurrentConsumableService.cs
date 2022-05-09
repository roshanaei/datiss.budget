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
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class CostCurrentConsumableService : ICostCurrentConsumableService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostCurrentConsumable> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;


        public CostCurrentConsumableService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentConsumable>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentConsumable> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostCurrentConsumable> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentConsumableDTO>> CreateAsync(CreateCostCurrentConsumableDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new CostCurrentConsumable
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                ActivityType = model.ActivityType,
                ConsumableTypeId = model.ConsumableTypeId,
                ConsumableAmount = model.ConsumableAmount,
                ConsumableCost = model.ConsumableCost
            };

            model.ConsumableTypeDisplay = (await _constSet.FindAsync(model.ConsumableTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ActivityType , model.ConsumableTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentConsumableDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = entity.Adapt<CostCurrentConsumableDTO>();
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.ActivityType = entity.ActivityType;
                    result.ConsumableTypeId = entity.ConsumableTypeId;
                    result.ConsumableTypeDisplay = model.ConsumableTypeDisplay;
                    result.ConsumableAmount = entity.ConsumableAmount;
                    result.ConsumableCost = entity.ConsumableCost;

                    return ValidationResult<CostCurrentConsumableDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentConsumableDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentConsumableDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumableTypeOrgDuplicates,
                model.ConsumableTypeDisplay, organizationDisplay)
                );


        }

        public async Task<ValidationResult<CostCurrentConsumableDTO>> UpdateAsync(UpdateCostCurrentConsumableDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.ConsumableTypeDisplay = (await _constSet.FindAsync(model.ConsumableTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ActivityType,model.ConsumableTypeId ,model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.ActivityType = model.ActivityType;
                    entity.ConsumableTypeId = model.ConsumableTypeId;
                    entity.ConsumableAmount = model.ConsumableAmount;
                    entity.ConsumableCost = model.ConsumableCost;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentConsumableDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = new CostCurrentConsumableDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        ActivityType = model.ActivityType,
                        ConsumableTypeId = model.ConsumableTypeId,
                        ConsumableTypeDisplay = model.ConsumableTypeDisplay,
                        ConsumableAmount = model.ConsumableAmount,
                        ConsumableCost = model.ConsumableCost,
                        OrganizationDisplay = organizationDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<CostCurrentConsumableDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentConsumableDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentConsumableDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumableTypeOrgDuplicates,
                model.ConsumableTypeDisplay, organizationDisplay)
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

        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId, ActivityType activityType)
        {
            var organization = await _orgDbSet.FindAsync(organizationId);
            organization.CheckReferenceIsNull(nameof(organization));

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var self = await _dbSet.Where(_ => _.YearId == yearId)
                                   .Where(_ => _.OrganizationId == organizationId)
                                   .Where(_ => _.ActivityType == activityType)
                                   .ToListAsync();

            IEnumerable<CostCurrentConsumable> childrens = new CostCurrentConsumable[] { };

            if (organization.Type == OrganizationType.County || organization.Type == OrganizationType.Root)
            {
                childrens = await getChildren(organizationId, yearId , activityType);
            }

            if (self.Count() == 0 && childrens.Count() == 0)
                throw new DeleteNullRecordException();

            _dbSet.RemoveRange(self);

            if (childrens.Any())
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
                Key = "CostCurrentConsumable_Cal1",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostCurrentConsumable_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentConsumable_Cal2",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostCurrentConsumable_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentConsumable_Cal3",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostCurrentConsumable_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentConsumable_Cal4",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostCurrentConsumable_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentConsumable_Cal5",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostCurrentConsumable_Cal5] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentConsumable_Cal6",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostCurrentConsumable_Cal6] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });


            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostCurrentConsumableDTO>> GetListAsync(CostCurrentConsumableFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentConsumableDTO>
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
                                    .Select(x => new CostCurrentConsumableDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        ConsumableTypeId = x.ConsumableTypeId,
                                        ConsumableTypeDisplay = x.ConsumableType.Title,
                                        ConsumableAmount = x.ConsumableAmount,
                                        ConsumableCost = x.ConsumableCost
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId, ActivityType activityType)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            if (!await hasAnyDataAsync(sourceOrgId, sourceYearId, activityType))
                throw new CopyOrgNullDataException();
            var result = new List<CostCurrentConsumable>();

            if (await Query()
                        .Where(_ => _.OrganizationId == sourceOrgId)
                        .Where(_ => _.YearId == destYearId)
                        .Where(_ => _.ActivityType == activityType)
                        .AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .Where(_ => _.ActivityType == activityType)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, item.OrganizationId, item.ActivityType , item.ConsumableTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentConsumable
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityType = item.ActivityType,
                        ConsumableTypeId = item.ConsumableTypeId,
                        ConsumableAmount = item.ConsumableAmount,
                        ConsumableCost = item.ConsumableCost
                    };
                    result.Add(entity);
                }
            }

            var childrens = await getChildrenData(sourceOrgId, sourceYearId, destYearId, activityType);

            if (childrens.Any())
            {
                result.AddRange(childrens);
            }

            _dbSet.AddRange(result);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                throw new CopyDataBaseException();
            }
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, ActivityType activityType, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostCurrentConsumableImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentConsumable>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                 .GetAllDescendentsAsync(_userContext.OrganizationId);

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;
                rec.ActivityType = activityType;
                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex + 2, rec.YearId)
                        );
                }
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rowIndex + 2, rec.OrganizationId)
                        );
                }
                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex + 2)
                        );
                }

                rowIndex++;
            }

            rowIndex = 1;

            if (!continueIfAnyOrgMissing)
            {
                var missingOrgs = new List<Organization>();

                foreach (var item in descendents)
                {
                    var existInExcel = records.Any(_ => _.OrganizationId == item.Id);
                    if (!existInExcel)
                        if (item.Type == Enum.OrganizationType.City || item.Type == Enum.OrganizationType.Village)
                            missingOrgs.Add(item);
                }

                if (missingOrgs.Any())
                {
                    string orgNames = "";
                    foreach (var item in missingOrgs)
                    {
                        orgNames += "- " + item.Title + "<br>";
                    }

                    return new ImportResult
                    {
                        Message = orgNames,
                        AskToImport = true
                    };
                }
            }

            foreach (var record in records)
            {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelAccessError, rowIndex + 2)
                        );

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.ActivityType,
                    record.ConsumableTypeId))
                {

                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelLogicError, rowIndex + 2)
                        );
                }

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelCalculationField)
                    );
            }

            return ImportResult.Succeed(
                string.Format(ServiceMessages.ImportExcelSuccess)
                );
        }

        public async Task<IEnumerable<CostCurrentConsumableDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostCurrentConsumableFilterDTO
            {
                OrganizationId = organizationId,
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, "getexport", filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Select(x => new CostCurrentConsumableDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        ConsumableTypeId = x.ConsumableTypeId,
                                        ConsumableTypeDisplay = x.ConsumableType.Title,
                                        ConsumableAmount = x.ConsumableAmount,
                                        ConsumableCost = x.ConsumableCost
                                    }).ToListAsync();
            return items;
        }

        public async Task<Stream> ExportExcelAsync(CostCurrentConsumableFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Select(x => new CostCurrentConsumableDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        ConsumableTypeId = x.ConsumableTypeId,
                                        ConsumableTypeDisplay = x.ConsumableType.Title,
                                        ConsumableAmount = x.ConsumableAmount,
                                        ConsumableCost = x.ConsumableCost
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<CostCurrentConsumable>> setFilter(
            IQueryable<CostCurrentConsumable> query,
            CostCurrentConsumableFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostCurrentConsumable>();

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
            if (filter.ActivityType.HasValue)
                query = query.Where(x => x.ActivityType == filter.ActivityType.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostCurrentConsumable> setOrder(
           IQueryable<CostCurrentConsumable> query,
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
                case "getexport":
                    return query.Include(x => x.Organization)
                                .Include(x => x.ConsumableType)
                                .OrderBy(x => x.ActivityType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.ConsumableType.DisplayOrder);
                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.ConsumableType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x=>x.ConsumableType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostCurrentConsumable>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId,
            ActivityType activityType)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostCurrentConsumable>();

            foreach (var org in children)
            {
                if (await Query()
                            .Where(_ => _.OrganizationId == org.Id)
                            .Where(_ => _.YearId == targetYearId)
                            .Where(_ => _.ActivityType == activityType).AnyAsync())
                {
                    throw new CopyDestYearHasDataException();
                }

                var data = await Query()
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .Where(_ => _.ActivityType == activityType)
                                .ToListAsync();

                foreach (var item in data)
                {
                    if (!await checkLogicAsync(targetYearId, item.OrganizationId, item.ActivityType , item.ConsumableTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentConsumable
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActivityType = item.ActivityType,
                        ConsumableTypeId = item.ConsumableTypeId,
                        ConsumableAmount = item.ConsumableAmount,
                        ConsumableCost = item.ConsumableCost
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId, activityType));
            }

            return result;
        }
        private async Task<IEnumerable<CostCurrentConsumable>> getChildren(
            int parentOrganizationId,
            int yearId,
            ActivityType activityType)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostCurrentConsumable>();
            foreach (var org in children)
            {
                var data = await Query()
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .Where(_ => _.ActivityType == activityType)
                                .ToListAsync();

                foreach (var item in data)
                {
                    result.Add(item);
                }
                result.AddRange(await getChildren(org.Id, yearId, activityType));
            }
            return result;
        }
        private async Task<bool> hasAnyDataAsync(int orgid, int yearid, ActivityType activityType)
        {
            bool any = await Query().AnyAsync(x => x.OrganizationId == orgid &&
                                                x.YearId == yearid &&
                                                x.ActivityType == activityType);
            if (any)
            {
                return true;
            }
            else
            {
                var childs = await _organizationService.GetWithChildrenAsync(orgid);
                foreach (var child in childs)
                    if (await Query().AnyAsync(x => x.YearId == yearid &&
                                                    x.OrganizationId == child.Id &&
                                                    x.ActivityType == activityType))
                        return true;
            }

            return false;

        }
        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            ActivityType activityType,
            int consumableTypeId,
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
                                                x.ConsumableTypeId == consumableTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.ActivityType == activityType &&
                                            x.ConsumableTypeId == consumableTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
