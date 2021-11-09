using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Resources;

namespace Datiss.Budget.Services
{
    public class WasteInstallFeeService : IWasteInstallFeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IOrganizationService _organizationService;
        private readonly DbSet<WasteInstallFee> _dbSet;

        public WasteInstallFeeService(
            IUnitOfWork uow,
            IOrganizationService organizationService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WasteInstallFee>();
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<WasteInstallFee> Query()
            => _dbSet.AsNoTracking();

        public async Task<WasteInstallFee> GetByIdAsync(int id) {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult> CreateAsync(CreateWasteInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new WasteInstallFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                DWasteTypeId = model.DWasteTypeId,
                WsInstllFee = model.WInstllFee
            };

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.DWasteTypeId)) {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_DWasteType, 
                                model.DWasteTypeTitle)
                );
        }

        public async Task<ValidationResult> UpdateAsync(UpdateWasteInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.DWasteTypeId, model.Id)) {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.DWasteTypeId = model.DWasteTypeId;
                entity.WsInstllFee = model.WInstllFee;

                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_DWasteType,
                                model.DWasteTypeTitle)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedResult<WasteInstallFeeDTO>> GetListAsync(WasteInstallFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<WasteInstallFeeDTO>
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
                                    .Include(x => x.DWasteType)
                                    .Select(x => new WasteInstallFeeDTO
                                    {
                                        Id = x.Id,
                                        DWasteTypeDisplay = x.DWasteType.Title,
                                        DWasteTypeId = x.DWasteTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WsInstallFee = x.WsInstllFee,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        #region Private Helper Methods
        private async Task<IQueryable<WasteInstallFee>> setFilter(
            IQueryable<WasteInstallFee> query,
            WasteInstallFeeFilterDTO filter)
        {

            var predicate = LinqKit.PredicateBuilder.New<WasteInstallFee>();

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

            if (filter.DWasteTypeId.HasValue)
                query = query.Where(x => x.DWasteTypeId == filter.DWasteTypeId.Value);

            if (filter.WInstallFee.HasValue)
            {
                switch (filter.FeeMode)
                {
                    case InstallFeeFilterMode.Exact:
                        query = query.Where(x => x.WsInstllFee == filter.WInstallFee.Value);
                        break;
                    case InstallFeeFilterMode.GreaterThan:
                        query = query.Where(x => x.WsInstllFee >= filter.WInstallFee.Value);
                        break;
                    case InstallFeeFilterMode.LessThan:
                        query = query.Where(x => x.WsInstllFee <= filter.WInstallFee.Value);
                        break;
                }
            }

            return query;
        }
        private IQueryable<WasteInstallFee> setOrder(
            IQueryable<WasteInstallFee> query,
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

                case "dwatertype":
                    return desc
                        ? query.OrderByDescending(x => x.DWasteType.DisplayOrder)
                        : query.OrderBy(x => x.DWasteType.DisplayOrder);

                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }

        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int dwaterTypeId,
            int? id = null) { 
            var result = id == null 
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.DWasteTypeId == dwaterTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.DWasteTypeId == dwaterTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
