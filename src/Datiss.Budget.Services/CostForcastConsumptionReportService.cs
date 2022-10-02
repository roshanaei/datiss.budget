using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Datiss.Budget.Services
{
    public class CostForcastConsumptionReportService : ICostForcastConsumptionReportService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostForcastConsumptionReport> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastConsumptionReportService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastConsumptionReport>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostForcastConsumptionReport> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastConsumptionReport> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastConsumptionReportDTO>> UpdateAsync(UpdateCostForcastConsumptionReportDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.SectionTypeTitle = (await _constSet.FindAsync(model.SectionTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.SectionTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.SectionTypeId = model.SectionTypeId;
                    entity.FunctionalBasicYear = model.FunctionalBasicYear;
                    entity.FunctionalYear_1 = model.FunctionalYear_1;
                    entity.ForcastY = model.ForcastY;
                    entity.ApproveYear_1 = model.ApproveYear_1;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostForcastConsumptionReportDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<CostForcastConsumptionReportDTO>();

                    result.OrganizationDisplay = organizationDisplay;
                    result.SectionTypeDisplay = model.SectionTypeTitle;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                                  
                    return ValidationResult<CostForcastConsumptionReportDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastConsumptionReportDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostForcastConsumptionReportDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,
                model.SectionTypeTitle,organizationDisplay)
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

            var self = await _dbSet.Where(x => x.YearId == yearId)
                                   .Where(x => x.OrganizationId == organizationId)
                                   .ToListAsync();

            IEnumerable<CostForcastConsumptionReport> childrens = new CostForcastConsumptionReport[] { };

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

        public async Task<PagedResult<CostForcastConsumptionReportDTO>> GetListAsync(CostForcastConsumptionReportFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastConsumptionReportDTO>
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
                                        .Include(x => x.SectionType)
                                        .Select(x => x.Adapt<CostForcastConsumptionReportDTO>())
                                        .ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task<ValidationResult> CalculationAsync(int yearId, int organizationId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            await _uow.ExecuteScalar<ValidationResult>(
                                    "[dbo].[CostForcastConsumptionReport_Insert] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray());


            return ValidationResult.Success();
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopySameYearException();
            ///////////////////////////////////////////////////////////////check
            if (!await hasAnyDataAsync(sourceOrgId, sourceYearId))
                throw new CopyOrgNullDataException();

            var result = new List<CostForcastConsumptionReport>();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    item.Id = 0;
                    item.YearId = destYearId;
                    var entity = item.Adapt<CostForcastConsumptionReport>();
                    
                    result.Add(entity);
                }
            }

            var childrens = await getChildrenData(sourceOrgId, sourceYearId, destYearId);

            if (childrens.Any())
            {
                result.AddRange(childrens);
            }

            foreach (var record in result)
            {
                var entity = await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.SectionTypeId);

                if (entity == null)
                {
                    await _dbSet.AddAsync(record);
                }
                else
                {
                    entity.ApproveYear_1 = record.ApproveYear_1;
                    _dbSet.Update(entity);
                }
            }

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                throw new CopyDataBaseException();
            }
        }

        public async Task<IEnumerable<CostForcastConsumptionReportDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostForcastConsumptionReportFilterDTO
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
                                    .Include(x => x.SectionType)
                                    .Select(x => x.Adapt<CostForcastConsumptionReportDTO>())
                                    .ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostForcastConsumptionReportImportModel>
               (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostForcastConsumptionReport>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var sectionTypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CFConsumptionReportType &&
                                                    x.Parent.ParentId == null &&
                                                    x.Status != EntityStatus.Deleted);

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
                if (!await sectionTypes.AnyAsync(x => x.Id == rec.SectionTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUserType, rowIndex + 2, rec.SectionTypeId)
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

            #region CheckType

            var missingSectionType = new List<Constant>();
            string orgTitle = "";
            foreach (var org in existOrgs)
            {
                if (!string.IsNullOrWhiteSpace(orgTitle))
                {
                    break;
                }
                foreach (var item in sectionTypes)
                {
                    var existDWTypeInExcel = records.Any(_ => _.SectionTypeId == item.Id &&
                                                              _.OrganizationId == org.Id);
                    if (!existDWTypeInExcel)
                    {
                        missingSectionType.Add(item);
                        orgTitle = org.Title;
                    }

                }
            }
            if (missingSectionType.Any())
            {
                string sectionTypeNames = "";
                foreach (var item in missingSectionType)
                {
                    sectionTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelTitleNotInExcel, sectionTypeNames, orgTitle));
            }

            #endregion

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

                rowIndex++;
            }

            foreach (var record in records)
            {

                var entity = await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.SectionTypeId);
                if (entity == null)
                {
                    await _dbSet.AddAsync(record);
                }
                else
                {
                    entity.FunctionalBasicYear = record.FunctionalBasicYear;
                    entity.FunctionalYear_1 = record.FunctionalYear_1;
                    _dbSet.Update(entity);
                }

            }


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


        #region Privte Helper Methods
        private async Task<IQueryable<CostForcastConsumptionReport>> setFilter(
            IQueryable<CostForcastConsumptionReport> query,
            CostForcastConsumptionReportFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastConsumptionReport>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue)
            {
                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);

                foreach (var org in organizations)
                {
                    predicate.Or(x => x.OrganizationId == org.Id);
                }

                query = query.Where(predicate);
            }

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(x => x.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         x.SectionType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostForcastConsumptionReport> setOrder(
            IQueryable<CostForcastConsumptionReport> query,
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


                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.SectionType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.SectionType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostForcastConsumptionReport>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostForcastConsumptionReport>();
            foreach (var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .ToListAsync();

                foreach (var item in data)
                {
                    result.Add(item);
                }
                result.AddRange(await getChildren(org.Id, yearId));
            }
            return result;
        }

        private async Task<IEnumerable<CostForcastConsumptionReport>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(x => x.Status != EntityStatus.Deleted &&
                            x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostForcastConsumptionReport>();

            foreach (var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .ToListAsync();

                foreach (var item in data)
                {
                    item.Id = 0;
                    item.YearId = targetYearId;
                    var entity = item.Adapt<CostForcastConsumptionReport>();

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
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
                    if (await Query().AnyAsync(x => x.YearId == yearid &&
                                                    x.OrganizationId == child.Id))
                        return true;
            }

            return false;
        }

        #endregion

        #region Logics
        private async Task<bool> checkLogicAsync(
             int yearId,
             int organizationId,
             int sectionTypeId,
             int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                   ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                   x.OrganizationId == organizationId &&
                                                   x.SectionTypeId == sectionTypeId)

                   : await Query().AnyAsync(x => x.YearId == yearId &&
                                                 x.OrganizationId == organizationId &&
                                                 x.SectionTypeId == sectionTypeId &&
                                                 x.Id != id);

            return !result;
        }
        private async Task<CostForcastConsumptionReport> checkLogicAsync(
             int yearId,
             int organizationId,
             int sectionTypeId)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(x => x.YearId == yearId &&
                                                             x.OrganizationId == organizationId &&
                                                             x.SectionTypeId == sectionTypeId);
            if (entity == null)
            {
                return null;
            }
            return entity;
        }

        #endregion
    }
}
