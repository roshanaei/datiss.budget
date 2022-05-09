using Datiss.Budget.Common;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class CostCurrentWaterSourceService : ICostCurrentWaterSourceService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;


        private readonly DbSet<CostCurrentWaterSource> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;


        public CostCurrentWaterSourceService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentWaterSource>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentWaterSource> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostCurrentWaterSource> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentWaterSourceDTO>> CreateAsync(CreateCostCurrentWaterSourceDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new CostCurrentWaterSource
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                WaterSourceTypeId = model.WaterSourceTypeId,
                ActiveSource = model.ActiveSource,
                BaseProduction = model.BaseProduction,
                LastYearProduction = model.LastYearProduction,
                ForcastProduction = model.ForcastProduction
            };

            model.WaterSourceTypeTitle = (await _constSet.FindAsync(model.WaterSourceTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.WaterSourceTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentWaterSourceDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = entity.Adapt<CostCurrentWaterSourceDTO>();
                    result.WaterSourceTypeDisplay = model.WaterSourceTypeTitle;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.ActiveSource = model.ActiveSource;
                    result.BaseProduction = model.BaseProduction;
                    result.LastYearProduction = model.LastYearProduction;
                    result.ForcastProduction = model.ForcastProduction;

                    return ValidationResult<CostCurrentWaterSourceDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentWaterSourceDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentWaterSourceDTO>.Failed(
                string.Format(ServiceMessages.Logic_WaterSourceTypeDuplicate,
                model.WaterSourceTypeTitle, organizationDisplay)
                );
        }

        public async Task<ValidationResult<CostCurrentWaterSourceDTO>> UpdateAsync(UpdateCostCurrentWaterSourceDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.WaterSourceTypeTitle = (await _constSet.FindAsync(model.WaterSourceTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.WaterSourceTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.WaterSourceTypeId = model.WaterSourceTypeId;
                    entity.ActiveSource = model.ActiveSource;
                    entity.BaseProduction = model.BaseProduction;
                    entity.LastYearProduction = model.LastYearProduction;
                    entity.ForcastProduction = model.ForcastProduction;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentWaterSourceDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = new CostCurrentWaterSourceDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        WaterSourceTypeId = model.WaterSourceTypeId,
                        ActiveSource = model.ActiveSource,
                        BaseProduction = model.BaseProduction,
                        LastYearProduction = model.LastYearProduction,
                        ForcastProduction = model.ForcastProduction,
                        OrganizationDisplay = organizationDisplay,
                        WaterSourceTypeDisplay = model.WaterSourceTypeTitle,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<CostCurrentWaterSourceDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentWaterSourceDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }
            return ValidationResult<CostCurrentWaterSourceDTO>.Failed(
                string.Format(ServiceMessages.Logic_WaterSourceTypeDuplicate,
                 model.WaterSourceTypeTitle, organizationDisplay)
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

            IEnumerable<CostCurrentWaterSource> childrens = new CostCurrentWaterSource[] { };

            if (organization.Type == OrganizationType.County || organization.Type == OrganizationType.Root)
            {
                childrens = await getChildren(organizationId, yearId);
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
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            var result = new List<CalculationItemData>();


            result.Add(new CalculationItemData
            {
                Key = "CostCurrentWaterSource_Cal1",
                Value = await _uow.ExecuteScalar<long>(
                        "[dbo].[CostCurrentWaterSource_Cal1] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentWaterSource_Cal2",
                Value = await _uow.ExecuteScalar<long>(
                        "[dbo].[CostCurrentWaterSource_Cal2] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentWaterSource_Cal3",
                Value = await _uow.ExecuteScalar<long>(
                        "[dbo].[CostCurrentWaterSource_Cal3] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentWaterSource_Cal4",
                Value = await _uow.ExecuteScalar<long>(
                        "[dbo].[CostCurrentWaterSource_Cal4] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentWaterSource_Cal5",
                Value = await _uow.ExecuteScalar<long>(
                        "[dbo].[CostCurrentWaterSource_Cal5] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostCurrentWaterSource_Cal6",
                Value = await _uow.ExecuteScalar<long>(
                        "[dbo].[CostCurrentWaterSource_Cal6] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostCurrentWaterSourceDTO>> GetListAsync(CostCurrentWaterSourceFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentWaterSourceDTO>
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
                                    .Include(x => x.WaterSourceType)
                                    .Select(x => new CostCurrentWaterSourceDTO
                                    {
                                        Id = x.Id,
                                        WaterSourceTypeDisplay = x.WaterSourceType.Title,
                                        WaterSourceTypeId = x.WaterSourceTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActiveSource = x.ActiveSource,
                                        BaseProduction = x.BaseProduction,
                                        LastYearProduction = x.LastYearProduction,
                                        ForcastProduction = x.ForcastProduction,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            if (!await hasAnyDataAsync(sourceOrgId, sourceYearId))
                throw new CopyOrgNullDataException();
            var result = new List<CostCurrentWaterSource>();

            if (await Query()
                        .Where(_ => _.OrganizationId == sourceOrgId)
                        .Where(_ => _.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.WaterSourceTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentWaterSource
                    {
                        WaterSourceTypeId = item.WaterSourceTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActiveSource = item.ActiveSource,
                        BaseProduction = item.BaseProduction,
                        LastYearProduction = item.LastYearProduction,
                        ForcastProduction = item.ForcastProduction
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

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                throw new CopyDataBaseException();
            }
        }

        public async Task<IEnumerable<CostCurrentWaterSourceDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostCurrentWaterSourceFilterDTO
            {
                OrganizationId = organizationId,
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.WaterSourceType)
                                    .Select(x => new CostCurrentWaterSourceDTO
                                    {
                                        Id = x.Id,
                                        WaterSourceTypeDisplay = x.WaterSourceType.Title,
                                        WaterSourceTypeId = x.WaterSourceTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActiveSource = x.ActiveSource,
                                        BaseProduction = x.BaseProduction,
                                        LastYearProduction = x.LastYearProduction,
                                        ForcastProduction = x.ForcastProduction
                                    }).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostCurrentWaterSourceImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentWaterSource>>();

            int rowIndex = 1;

            var waterSourceType = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__WaterSourceType).ToList();

            var descendents = await _organizationService
                             .GetAllDescendentsAsync(_userContext.OrganizationId);

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;
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
                if (!waterSourceType.Any(x => x.Id == rec.WaterSourceTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidWaterSourceType, rowIndex + 2, rec.WaterSourceTypeId)
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
            //
            var missingOrgs = new List<Organization>();
            var existOrgs = new List<Organization>();

            foreach (var item in descendents)
            {
                var existInExcel = records.Any(_ => _.OrganizationId == item.Id);
                if (!existInExcel)
                {
                    if (item.Type == Enum.OrganizationType.City || item.Type == Enum.OrganizationType.Village)
                        missingOrgs.Add(item);
                }
                else
                    existOrgs.Add(item);
            }
            //
            //Start WaterSourceType
            var missingwaterType = new List<Constant>();
            string orgTitle = "";
            foreach (var org in existOrgs)
            {
                if (!string.IsNullOrWhiteSpace(orgTitle))
                {
                    break;
                }
                foreach (var item in waterSourceType)
                {
                    var existCCNOTypeInExcel = records.Any(_ => _.WaterSourceTypeId == item.Id &&
                                              _.OrganizationId == org.Id);
                    if (!existCCNOTypeInExcel)
                    {
                        missingwaterType.Add(item);
                        orgTitle = org.Title;
                    }

                }
            }
            if (missingwaterType.Any())
            {
                string waterTypeNames = "";
                foreach (var item in missingwaterType)
                {
                    waterTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelWaterSourceTypeOrgNotInExcels, waterTypeNames, orgTitle));
            }
            //end

            rowIndex = 1;

            if (!continueIfAnyOrgMissing)
            {
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
                    record.WaterSourceTypeId))
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

        #region Private Helper Methods
        private async Task<IQueryable<CostCurrentWaterSource>> setFilter(
            IQueryable<CostCurrentWaterSource> query,
            CostCurrentWaterSourceFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostCurrentWaterSource>();

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

            if (filter.WaterSourceTypeId.HasValue)
                query = query.Where(x => x.WaterSourceTypeId == filter.WaterSourceTypeId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.WaterSourceType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostCurrentWaterSource> setOrder(
           IQueryable<CostCurrentWaterSource> query,
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

                case "costCurrentType":
                    return desc
                        ? query.OrderByDescending(x => x.WaterSourceType.Title)
                        : query.OrderBy(x => x.WaterSourceType.Title);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.WaterSourceType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.WaterSourceType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostCurrentWaterSource>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostCurrentWaterSource>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.WaterSourceTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentWaterSource
                    {
                        WaterSourceTypeId = item.WaterSourceTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActiveSource = item.ActiveSource,
                        BaseProduction = item.BaseProduction,
                        LastYearProduction = item.LastYearProduction,
                        ForcastProduction = item.ForcastProduction
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<CostCurrentWaterSource>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostCurrentWaterSource>();
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
            int organizaionId,
            int waterSourceTypeId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null

                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizaionId &&
                                            x.WaterSourceTypeId == waterSourceTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                               x.OrganizationId == organizaionId &&
                                               x.WaterSourceTypeId == waterSourceTypeId &&
                                               x.Id != id);

            return !result;
        }
        #endregion

    }
}
