using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Resources;
using Microsoft.Data.SqlClient;
using Mapster;
using Datiss.Budget.Common;
using Datiss.Budget.Extensions;
using Datiss.Budget.Services.Excel.Models;

namespace Datiss.Budget.Services
{
    public class CostCurrentPMDepService : ICostCurrentPMDepService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostCurrentPMDep> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostCurrentPMDepService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentPMDep>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentPMDep> Query()
              => _dbSet.AsNoTracking();

        public async Task<CostCurrentPMDep> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentPMDepDTO>> UpdateAsync(UpdateCostCurrentPMDepDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.CCPMDepTypeTitle = (await _constSet.FindAsync(model.CCPMDepTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.CCPMDepTypeId, model.ActivityType, model.RecordType, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.CCPMDepTypeId = model.CCPMDepTypeId;
                    entity.ActivityType = model.ActivityType;
                    entity.RecordType = model.RecordType;
                    entity.CostCenter = model.CostCenter;
                    entity.FinancePMCost = model.FinancePMCost;
                    entity.RFinancePMCost_D = model.RFinancePMCost_D;
                    entity.FinanceDepCost = model.FinanceDepCost;
                    entity.RFinanceDepCost_D = model.RFinanceDepCost_D;


                    await _uow.SaveChangesAsync();

                    var result = new CostCurrentPMDepDTO
                    {
                        OrganizationId = model.OrganizationId,
                        OrganizationDisplay = organizationDisplay,
                        YearId = model.YearId,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        CCPMDepTypeId = model.CCPMDepTypeId,
                        CCPMDepTypeDisplay = model.CCPMDepTypeTitle,
                        CostCenter = model.CostCenter,
                        FinancePMCost = model.FinancePMCost,
                        RFinancePMCost_D = model.RFinancePMCost_D,
                        FinanceDepCost = model.FinanceDepCost,
                        RFinanceDepCost_D = model.RFinanceDepCost_D
                    };

                    return ValidationResult<CostCurrentPMDepDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentPMDepDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentPMDepDTO>.Failed(
               string.Format(ServiceMessages.Logic_NOICOrgDuplicate,
                                model.CCPMDepTypeTitle, organizationDisplay)
               );
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

        public async Task<ValidationResult> CalculationAsync(int yearId, int organizationId)
        {
            var result = new List<CalculationItemData>();
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };
            return ValidationResult.Success();
        }

        public async Task<PagedResult<CostCurrentPMDepDTO>> GetListAsync(CostCurrentPMDepFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentPMDepDTO>
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
                                    .Include(x => x.CCPMDepType)
                                    .Select(x => new CostCurrentPMDepDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        CCPMDepTypeDisplay = x.CCPMDepType.Title,
                                        CCPMDepTypeId = x.CCPMDepTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityType = x.ActivityType,
                                        RecordType = x.RecordType,
                                        CostCenter = x.CostCenter,
                                        FinancePMCost = x.FinancePMCost,
                                        RFinancePMCost_D = x.RFinancePMCost_D,
                                        FinanceDepCost = x.FinanceDepCost,
                                        RFinanceDepCost_D = x.RFinanceDepCost_D
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
            var result = new List<CostCurrentPMDep>();

            if (await Query()
                        .Where(_ => _.OrganizationId == sourceOrgId)
                        .Where(_ => _.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .Where(_=>_.RecordType == RecordType.Base)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.CCPMDepTypeId, item.ActivityType, item.RecordType))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentPMDep
                    {
                        CCPMDepTypeId = item.CCPMDepTypeId,
                        ActivityType = item.ActivityType,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        RecordType = item.RecordType,
                        CostCenter = item.CostCenter,
                        FinancePMCost = item.FinancePMCost,
                        RFinancePMCost_D = item.RFinancePMCost_D,
                        FinanceDepCost = item.FinanceDepCost,
                        RFinanceDepCost_D = item.RFinanceDepCost_D
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

        public async Task<Stream> ExportExcelAsync(CostCurrentPMDepFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.CCPMDepType)
                                    .Select(x => new CostCurrentPMDepDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        CCPMDepTypeDisplay = x.CCPMDepType.Title,
                                        CCPMDepTypeId = x.CCPMDepTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityType = x.ActivityType,
                                        RecordType = x.RecordType,
                                        CostCenter = x.CostCenter,
                                        FinancePMCost = x.FinancePMCost,
                                        RFinancePMCost_D = x.RFinancePMCost_D,
                                        FinanceDepCost = x.FinanceDepCost,
                                        RFinanceDepCost_D = x.RFinanceDepCost_D

                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        public async Task<IEnumerable<CostCurrentPMDepDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostCurrentPMDepFilterDTO
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
                                    .Include(x => x.CCPMDepType)
                                    .Select(x => new CostCurrentPMDepDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        CCPMDepTypeDisplay = x.CCPMDepType.Title,
                                        CCPMDepTypeId = x.CCPMDepTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityType = x.ActivityType,
                                        RecordType = x.RecordType,
                                        CostCenter = x.CostCenter,
                                        FinancePMCost = x.FinancePMCost,
                                        RFinancePMCost_D = x.RFinancePMCost_D,
                                        FinanceDepCost = x.FinanceDepCost,
                                        RFinanceDepCost_D = x.RFinanceDepCost_D

                                    }).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostCurrentPMDepImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentPMDep>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var ccPMDepType = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CCPMDepType &&
                                                   x.Status == EntityStatus.Enabled);

            var activityType = EnumSelectListProvider.GetActivityTypeItems().ToList();


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
                if (!await ccPMDepType.AnyAsync(x => x.Id == rec.CCPMDepTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUserType, rowIndex + 2, rec.CCPMDepTypeId)
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

            //Start Missing Type
            var missingccPMDepType = new List<Constant>();
            string missingActivity = "";

            string orgTitle = "";
            string activityTitle = "";

            foreach (var org in existOrgs)
            {
                if (!string.IsNullOrWhiteSpace(orgTitle))
                {
                    break;
                }
                foreach (var act in activityType)
                {
                    var activity = act.Value == "0" ? ActivityType.Water : ActivityType.Waste;
                    if (!records.Any(_ => _.ActivityType == activity &&
                                       _.OrganizationId == org.Id))
                    {
                        missingActivity += "- [" + act.Text + "]<br>";
                        orgTitle = org.Title;
                    }
                    else
                    {
                        foreach (var cc in ccPMDepType)
                        {
                            var exist = records.Any(_ => _.OrganizationId == org.Id &&
                                                         _.ActivityType == activity &&
                                                         _.CCPMDepTypeId == cc.Id);
                            if (!exist)
                            {
                                missingccPMDepType.Add(cc);
                                activityTitle = act.Text;
                                orgTitle = org.Title;
                            }
                        }

                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(missingActivity))
            {
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUserTypeOrgNotInExcel, missingActivity, orgTitle));
            }

            if (missingccPMDepType.Any())
            {
                string ccTypeNames = "";
                foreach (var item in missingccPMDepType)
                {
                    ccTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelDiameterPipeUserTypeOrgNotInExcel, ccTypeNames, activityTitle, orgTitle));
            }
            //End

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
                    record.CCPMDepTypeId,
                    record.ActivityType,
                    record.RecordType))
                {

                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelLogicError, rowIndex + 2)
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

        #region Private Helper Methods

        private IQueryable<CostCurrentPMDep> setOrder(
            IQueryable<CostCurrentPMDep> query,
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

                case "ccPMDeptype":
                    return desc
                        ? query.OrderByDescending(x => x.CCPMDepType.DisplayOrder)
                        : query.OrderBy(x => x.CCPMDepType.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.CCPMDepType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.CCPMDepType.DisplayOrder);
            }
        }
        private async Task<IQueryable<CostCurrentPMDep>> setFilter(
            IQueryable<CostCurrentPMDep> query,
            CostCurrentPMDepFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = LinqKit.PredicateBuilder.New<CostCurrentPMDep>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.RecordType.HasValue)
                query = query.Where(x=>x.RecordType == filter.RecordType.Value);

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
            if (filter.CCPMDepTypeId.HasValue)
                query = query.Where(x => x.CCPMDepTypeId == filter.CCPMDepTypeId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         _.CCPMDepType.Title.ToUpper().Contains(filter.Search));
            }
            return query;
        }
        private async Task<IEnumerable<CostCurrentPMDep>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId &&
                            _.Status != EntityStatus.Deleted)
                .ToListAsync();
            var result = new List<CostCurrentPMDep>();
            foreach (var org in children)
            {
                if (await Query()
                            .Where(_ => _.OrganizationId == org.Id)
                            .Where(_ => _.YearId == targetYearId)
                            .Where(_ => _.RecordType == RecordType.Base).AnyAsync())
                {
                    throw new CopyDestYearHasDataException();
                }

                var data = await Query()
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .Where(_ => _.RecordType == RecordType.Base)
                                .ToListAsync();

                foreach (var item in data)
                {
                    var entity = new CostCurrentPMDep
                    {
                        CCPMDepTypeId = item.CCPMDepTypeId,
                        ActivityType = item.ActivityType,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        RecordType = item.RecordType,
                        CostCenter = item.CostCenter,
                        FinancePMCost = item.FinancePMCost,
                        RFinancePMCost_D = item.RFinancePMCost_D,
                        FinanceDepCost = item.FinanceDepCost,
                        RFinanceDepCost_D = item.RFinanceDepCost_D
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<CostCurrentPMDep>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId &&
                            _.Status != EntityStatus.Deleted)
                .ToListAsync();
            var result = new List<CostCurrentPMDep>();
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
            int ccPMDepTypeId,
            ActivityType activity,
            RecordType recordType,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.CCPMDepTypeId == ccPMDepTypeId &&
                                                x.ActivityType == activity &&
                                                x.RecordType == recordType)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.CCPMDepTypeId == ccPMDepTypeId &&
                                            x.ActivityType == activity &&
                                            x.RecordType == recordType &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
