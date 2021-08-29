using Datiss.Budget.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Resources;

namespace Datiss.Budget.Services
{
    public class BranchFeeAmountService : IBranchFeeAmountService
    {
        private readonly IUnitOfWork _uow;

        private DbSet<BranchFeeAmount> _dbSet;

        public BranchFeeAmountService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));

            _dbSet = _uow.Set<BranchFeeAmount>();
        }

        private IQueryable<BranchFeeAmount> Query()
            => _dbSet.AsNoTracking();

        public async Task<BranchFeeAmount> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult> AddAsync(CreateBranchFeeAmountDTO model)
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

            if(await checkLogicAsync(model.YearId, model.OrganizationId))
            {
                await _dbSet.AddAsync(entity); 
                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }
            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_BranchFeeAmount,
                                    model.YearId, model.OrganizationId)
                );
        }

        public async Task<ValidationResult> UpdateAsync(UpdateBranchFeeAmountViewModel model)
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
        
        public async Task<PagedResult<BranchFeeAmountViewModel>> GetListAsync(BranchFeeAmountFilter filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<BranchFeeAmountViewModel>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

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

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                            .Include(x => x.FinanceYear)
                            .Include(x => x.Organization)
                            .Include(x => x.UrbanAdjustmentFactor)
                            .Include(x => x.WasteRateInWater)
                            .Include(x => x.WaterBranchingPerHousing)
                            .Include(x => x.TubingCost)
                            .Include(x => x.WaterPartnershipAmountDomestic)
                            .Include(x => x.WaterPartnershipAmountNDomestic)
                            .Include(x => x.WastePartnershipAmountDomestic)
                            .Include(x => x.WastePartnershipAmountNDomestic)
                            .Include(x => x.FixCostNote11H)
                            .Include(x => x.FixCostNote11NH)
                            .Include(x => x.FixCostNote11HWs)
                            .Include(x => x.FixCostNote11NHWs)
                            .Include(x => x.WsTubingCost)
                            .Select(x => new BranchFeeAmountViewModel {
                                Id=x.Id,
                                Year = x.FinanceYear.Year,
                                YearId = x.YearId,
                                OrganizationId = x.OrganizationId,
                                OrganizationDisplay = x.Organization.Title,
                                UrbanAdjustmentFactor = x.UrbanAdjustmentFactor,
                                WasteRateInWater = x.WasteRateInWater,
                                WaterBranchingPerHousing  = x.WaterBranchingPerHousing,
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
        private IQueryable<BranchFeeAmount>setOrder(
            IQueryable<BranchFeeAmount> query,
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
