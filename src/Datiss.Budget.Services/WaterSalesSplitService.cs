using Datiss.Budget.DataLayer.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Resources;


namespace Datiss.Budget.Services
{
    public class WaterSalesSplitService : IWaterSalesSplitService
    {
        private readonly IUnitOfWork _uow;

        private readonly DbSet<WaterSalesSplit> _dbSet;

        public WaterSalesSplitService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WaterSalesSplit>();
        }

        private IQueryable<WaterSalesSplit> Query()
              => _dbSet.AsNoTracking();

        public async Task<WaterSalesSplit> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult> CreateAsync(CreateWaterSalesSplitDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var entity = new WaterSalesSplit
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                WPipeDiameterId = model.WPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales
            };

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.WPipeDiameterId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_WaterSalesSplit,
                                model.UserTypeTitle, model.WPipeDiameterTitle)
                );
        }


        public async Task<ValidationResult> UpdateAsync(UpdateWaterSalesSplitDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.WPipeDiameterId, model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.UserTypeId = model.UserTypeId;
                entity.WPipeDiameterId = model.WPipeDiameterId;
                entity.NumberSales = model.NumberSales;
                entity.UnitSales = model.UnitSales;

                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
               string.Format(ServiceMessages.Logic_WaterSalesSplit,
                                model.UserTypeTitle, model.WPipeDiameterTitle)
               );
        }
        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);
            await _uow.SaveChangesAsync();

        }

         public async Task<PagedResult<WaterSalesSplitDTO>> GetListAsync(WaterSalesSplitFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<WaterSalesSplitDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            if (filter.UserTypeId.HasValue)
                query = query.Where(x => x.UserTypeId == filter.UserTypeId.Value);

            if (filter.WPipeDiameterId.HasValue)
                query = query.Where(x => x.WPipeDiameterId == filter.WPipeDiameterId.Value);

            if (filter.NumberSales.HasValue)
            {
                switch (filter.NumberMode)
                {
                    case InstallFeeFilterMode.Exact:
                        query = query.Where(x => x.NumberSales == filter.NumberSales.Value);
                        break;
                    case InstallFeeFilterMode.GreaterThan:
                        query = query.Where(x => x.NumberSales >= filter.NumberSales.Value);
                        break;
                    case InstallFeeFilterMode.LessThan:
                        query = query.Where(x => x.NumberSales <= filter.NumberSales.Value);
                        break;
                }
            }

            if (filter.UnitSales.HasValue)
            {
                switch (filter.UnitMode)
                {
                    case InstallFeeFilterMode.Exact:
                        query = query.Where(x => x.UnitSales == filter.UnitSales.Value);
                        break;
                    case InstallFeeFilterMode.GreaterThan:
                        query = query.Where(x => x.UnitSales >= filter.UnitSales.Value);
                        break;
                    case InstallFeeFilterMode.LessThan:
                        query = query.Where(x => x.UnitSales <= filter.UnitSales.Value);
                        break;
                }
            }
            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Include(x=> x.WPipeDiameter)
                                    .Select(x => new WaterSalesSplitDTO 
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        WPipeDiameterDisplay = x.WPipeDiameter.Title,
                                        WPipeDiameterId = x.WPipeDiameterId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }


        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int userTypeId,
            int wPipeDiameterId,
            int? id = null)
        {
            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId &&
                                                x.WPipeDiameterId == wPipeDiameterId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == userTypeId &&
                                            x.WPipeDiameterId == wPipeDiameterId &&
                                            x.Id != id);
            return !result;
        }

        #endregion

        #region Private Helper Methods

        private IQueryable<WaterSalesSplit> setOrder(
            IQueryable<WaterSalesSplit> query,
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

                case "usertype":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);
                case "WPipeDiameter":
                    return desc
                        ? query.OrderByDescending(x => x.WPipeDiameter.DisplayOrder)
                        : query.OrderBy(x => x.WPipeDiameter.DisplayOrder);

                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }

        #endregion
    }
}
    
