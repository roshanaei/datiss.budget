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
using Datiss.Budget.Enum;
using Datiss.Budget.Common;

namespace Datiss.Budget.Services
{
    public class IncomeCurrentOperationalService 
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentOperational> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeCurrentOperationalService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentOperational>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentOperational> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentOperational> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeCurrentOperationalDTO>> CreateAsync(CreateIncomeCurrentOperationalDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeCurrentOperational
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                ActivityType = model.ActivityType,
                ICOTypeId = model.ICOTypeId,
                CountH = model.CountH,
                PriceH = model.PriceH,
                CountNH = model.CountNH,
                PriceNH = model.PriceNH
            };

            model.ICOTypeTitle = (await _constSet.FindAsync(model.ICOTypeId)).Title;

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId ,model.ActivityType, model.ICOTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentOperationalDTO>();
                    result.ICOTypeTitle = model.ICOTypeTitle;
                    result.ActivityType = model.ActivityType;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.CountH = entity.CountH;
                    result.PriceH = entity.PriceH;
                    result.CountNH = entity.CountNH;
                    result.PriceNH = entity.PriceNH;

                    return ValidationResult<IncomeCurrentOperationalDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentOperationalDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentOperationalDTO>.Failed(
                string.Format(ServiceMessages.Logic_ActivityOrgDuplicate,
                                                    model.ActivityType
                                                    ,organizationDisplay)
                );

        }

        public async Task<ValidationResult<IncomeCurrentOperationalDTO>> UpdateAsync(UpdateIncomeCurrentOperationalDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.ICOTypeTitle = (await _constSet.FindAsync(model.ICOTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId,model.ActivityType, model.ICOTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.ActivityType = model.ActivityType;
                    entity.ICOTypeId = model.ICOTypeId;
                    entity.CountH = model.CountH;
                    entity.PriceH = model.PriceH;
                    entity.CountNH = model.PriceNH;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentOperationalDTO
                    {
                        OrganizationId = model.OrganizationId,
                        OrganizationDisplay = organizationDisplay,
                        YearId = model.YearId,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        ActivityType = model.ActivityType,
                        ICOTypeId = model.ICOTypeId,
                        ICOTypeTitle = model.ICOTypeTitle,
                        CountH = model.CountH,
                        PriceH = model.PriceH,
                        CountNH = model.CountNH,
                        PriceNH = model.PriceNH
                    };

                    return ValidationResult<IncomeCurrentOperationalDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentOperationalDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentOperationalDTO>.Failed(
                string.Format(ServiceMessages.Logic_ActivityTitleOrgDuplicate
                                            , model.ActivityType
                                            ,model.ICOTypeTitle
                                            ,organizationDisplay)
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
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };
            var result = new List<CalculationItemData>();
            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentOperational_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentOperational_Cal1] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentOperational_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                       "[dbo].[IncomeCurrentOperational_Cal2] @YearId, @OrganizationId",
                       parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<IncomeCurrentOperationalDTO>> GetListAsync(IncomeCurrentOperationalFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<IncomeCurrentOperationalDTO>
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
                                    .Include(x => x.ActivityType)
                                    .Include(x => x.ICOType)
                                    .Select(x => new IncomeCurrentOperationalDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityType = x.ActivityType,
                                        ICOTypeId = x.ICOTypeId,
                                        ICOTypeTitle = x.ICOType.Title,
                                        CountH = x.CountH,
                                        PriceH = x.PriceH,
                                        CostH = x.CostH,
                                        CountNH = x.CountNH,
                                        PriceNH = x.PriceNH,
                                        CostNH = x.CostNH,
                                        TotalCount = x.TotalCount,
                                        TotalCost = x.TotalCost
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
            var result = new List<IncomeCurrentOperational>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.ActivityType, item.ICOTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentOperational
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityType = item.ActivityType,
                        ICOTypeId = item.ICOTypeId,
                        CountH = item.CountH,
                        PriceH = item.PriceH,
                        CountNH = item.CountNH,
                        PriceNH = item.PriceNH
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

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<IncomeCurrentOperationalImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<IncomeCurrentOperational>>();

            int rowIndex = 1;

            var ciotypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CIOWType);
            var ciowstypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CIOWsType);



            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;
                var org = await _orgDbSet.FindAsync(rec.OrganizationId);

                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex + 1, rec.YearId)
                        );
                }
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rowIndex + 1, rec.OrganizationId)
                        );
                }
                if (!await ciotypes.AnyAsync(x => x.Id == rec.ICOTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelTitleNotInExcel, rowIndex + 1, rec.ICOTypeId)
                        );
                }
                if (!await ciowstypes.AnyAsync(x => x.Id == rec.ICOTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelTitleNotInExcel, rowIndex + 1, rec.ICOTypeId)
                        );
                }
                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex + 1)
                        );
                }

                rowIndex++;
            }

            //Start ICOType
            var missingDWType = new List<Constant>();
            foreach (var item in ciotypes)
            {
                var existDWTypeInExcel = records.Any(_ => _.ICOTypeId == item.Id);
                if (!existDWTypeInExcel)
                    missingDWType.Add(item);

            }
            if (missingDWType.Any())
            {
                string dWaterTypeNames = "";
                foreach (var item in missingDWType)
                {
                    dWaterTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelTitleNotInExcel, dWaterTypeNames));
            }
            //end

            rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

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
                        string.Format(ServiceMessages.ImportExcelAccessError, rowIndex + 1)
                        );

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.ActivityType,
                    record.ICOTypeId))
                {

                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelLogicError, rowIndex + 1)
                        );
                }

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();

            return ImportResult.Succeed(
                string.Format(ServiceMessages.ImportExcelSuccess)
                );
        }

        public async Task<IEnumerable<IncomeCurrentOperationalDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new IncomeCurrentOperationalFilterDTO
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
                                    .Include(x => x.ActivityType)
                                    .Include(x => x.ICOType)
                                    .Select(x => new IncomeCurrentOperationalDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        ICOTypeTitle = x.ICOType.Title,
                                        ICOTypeId = x.ICOTypeId,
                                        CountH = x.CountH,
                                        PriceH = x.PriceH,
                                        CostH = x.CostH,
                                        CountNH = x.CountNH,
                                        PriceNH = x.PriceNH,
                                        CostNH = x.CostNH,
                                        TotalCount = x.CountNH,
                                        TotalCost = x.TotalCost                                                                                                
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(IncomeCurrentOperationalFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.ActivityType)
                                    .Include(x => x.ICOType)
                                    .Select(x => new IncomeCurrentOperationalDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityType = x.ActivityType,
                                        ICOTypeTitle = x.ICOType.Title,
                                        ICOTypeId = x.ICOTypeId,
                                        CountH = x.CountH,
                                        PriceH = x.PriceH,
                                        CostH = x.CostH,
                                        CountNH = x.CountNH,
                                        PriceNH = x.PriceNH,
                                        CostNH = x.CostNH,
                                        TotalCount = x.TotalCount,
                                        TotalCost = x.TotalCost
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Private Helper Methods
        private async Task<IQueryable<IncomeCurrentOperational>> setFilter(
            IQueryable<IncomeCurrentOperational> query,
            IncomeCurrentOperationalFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<IncomeCurrentOperational>();

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

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(x => x.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         x.ICOType.Title.ToUpper().Contains(filter.Search)  ||
                                         x.ActivityType.ToString().ToUpper().Contains(filter.Search) ||
                                         x.CountH.ToString().Contains(filter.Search) ||
                                         x.PriceH.ToString().Contains(filter.Search) ||
                                         x.CostH.ToString().Contains(filter.Search) ||
                                         x.CountNH.ToString().Contains(filter.Search) ||
                                         x.PriceNH.ToString().Contains(filter.Search) ||
                                         x.CostNH.ToString().Contains(filter.Search) ||
                                         x.TotalCost.ToString().Contains(filter.Search) ||
                                         x.TotalCount.ToString().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<IncomeCurrentOperational> setOrder(
           IQueryable<IncomeCurrentOperational> query,
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

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.ActivityType)
                                .Include(x => x.ICOType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.ActivityType)
                                .ThenBy(x => x.ICOType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<IncomeCurrentOperational>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<IncomeCurrentOperational>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.ActivityType, item.ICOTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentOperational
                    {
                        ICOTypeId = item.ICOTypeId,
                        ActivityType = item.ActivityType,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        CountH = item.CountH,
                        PriceH = item.PriceH,
                        CountNH = item.CountNH,
                        PriceNH = item.PriceNH
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<IncomeCurrentOperational>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<IncomeCurrentOperational>();
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
            ActivityType activityType,
            int icoTypeId,
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
                                              x.ICOTypeId == icoTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.ActivityType == activityType &&
                                            x.ICOTypeId == icoTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
