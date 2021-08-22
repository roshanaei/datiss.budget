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
    class BranchFeeAmountService : IBranchFeeAmountService
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


    }
}
