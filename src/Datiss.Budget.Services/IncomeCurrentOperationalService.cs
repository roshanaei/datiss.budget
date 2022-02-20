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
    public class IncomeCurrentOperationalService : IIncomeCurrentOperationalService
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
                CostH = model.CostH,
                CountNH = model.CountNH,
                PriceNH = model.PriceNH,
                CostNH = model.CostNH,
                TotalCount = model.TotalCount,
                TotalCost = model.TotalCost,
            };

            model.ICOTypeDisplay = (await _constSet.FindAsync(model.ICOTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ICOTypeId,model.ActivityType))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentOperationalDTO>();
                    result.ICOTypeDisplay = model.ICOTypeDisplay;
                    result.ActivityType = model.ActivityType;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.CountH = entity.CountH;
                    result.CostH = entity.CostH;
                    result.PriceH = entity.PriceH;
                    result.CountNH = entity.CountNH;
                    result.CostNH = entity.CostNH;
                    result.PriceNH = entity.PriceNH;

                    return ValidationResult<IncomeCurrentOperationalDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentOperationalDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentOperationalDTO>.Failed(
                string.Format(ServiceMessages.Logic_ActivityICOTypeDuplicate,
                model.ActivityType.ToDisplay(), model.ICOTypeDisplay, organizationDisplay)
                );
        }

        public async Task<ValidationResult<IncomeCurrentOperationalDTO>> UpdateAsync(UpdateIncomeCurrentOperationalDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.ICOTypeDisplay = (await _constSet.FindAsync(model.ICOTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ICOTypeId,model.ActivityType, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.ActivityType = model.ActivityType;
                    entity.ICOTypeId = model.ICOTypeId;
                    entity.PriceH = model.PriceH;
                    entity.CountH = model.CountH;
                    entity.CostH = model.CostH;
                    entity.PriceNH = model.PriceNH;
                    entity.CountNH = model.CountNH;
                    entity.CostNH = model.CostNH;
                    entity.TotalCount = model.TotalCount;
                    entity.TotalCost = model.TotalCost;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentOperationalDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        ICOTypeId = model.ICOTypeId,
                        ActivityType = model.ActivityType,
                        CountH = model.CountH,
                        PriceH = model.PriceH,
                        CostH = model.CostH,
                        CountNH = model.CountNH,
                        PriceNH = model.PriceNH,
                        CostNH = model.CostNH,
                        TotalCount = model.TotalCount,
                        TotalCost = model.TotalCost,
                        OrganizationDisplay = organizationDisplay,
                        ICOTypeDisplay = model.ICOTypeDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<IncomeCurrentOperationalDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentOperationalDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentOperationalDTO>.Failed(
                string.Format(ServiceMessages.Logic_ActivityICOTypeDuplicate,
                model.ActivityType.ToDisplay(), model.ICOTypeDisplay, organizationDisplay)
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
                //Key = "",
                //Value = await _uow.ExecuteScalar<int>("[dbo].[Cal1] @YearId, @OrganizationId",parameters: sqlParams.ToArray())
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
                                    .Include(x => x.ICOType)
                                    .Select(x => new IncomeCurrentOperationalDTO
                                    {
                                        Id = x.Id,
                                        ICOTypeDisplay = x.ICOType.Title,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        PriceH = x.PriceH,
                                        CountH = x.CountH,
                                        CostH = x.CostH,
                                        PriceNH = x.PriceNH,
                                        CountNH = x.CountNH,
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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.ICOTypeId, item.ActivityType))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentOperational
                    {
                        ICOTypeId = item.ICOTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityType = item.ActivityType,
                        CountH = item.CountH,
                        PriceH = item.PriceH,
                        CostH = item.CostH,
                        CountNH = item.CountNH,
                        PriceNH = item.PriceNH,
                        CostNH = item.CostNH,
                        TotalCount = item.TotalCount,
                        TotalCost = item.TotalCost                        
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

            var activitytypes = ActivityType.GetValues<ActivityType>();

            var ciowtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__CIOWType);
            
            var ciowstypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__CIOWsType);

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
                if(rec.ActivityType == ActivityType.Water)
                {
                    if (!await ciowtypes.AnyAsync(x => x.Id == rec.ICOTypeId))
                    {
                        return ImportResult.Failed(
                            string.Format(ServiceMessages.ImportExcelInvalidICOTypeActivity, rowIndex + 2,rec.ActivityType.ToDisplay(), rec.ICOTypeId)
                            );
                    }
                }
                else
                {
                    if (!await ciowstypes.AnyAsync(x => x.Id == rec.ICOTypeId))
                    {
                        return ImportResult.Failed(
                            string.Format(ServiceMessages.ImportExcelInvalidICOTypeActivity, rowIndex + 2, rec.ActivityType.ToDisplay(), rec.ICOTypeId)
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
            var missingCIOWType = new List<Constant>();
            var missingCIOWsType = new List<Constant>();
            string missingActicity = "";

            string orgTitle = "";
            string activityTitle = "";

            string missingCIOWTypeTtile = "";
            string missingCIOWsTypeTtile = "";

            string IcoTypeNames = "";


            foreach (var org in existOrgs)
            {
                if (!string.IsNullOrWhiteSpace(orgTitle))
                    break;
                foreach (var activity in activitytypes)
                {
                    if (!string.IsNullOrWhiteSpace(activityTitle))
                        break;
                    var existActivityInExcel = records.Any(_ => _.ActivityType == activity &&
                                                                _.OrganizationId == org.Id);
                    if (!existActivityInExcel)
                    {
                        missingActicity += "- [" + activity.ToDisplay() + "]<br>";
                        orgTitle = org.Title;
                    }
                    else
                    {
                        if (activity == ActivityType.Water)
                        {
                            foreach (var ciow in ciowtypes)
                            {
                                var exist = records.Any(_ => _.ActivityType == activity &&
                                                             _.OrganizationId == org.Id &&
                                                             _.ICOTypeId == ciow.Id);
                                if (!exist)
                                {
                                    missingCIOWType.Add(ciow);
                                    missingCIOWTypeTtile =  ciow.Title;
                                    activityTitle = activity.ToDisplay();
                                    orgTitle = org.Title;
                                }
                            }
                        }
                        else if (activity == ActivityType.Waste)
                        {
                            foreach (var ciows in ciowstypes)
                            {
                                var existWs = records.Any(_ => _.ActivityType == activity &&
                                                               _.OrganizationId == org.Id &&
                                                               _.ICOTypeId == ciows.Id);
                                if (!existWs)
                                {
                                    missingCIOWsType.Add(ciows);
                                    missingCIOWsTypeTtile = ciows.Title;
                                    activityTitle = activity.ToDisplay();
                                    orgTitle = org.Title;
                                }
                            }
                        }
                    }
                }
            }

            if(!string.IsNullOrWhiteSpace(missingActicity))
            {
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelActivityTypeNotInExcel,missingActicity,orgTitle));
            }
            if (missingCIOWType.Any())
            {
                foreach (var missCioW in missingCIOWType)
                {
                    IcoTypeNames += "- [" + missCioW.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelICOTypeActivityOrgNotInExcel, IcoTypeNames, activityTitle, orgTitle));
            }
            if (missingCIOWsType.Any())
            {
                foreach (var missCioWs in missingCIOWsType)
                {
                    IcoTypeNames += "- [" + missCioWs.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelICOTypeActivityOrgNotInExcel, IcoTypeNames, activityTitle, orgTitle));
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
                    record.ICOTypeId,
                    record.ActivityType))
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
                                    .Select(x => new IncomeCurrentOperationalDTO
                                    {
                                        Id = x.Id,
                                        ICOTypeDisplay = x.ICOType.Title,
                                        ICOTypeId = x.ICOTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        CountH = x.CountH,
                                        PriceH = x.PriceH,
                                        CostH = x.CostH,
                                        CountNH = x.CountNH,
                                        PriceNH = x.PriceNH,
                                        CostNH = x.CostNH,
                                        TotalCost = x.TotalCost,
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
                                    .Include(x => x.ICOType)
                                    .Select(x => new IncomeCurrentOperationalDTO
                                    {
                                        Id = x.Id,
                                        ICOTypeDisplay = x.ICOType.Title,
                                        ICOTypeId = x.ICOTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        CountH = x.CountH,
                                        PriceH = x.PriceH,
                                        CostH = x.CostH,
                                        CountNH = x.CountNH,
                                        PriceNH = x.PriceNH,
                                        CostNH = x.CostNH,
                                        TotalCount = x.TotalCount,
                                        TotalCost = x.TotalCost,
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

            if (filter.Type.HasValue)
                query = query.Where(x => x.ActivityType == filter.Type.Value);

            if (filter.ICOTypeId.HasValue)
                query = query.Where(x => x.ICOTypeId == filter.ICOTypeId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         _.ICOType.Title.ToUpper().Contains(filter.Search));
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

                case "icotype":
                    return desc
                        ? query.OrderByDescending(x => x.ICOType.DisplayOrder)
                        : query.OrderBy(x => x.ICOType.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.ICOType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
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
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.ICOTypeId, item.ActivityType))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentOperational
                    {
                        ICOTypeId = item.ICOTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActivityType = item.ActivityType,
                        CountH = item.CountH,
                        PriceH = item.PriceH,
                        CostH = item.CostH,
                        CountNH = item.CountNH,
                        PriceNH = item.PriceNH,
                        CostNH = item.CostNH,
                        TotalCount = item.TotalCount,
                        TotalCost = item.TotalCost,

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
            int IcoTypeId,
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
                                                x.ActivityType == activityType &&
                                                x.ICOTypeId == IcoTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.ActivityType == activityType &&
                                            x.ICOTypeId == IcoTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion

    }
}
