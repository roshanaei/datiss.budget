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
                  string.Format(ServiceMessages.Logic_BranchFeeAmount,
                  model.YearId, model.OrganizationId)
                  );
            }

        public async Task<ValidationResult> UpdateAsync(UpdateBranchFeeAmountDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.YearId,model.OrganizationId))
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

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_BranchFeeAmount,
                                model.YearId, model.OrganizationId)
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

            var result = new List<BranchFeeAmount>();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();
            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
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

        public async Task<IEnumerable<BranchFeeAmountDTO>> GetExportItemsAsync(BranchFeeAmountFilterDTO filter)
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

            return items;
        }

        public async Task ImportExcelAsync(IFormFile fileInfo)
        {
            var data = await _excelService.ImportAsync<BranchFeeAmountImportModel>(fileInfo);
            var records = data.Adapt<List<BranchFeeAmount>>();
            int rowIndex = 1;
            foreach (var record in records)
            { 
                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    throw new UserOrganizationAccessException(rowIndex);

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId))
                    throw new ImportExcelFileException(rowIndex);
                rowIndex++;
            }
            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();
        }


        #region Private Helper Methods

        private IQueryable<BranchFeeAmount> setOrder(
            IQueryable<BranchFeeAmount> query,
            string orderBy = "id",
            bool desc = false) {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";
            orderBy = orderBy.ToLower();
            switch (orderBy) {
                case "year":
                    return desc
                        ? query.OrderByDescending(x => x.FinanceYear.Year)
                        : query.OrderBy(x => x.FinanceYear.Year);

                case "organization":
                    return desc
                        ? query.OrderByDescending(x => x.Organization.Title)
                        : query.OrderBy(x => x.Organization.Title);

                case "urbanAdjustmentFactor":
                    return desc
                        ? query.OrderByDescending(x => x.UrbanAdjustmentFactor)
                        : query.OrderBy(x => x.UrbanAdjustmentFactor);

                case "wasteRateInWater":
                    return desc
                        ? query.OrderByDescending(x => x.WasteRateInWater)
                        : query.OrderBy(x => x.WasteRateInWater);

                case "waterBranchingPerHousing":
                    return desc
                        ? query.OrderByDescending(x => x.WaterBranchingPerHousing)
                        : query.OrderBy(x => x.WaterBranchingPerHousing);

                case "tubingCost":
                    return desc
                        ? query.OrderByDescending(x => x.TubingCost)
                        : query.OrderBy(x => x.TubingCost);

                case "waterPartnershipAmountDomestic":
                    return desc
                        ? query.OrderByDescending(x => x.WaterPartnershipAmountDomestic)
                        : query.OrderBy(x => x.WaterPartnershipAmountDomestic);

                case "waterPartnershipAmountNDomestic":
                    return desc
                        ? query.OrderByDescending(x => x.WaterPartnershipAmountNDomestic)
                        : query.OrderBy(x => x.WaterPartnershipAmountNDomestic);


                case "wastePartnershipAmountDomestic":
                    return desc
                        ? query.OrderByDescending(x => x.WastePartnershipAmountDomestic)
                        : query.OrderBy(x => x.WastePartnershipAmountDomestic);

                case "wastePartnershipAmountNDomestic":
                    return desc
                        ? query.OrderByDescending(x => x.WastePartnershipAmountNDomestic)
                        : query.OrderBy(x => x.WastePartnershipAmountNDomestic);

                case "fixCostNote11H":
                    return desc
                        ? query.OrderByDescending(x => x.FixCostNote11H)
                        : query.OrderBy(x => x.FixCostNote11H);

                case "fixCostNote11NH ":
                    return desc
                        ? query.OrderByDescending(x => x.FixCostNote11NH)
                        : query.OrderBy(x => x.FixCostNote11NH);

                case "fixCostNote11HWs":
                    return desc
                        ? query.OrderByDescending(x => x.FixCostNote11HWs)
                        : query.OrderBy(x => x.FixCostNote11HWs);

                case "fixCostNote11NHWs":
                    return desc
                        ? query.OrderByDescending(x => x.FixCostNote11NHWs)
                        : query.OrderBy(x => x.FixCostNote11NHWs);
                case "wsTubingCost":
                    return desc
                        ? query.OrderByDescending(x => x.WsTubingCost)
                        : query.OrderBy(x => x.WsTubingCost);
                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);

            }
        }
        private async Task<IQueryable<BranchFeeAmount>> setFilter(
            IQueryable<BranchFeeAmount> query,
            BranchFeeAmountFilterDTO filter){
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

            if (filter.UrbanAdjustmentFactor.HasValue)
                query = query.Where(x => x.UrbanAdjustmentFactor == filter.UrbanAdjustmentFactor.Value);

            if (filter.WasteRateInWater.HasValue)
                query = query.Where(x => x.WasteRateInWater == filter.WasteRateInWater.Value);

            if (filter.WaterBranchingPerHousing.HasValue)
                query = query.Where(x => x.WaterBranchingPerHousing == filter.WaterBranchingPerHousing.Value);

            if (filter.TubingCost.HasValue)
                query = query.Where(x => x.TubingCost == filter.TubingCost.Value);

            if (filter.WaterPartnershipAmountDomestic.HasValue)
                query = query.Where(x => x.WaterPartnershipAmountDomestic == filter.WaterPartnershipAmountDomestic.Value);

            if (filter.WaterPartnershipAmountNDomestic.HasValue)
                query = query.Where(x => x.WaterPartnershipAmountNDomestic == filter.WaterPartnershipAmountNDomestic.Value);

            if (filter.WastePartnershipAmountDomestic.HasValue)
                query = query.Where(x => x.WastePartnershipAmountDomestic == filter.WastePartnershipAmountDomestic.Value);

            if (filter.WastePartnershipAmountNDomestic.HasValue)
                query = query.Where(x => x.WastePartnershipAmountNDomestic == filter.WastePartnershipAmountNDomestic.Value);

            if (filter.FixCostNote11H.HasValue)
                query = query.Where(x => x.FixCostNote11H == filter.FixCostNote11H.Value);

            if (filter.FixCostNote11NH.HasValue)
                query = query.Where(x => x.FixCostNote11NH == filter.FixCostNote11NH.Value);

            if (filter.FixCostNote11HWs.HasValue)
                query = query.Where(x => x.FixCostNote11HWs == filter.FixCostNote11HWs.Value);

            if (filter.FixCostNote11NHWs.HasValue)
                query = query.Where(x => x.FixCostNote11NHWs == filter.FixCostNote11NHWs.Value);

            if (filter.WsTubingCost.HasValue)
                query = query.Where(x => x.WsTubingCost == filter.WsTubingCost.Value);

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

        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int? id = null)
        {
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
