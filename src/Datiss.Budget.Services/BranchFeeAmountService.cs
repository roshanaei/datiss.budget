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
    public class BranchFeeAmountService : IBranchFeeAmountService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<BranchFeeAmount> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;

        public BranchFeeAmountService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<BranchFeeAmount>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<BranchFeeAmount> Query()
            => _dbSet.AsNoTracking();

        public async Task<BranchFeeAmount> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<BranchFeeAmountDTO>> CreateAsync(CreateBranchFeeAmountDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new BranchFeeAmount
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UrbanAdjustmentFactor = model.UrbanAdjustmentFactor,
                WasteRateInWater = model.WasteRateInWater,
                WaterBranchingPerHousing = model.WaterBranchingPerHousing,
                TubingCost = model.TubingCost,
                WaterPartnershipAmountDomestic = model.WaterPartnershipAmountDomestic,
                WaterPartnershipAmountNDomestic = model.WaterPartnershipAmountNDomestic,
                WastePartnershipAmountDomestic = model.WastePartnershipAmountDomestic,
                WastePartnershipAmountNDomestic = model.WastePartnershipAmountNDomestic,
                FixCostNote11H = model.FixCostNote11H,
                FixCostNote11NH = model.FixCostNote11NH,
                FixCostNote11HWs = model.FixCostNote11HWs,
                FixCostNote11NHWs = model.FixCostNote11NHWs,
                WsTubingCost = model.WsTubingCost
            };

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<BranchFeeAmountDTO>();
                    result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.UrbanAdjustmentFactor = entity.UrbanAdjustmentFactor;
                    result.WasteRateInWater = entity.WasteRateInWater;
                    result.WaterBranchingPerHousing = entity.WaterBranchingPerHousing;
                    result.TubingCost = entity.TubingCost;
                    result.WaterPartnershipAmountDomestic = entity.WaterPartnershipAmountDomestic;
                    result.WaterPartnershipAmountNDomestic = entity.WaterPartnershipAmountNDomestic;
                    result.WastePartnershipAmountDomestic = entity.WastePartnershipAmountDomestic;
                    result.WastePartnershipAmountNDomestic = entity.WastePartnershipAmountNDomestic;
                    result.FixCostNote11H = entity.FixCostNote11H;
                    result.FixCostNote11NH = entity.FixCostNote11NH;
                    result.FixCostNote11HWs = entity.FixCostNote11HWs;
                    result.FixCostNote11NHWs = entity.FixCostNote11NHWs;
                    result.WsTubingCost = entity.WsTubingCost;

                    return ValidationResult<BranchFeeAmountDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<BranchFeeAmountDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<BranchFeeAmountDTO>.Failed(
                  string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                  model.YearId, model.OrganizationId)
                  );
            }

        public async Task<ValidationResult<BranchFeeAmountDTO>> UpdateAsync(UpdateBranchFeeAmountDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId,model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UrbanAdjustmentFactor = model.UrbanAdjustmentFactor;
                    entity.WasteRateInWater = model.WasteRateInWater;
                    entity.WaterBranchingPerHousing = model.WaterBranchingPerHousing;
                    entity.TubingCost = model.TubingCost;
                    entity.WaterPartnershipAmountDomestic = model.WaterPartnershipAmountDomestic;
                    entity.WaterPartnershipAmountNDomestic = model.WaterPartnershipAmountNDomestic;
                    entity.WastePartnershipAmountDomestic = model.WastePartnershipAmountDomestic;
                    entity.WastePartnershipAmountNDomestic = model.WastePartnershipAmountNDomestic;
                    entity.FixCostNote11H = model.FixCostNote11H;
                    entity.FixCostNote11NH = model.FixCostNote11NH;
                    entity.FixCostNote11HWs = model.FixCostNote11HWs;
                    entity.FixCostNote11NHWs = model.FixCostNote11NHWs;
                    entity.WsTubingCost = model.WsTubingCost;

                    await _uow.SaveChangesAsync();

                    var result = new BranchFeeAmountDTO
                    {
                        OrganizationId=model.OrganizationId,
                        YearId=model.YearId,
                        UrbanAdjustmentFactor = model.UrbanAdjustmentFactor,
                        WasteRateInWater = model.WasteRateInWater,
                        WaterBranchingPerHousing = model.WaterBranchingPerHousing,
                        TubingCost = model.TubingCost,
                        WaterPartnershipAmountDomestic = model.WaterPartnershipAmountDomestic,
                        WaterPartnershipAmountNDomestic = model.WaterPartnershipAmountNDomestic,
                        WastePartnershipAmountDomestic = model.WastePartnershipAmountDomestic,
                        WastePartnershipAmountNDomestic = model.WastePartnershipAmountNDomestic,
                        FixCostNote11H = model.FixCostNote11H,
                        FixCostNote11NH = model.FixCostNote11NH,
                        FixCostNote11HWs = model.FixCostNote11HWs,
                        FixCostNote11NHWs = model.FixCostNote11NHWs,
                        WsTubingCost = model.WsTubingCost
                    };

                    return ValidationResult<BranchFeeAmountDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<BranchFeeAmountDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<BranchFeeAmountDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                model.YearId, 
                model.OrganizationId)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckReferenceIsNull(nameof(entity));

            var year = await _yearSet.FindAsync(entity.YearId);
            year.CheckReferenceIsNull(nameof(entity));

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

            var self = await _dbSet.Where(x => x.YearId == yearId)
                                    .Where(x => x.OrganizationId == organizationId)
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
                Key = "BranchFeeAmount_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[BranchFeeAmount_Cal1] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<BranchFeeAmountDTO>> GetListAsync(BranchFeeAmountFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<BranchFeeAmountDTO>
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
            
            result.Items = await query
                            .Include(x => x.FinanceYear)
                            .Include(x => x.Organization)
                            .Select(x => new BranchFeeAmountDTO {
                                Id=x.Id,
                                Year = x.FinanceYear.Year,
                                YearId = x.YearId,
                                OrganizationId = x.OrganizationId,
                                OrganizationDisplay = x.Organization.Title,
                                UrbanAdjustmentFactor = x.UrbanAdjustmentFactor,
                                WasteRateInWater = x.WasteRateInWater,
                                WaterBranchingPerHousing = x.WaterBranchingPerHousing,
                                TubingCost = x.TubingCost,
                                WaterPartnershipAmountDomestic = x.WaterPartnershipAmountDomestic,
                                WaterPartnershipAmountNDomestic = x.WaterPartnershipAmountNDomestic,
                                WastePartnershipAmountDomestic = x.WastePartnershipAmountDomestic,
                                WastePartnershipAmountNDomestic = x.WastePartnershipAmountNDomestic,
                                FixCostNote11H = x.FixCostNote11H,
                                FixCostNote11NH = x.FixCostNote11NH,
                                FixCostNote11HWs = x.FixCostNote11HWs,
                                FixCostNote11NHWs = x.FixCostNote11NHWs,
                                WsTubingCost = x.WsTubingCost
                            }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();

            if (destYearId < sourceYearId)
                throw new CopySameYearException();

            if (!await hasAnyDataAsync(sourceOrgId, sourceYearId))
                throw new CopyOrgNullDataException();

            var result = new List<BranchFeeAmount>();

            if (await Query()
                        .Where(x => x.OrganizationId == sourceOrgId)
                        .Where(x => x.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();
            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, sourceOrgId))
                        throw new CopyDestYearHasDataException();

                    var entity = new BranchFeeAmount
                    {
                        YearId = item.YearId,
                        OrganizationId = item.OrganizationId,
                        UrbanAdjustmentFactor = item.UrbanAdjustmentFactor,
                        WasteRateInWater = item.WasteRateInWater,
                        WaterBranchingPerHousing = item.WaterBranchingPerHousing,
                        TubingCost = item.TubingCost,
                        WaterPartnershipAmountDomestic = item.WaterPartnershipAmountDomestic,
                        WaterPartnershipAmountNDomestic = item.WaterPartnershipAmountNDomestic,
                        WastePartnershipAmountDomestic = item.WastePartnershipAmountDomestic,
                        WastePartnershipAmountNDomestic = item.WastePartnershipAmountNDomestic,
                        FixCostNote11H = item.FixCostNote11H,
                        FixCostNote11NH = item.FixCostNote11NH,
                        FixCostNote11HWs = item.FixCostNote11HWs,
                        FixCostNote11NHWs = item.FixCostNote11NHWs,
                        WsTubingCost = item.WsTubingCost
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

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo,int yearId,bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<BranchFeeAmountImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<BranchFeeAmount>>();

            int rowIndex = 1;

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;

                var org = await _orgDbSet.FindAsync(rec.OrganizationId);

                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear,rowIndex + 1,rec.YearId)
                        );
                }
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg,rowIndex + 1,rec.OrganizationId)
                        );
                }
                if(org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex +1 )
                        );
                }

                rowIndex++;
            }
            rowIndex = 1;

            var descendents = await _organizationService
                   .GetAllDescendentsAsync(_userContext.OrganizationId);
            if (!continueIfAnyOrgMissing)
            {
                var missingOrgs = new List<Organization>();

                foreach( var item in descendents)
                {
                    var existInExcel = records.Any(x => x.OrganizationId == item.Id);
                    if (!existInExcel)
                       
                        if (item.Type == Enum.OrganizationType.City || item.Type == Enum.OrganizationType.Village)
                            missingOrgs.Add(item);
                }

                if (missingOrgs.Any())
                {
                    string orgNames = "";
                    foreach(var item in missingOrgs)
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
            foreach(var record in records)
            {
                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelAccessError,rowIndex +1)
                        );

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId))
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

        public async Task<IEnumerable<BranchFeeAmountDTO>> GetExportItemsAsync(int yearId,int organizationId)
        {
            var filter = new BranchFeeAmountFilterDTO
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
                                    .Select(x => new BranchFeeAmountDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationId = x.OrganizationId,
                                        OrganizationDisplay = x.Organization.Title,
                                        UrbanAdjustmentFactor = x.UrbanAdjustmentFactor,
                                        WasteRateInWater = x.WasteRateInWater,
                                        WaterBranchingPerHousing = x.WaterBranchingPerHousing,
                                        TubingCost = x.TubingCost,
                                        WaterPartnershipAmountDomestic = x.WaterPartnershipAmountDomestic,
                                        WaterPartnershipAmountNDomestic = x.WaterPartnershipAmountNDomestic,
                                        WastePartnershipAmountDomestic = x.WastePartnershipAmountDomestic,
                                        WastePartnershipAmountNDomestic = x.WastePartnershipAmountNDomestic,
                                        FixCostNote11H = x.FixCostNote11H,
                                        FixCostNote11NH = x.FixCostNote11NH,
                                        FixCostNote11HWs = x.FixCostNote11HWs,
                                        FixCostNote11NHWs = x.FixCostNote11NHWs,
                                        WsTubingCost = x.WsTubingCost
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(BranchFeeAmountFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Select(x => new BranchFeeAmountDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationId = x.OrganizationId,
                                        OrganizationDisplay = x.Organization.Title,
                                        UrbanAdjustmentFactor = x.UrbanAdjustmentFactor,
                                        WasteRateInWater = x.WasteRateInWater,
                                        WaterBranchingPerHousing = x.WaterBranchingPerHousing,
                                        TubingCost = x.TubingCost,
                                        WaterPartnershipAmountDomestic = x.WaterPartnershipAmountDomestic,
                                        WaterPartnershipAmountNDomestic = x.WaterPartnershipAmountNDomestic,
                                        WastePartnershipAmountDomestic = x.WastePartnershipAmountDomestic,
                                        WastePartnershipAmountNDomestic = x.WastePartnershipAmountNDomestic,
                                        FixCostNote11H = x.FixCostNote11H,
                                        FixCostNote11NH = x.FixCostNote11NH,
                                        FixCostNote11HWs = x.FixCostNote11HWs,
                                        FixCostNote11NHWs = x.FixCostNote11NHWs,
                                        WsTubingCost = x.WsTubingCost
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Private Helper Methods

        private IQueryable<BranchFeeAmount> setOrder(
            IQueryable<BranchFeeAmount> query,
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
                        .OrderBy(x => x.Organization.DisplayOrder);
            }
        }

        private async Task<IQueryable<BranchFeeAmount>> setFilter(
            IQueryable<BranchFeeAmount> query,
            BranchFeeAmountFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<BranchFeeAmount>();

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
                                       x.UrbanAdjustmentFactor.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WasteRateInWater.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WaterBranchingPerHousing.ToString().ToUpper().Contains(filter.Search) ||
                                       x.TubingCost.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WaterPartnershipAmountDomestic.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WaterPartnershipAmountNDomestic.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WastePartnershipAmountDomestic.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WastePartnershipAmountNDomestic.ToString().ToUpper().Contains(filter.Search) ||
                                       x.FixCostNote11H.ToString().ToUpper().Contains(filter.Search) ||
                                       x.FixCostNote11NH.ToString().ToUpper().Contains(filter.Search) ||
                                       x.FixCostNote11HWs.ToString().ToUpper().Contains(filter.Search) ||
                                       x.FixCostNote11NHWs.ToString().ToUpper().Contains(filter.Search) ||
                                       x.WsTubingCost.ToString().ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private async Task<IEnumerable<BranchFeeAmount>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<BranchFeeAmount>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id))
                        throw new CopyDestYearHasDataException();

                    var entity = new BranchFeeAmount
                    {
                        YearId = item.YearId,
                        OrganizationId = item.OrganizationId,
                        UrbanAdjustmentFactor = item.UrbanAdjustmentFactor,
                        WasteRateInWater = item.WasteRateInWater,
                        WaterBranchingPerHousing = item.WaterBranchingPerHousing,
                        TubingCost = item.TubingCost,
                        WaterPartnershipAmountDomestic = item.WaterPartnershipAmountDomestic,
                        WaterPartnershipAmountNDomestic = item.WaterPartnershipAmountNDomestic,
                        WastePartnershipAmountDomestic = item.WastePartnershipAmountDomestic,
                        WastePartnershipAmountNDomestic = item.WastePartnershipAmountNDomestic,
                        FixCostNote11H = item.FixCostNote11H,
                        FixCostNote11NH = item.FixCostNote11NH,
                        FixCostNote11HWs = item.FixCostNote11HWs,
                        FixCostNote11NHWs = item.FixCostNote11NHWs,
                        WsTubingCost = item.WsTubingCost
                    };
                    result.Add(entity);
                }
                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }

        private async Task<IEnumerable<BranchFeeAmount>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<BranchFeeAmount>();
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
                if (await Query().Include(x => x.Organization)
                                 .AnyAsync(x => x.Organization.ParentId == orgid &&
                                                x.YearId == yearid))
                {
                    return true;
                }
                var childs = await _orgDbSet.Where(x => x.ParentId == orgid).ToListAsync();
                foreach (var child in childs)
                    return await hasAnyDataAsync(child.Id, yearid);
            }

            return false;

        }
        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                             x.OrganizationId == organizationId &&
                                             x.Id != id);
            return !result;
        }
        #endregion
    }
}
