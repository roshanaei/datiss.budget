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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class CostCurrentInstallationService : ICostCurrentInstallationService
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

            model.CCInstalationTypeDisplay = (await _constSet.FindAsync(model.CCInstalationTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.CCInstalationTypeId, model.ActivityType))
                {
                    await _dbSet.AddAsync(entity);
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentInstalationDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = entity.Adapt<CostCurrentInstalationDTO>();
                    result.CCInstalationTypeDisplay = model.CCInstalationTypeDisplay;
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
                string.Format(ServiceMessages.Logic_ActivityICOTypeDuplicate,
                model.ActivityType.ToDisplay(), model.CCInstalationTypeDisplay, organizationDisplay)
                );


        }

        public async Task<ValidationResult<CostCurrentInstalationDTO>> UpdateAsync(UpdateCostCurrentInstalationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.CCInstalationTypeDisplay = (await _constSet.FindAsync(model.CCInstalationTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.CCInstalationTypeId, model.ActivityType, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.CCInstalationTypeId = model.CCInstalationTypeId;
                    entity.ActivityType = model.ActivityType;
                    entity.NumberUser = model.NumberUser;
                    entity.Cost = model.Cost;
                    entity.Income = model.Income;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentInstalationDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = new CostCurrentInstalationDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        CCInstalationTypeId = model.CCInstalationTypeId,
                        ActivityType = model.ActivityType,
                        Cost = model.Cost,
                        Income = model.Income,
                        NumberUser = model.NumberUser,

                        OrganizationDisplay = organizationDisplay,
                        CCInstalationTypeDisplay = model.CCInstalationTypeDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<CostCurrentInstalationDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentInstalationDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }
            return ValidationResult<CostCurrentInstalationDTO>.Failed(
                string.Format(ServiceMessages.Logic_ActivityICOTypeDuplicate,
                model.ActivityType.ToDisplay(), model.CCInstalationTypeDisplay, organizationDisplay)
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
                //Key = "IncomeForcastOther_Cal1",
                //Value = await _uow.ExecuteScalar<int>()
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostCurrentInstalationDTO>> GetListAsync(CostCurrentInstalationFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentInstalationDTO>
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
                                    .Include(x => x.CCInstalationType)
                                    .Select(x => new CostCurrentInstalationDTO
                                    {
                                        Id = x.Id,
                                        CCInstalationTypeDisplay = x.CCInstalationType.Title,
                                        CCInstalationTypeId = x.CCInstalationTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityType = x.ActivityType,
                                        NumberUser = x.NumberUser,
                                        Cost = x.Cost,
                                        Income = x.Income,
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
            var result = new List<CostCurrentInstalation>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.CCInstalationTypeId, item.ActivityType))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentInstalation
                    {
                        CCInstalationTypeId = item.CCInstalationTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityType = item.ActivityType,
                        NumberUser = item.NumberUser,
                        Cost = item.Cost,
                        Income = item.Income
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

        public async Task<IEnumerable<CostCurrentInstalationDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostCurrentInstalationFilterDTO
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
                                    .Include(x => x.CCInstalationType)
                                    .Select(x => new CostCurrentInstalationDTO
                                    {
                                        Id = x.Id,
                                        CCInstalationTypeDisplay = x.CCInstalationType.Title,
                                        CCInstalationTypeId = x.CCInstalationTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        ActivityTypeDisplay = x.ActivityType.ToDisplay(),
                                        NumberUser = x.NumberUser,
                                        Cost = x.Cost,
                                        Income = x.Income
                                    }).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostCurrentInstallationImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentInstalation>>();

            int rowIndex = 1;

            var activity = ActivityType.GetValues<ActivityType>();

            var cciwtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__CurrentCostInstalationWater);

            var cciwstypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__CurrentCostInstalationWaste);

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
                if (rec.ActivityType == ActivityType.Water)
                {
                    if (!await cciwtypes.AnyAsync(x => x.Id == rec.CCInstalationTypeId))
                    {
                        return ImportResult.Failed(
                            string.Format(ServiceMessages.ImportExcelInvalidCCIActivityType, rowIndex + 2, rec.ActivityType.ToDisplay(), rec.CCInstalationTypeId)
                            );
                    }
                }
                else
                {
                    if (!await cciwstypes.AnyAsync(x => x.Id == rec.CCInstalationTypeId))
                    {
                        return ImportResult.Failed(
                            string.Format(ServiceMessages.ImportExcelInvalidCCIActivityType, rowIndex + 2, rec.ActivityType.ToDisplay(), rec.CCInstalationTypeId)
                            );
                    }
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
            //Start DWaterType
            var missingCCIWType = new List<Constant>();
            var missingCCIWsType = new List<Constant>();

            string orgTitle = "";
            string orgTitleW = "";
            string orgTitleWs = "";

            string activityTitle = "";
            string activityTitleW = "";
            string activityTitleWs = "";


            foreach (var org in existOrgs)
            {
                foreach (var act in activity)
                {
                    var existActivityInExcel = records.Any(_ => _.ActivityType == act &&
                                                              _.OrganizationId == org.Id);
                    if (!existActivityInExcel)
                    {
                        activityTitle = act.ToDisplay();
                        orgTitle = org.Title;
                    }
                    else
                    {
                        if (act == ActivityType.Water)
                        {
                            foreach (var cciw in cciwtypes)
                            {
                                var exist = records.Any(_ => _.ActivityType == act &&
                                                             _.OrganizationId == org.Id &&
                                                             _.CCInstalationTypeId == cciw.Id);
                                if (!exist)
                                {
                                    missingCCIWType.Add(cciw);
                                    activityTitleW = act.ToDisplay();
                                    orgTitleW = org.Title;
                                }
                            }
                        }
                        else
                        {
                            foreach (var cciws in cciwstypes)
                            {
                                var existWs = records.Any(_ => _.ActivityType == act &&
                                                             _.OrganizationId == org.Id &&
                                                             _.CCInstalationTypeId == cciws.Id);
                                if (!existWs)
                                {
                                    missingCCIWsType.Add(cciws);
                                    activityTitleWs = act.ToDisplay();
                                    orgTitleWs = org.Title;
                                }
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(activityTitle))
            {
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelActivityOrgNotInExcel, activityTitle, orgTitle));
            }

            if (missingCCIWType.Any())
            {
                string cciWTypeNames = "";
                foreach (var item in missingCCIWType)
                {
                    cciWTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelCCITypeActivityOrgNotInExcel, cciWTypeNames, activityTitleW, orgTitleW));
            }

            if (missingCCIWsType.Any())
            {
                string cciWsTypeNames = "";
                foreach (var item in missingCCIWsType)
                {
                    cciWsTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelCCITypeActivityOrgNotInExcel, cciWsTypeNames, activityTitleWs, orgTitleWs));
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
                    record.CCInstalationTypeId,
                    record.ActivityType))
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
                query = query.Where(_ => _.CCInstalationType.Title.ToUpper().Contains(filter.Search));
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
                                .ThenBy(x => x.Organization.RowOrder)
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
                .Where(_ => _.ParentId == parentOrganizationId)
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
