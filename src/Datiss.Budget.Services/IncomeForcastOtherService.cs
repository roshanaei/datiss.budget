using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public class IncomeForcastOtherService : IIncomeForcastOtherService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeForcastOther> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeForcastOtherService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeForcastOther>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeForcastOther> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeForcastOther> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeForcastOtherDTO>> CreateAsync(CreateIncomeForcastOtherDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeForcastOther
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                OIFTypeId = model.OIFTypeId,
                ActivityId = model.ActivityId,
                OIFCount = model.OIFCount,
                OIFPrice = model.OIFPrice
            };

            model.OIFTypeTitle = (await _constSet.FindAsync(model.OIFTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.OIFTypeId, model.ActivityId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeForcastOtherDTO>();
                    result.OIFTypeDisplay = (await _constSet.FindAsync(model.OIFTypeId)).Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.ActivityId = model.ActivityId;
                    result.OIFCount = model.OIFCount;
                    result.OIFPrice = model.OIFPrice;

                    return ValidationResult<IncomeForcastOtherDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeForcastOtherDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeForcastOtherDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,
                model.OIFTypeTitle, organizationDisplay)
                );


        }

        public async Task<ValidationResult<IncomeForcastOtherDTO>> UpdateAsync(UpdateIncomeForcastOtherDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.OIFTypeTitle = (await _constSet.FindAsync(model.OIFTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.OIFTypeId, model.ActivityId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.OIFTypeId = model.OIFTypeId;
                    entity.ActivityId = model.ActivityId;
                    entity.OIFCount = model.OIFCount;
                    entity.OIFPrice = model.OIFPrice;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeForcastOtherDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        OIFTypeId = model.OIFTypeId,
                        ActivityId = model.ActivityId,
                        OIFCount = model.OIFCount,
                        OIFPrice = model.OIFPrice,
                        OrganizationDisplay = organizationDisplay,
                        OIFTypeDisplay = (await _constSet.FindAsync(model.OIFTypeId)).Title,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<IncomeForcastOtherDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeForcastOtherDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }
            return ValidationResult<IncomeForcastOtherDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,
                model.OIFTypeTitle, organizationDisplay)
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
                Key = "IncomeForcastOther_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[IncomeForcastOther_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeForcastOther_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[IncomeForcastOther_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<IncomeForcastOtherDTO>> GetListAsync(IncomeForcastOtherFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<IncomeForcastOtherDTO>
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
                                    .Include(x => x.OIFType)
                                    .Select(x => new IncomeForcastOtherDTO
                                    {
                                        Id = x.Id,
                                        OIFTypeDisplay = x.OIFType.Title,
                                        OIFTypeId = x.OIFTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityId = x.ActivityId,
                                        OIFCount = x.OIFCount,
                                        OIFPrice = x.OIFPrice,
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
            var result = new List<IncomeForcastOther>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.OIFTypeId, item.ActivityId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeForcastOther
                    {
                        OIFTypeId = item.OIFTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityId = item.ActivityId,
                        OIFCount = item.OIFCount,
                        OIFPrice = item.OIFPrice
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
            var data = await _excelService.ImportAsync<IncomeForcastOtherImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<IncomeForcastOther>>();

            var datalist = data.ToList();
            for (int i = 0; i < datalist.Count(); i++)
            {
                if (datalist[i].ActivityId == 0)
                    records[i].ActivityId = ActivityType.Water;
                if (datalist[i].ActivityId == 1)
                    records[i].ActivityId = ActivityType.Waste;
            }

            int rowIndex = 1;

            var oiftypes = _constSet.Where(x => x.Status != EntityStatus.Deleted && 
                                                x.Parent.ConstantKey == ConstantKeys.__OIFType);

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
                if (!await oiftypes.AnyAsync(x => x.Id == rec.OIFTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.OIFTypeId)
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

            //Start OIFType
            var missingType = new List<Constant>();
            string orgTitle = "";
            foreach (var org in existOrgs)
            {
                foreach (var usert in oiftypes)
                {
                    var existTypeInExcel = records.Any(_ => _.OIFTypeId == usert.Id &&
                                              _.OrganizationId == org.Id);
                    if (!existTypeInExcel)
                    {
                        missingType.Add(usert);
                        orgTitle = org.Title;
                    }
                }
            }
            if (missingType.Any())
            {
                string oIFTypeNames = "";
                foreach (var item in missingType)
                {
                    oIFTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelTitleNotInExcel, oIFTypeNames, orgTitle));
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
                    record.OIFTypeId,
                    record.ActivityId))
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

        public async Task<IEnumerable<IncomeForcastOtherDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new IncomeForcastOtherFilterDTO
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
                                    .Include(x => x.OIFType)
                                    .Select(x => new IncomeForcastOtherDTO
                                    {
                                        Id = x.Id,
                                        OIFTypeDisplay = x.OIFType.Title,
                                        OIFTypeId = x.OIFTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityId = x.ActivityId,
                                        OIFCount = x.OIFCount,
                                        OIFPrice = x.OIFPrice
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(IncomeForcastOtherFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.OIFType)
                                    .Select(x => new IncomeForcastOtherDTO
                                    {
                                        Id = x.Id,
                                        OIFTypeDisplay = x.OIFType.Title,
                                        OIFTypeId = x.OIFTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        ActivityId = x.ActivityId,
                                        OIFCount = x.OIFCount,
                                        OIFPrice = x.OIFPrice,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<IncomeForcastOther>> setFilter(
            IQueryable<IncomeForcastOther> query,
            IncomeForcastOtherFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<IncomeForcastOther>();

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

            if (filter.OIFTypeId.HasValue)
                query = query.Where(x => x.OIFTypeId == filter.OIFTypeId.Value);

            if (filter.Type.HasValue)
                query = query.Where(x => x.ActivityId == filter.Type.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.OIFType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<IncomeForcastOther> setOrder(
           IQueryable<IncomeForcastOther> query,
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

                case "oiftype":
                    return desc
                        ? query.OrderByDescending(x => x.OIFType.DisplayOrder)
                        : query.OrderBy(x => x.OIFType.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.OIFType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.ActivityId)
                                .ThenBy(x => x.OIFType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<IncomeForcastOther>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<IncomeForcastOther>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.OIFTypeId, item.ActivityId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeForcastOther
                    {
                        OIFTypeId = item.OIFTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActivityId = item.ActivityId,
                        OIFCount = item.OIFCount,
                        OIFPrice = item.OIFPrice
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<IncomeForcastOther>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted && 
                            _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<IncomeForcastOther>();
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
                                                x.OIFTypeId == oIFTypeId &&
                                                x.ActivityId == activityType)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.OIFTypeId == oIFTypeId &&
                                            x.ActivityId == activityType &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
