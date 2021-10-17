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
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Services.Excel;
using Mapster;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Common.Exceptions;
using System.IO;
using Datiss.Budget.Entities;

namespace Datiss.Budget.Services
{
    public class WaterInstallFeeService : IWaterInstallFeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        
        private DbSet<WaterInstallFee> _dbSet;
        private DbSet<Organization> _orgDbSet;

        public WaterInstallFeeService(
            IUnitOfWork uow, 
            IExcelService excelService,
            IUserService userService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WaterInstallFee>();
            _orgDbSet = _uow.Set<Organization>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private IQueryable<WaterInstallFee> Query()
            => _dbSet.AsNoTracking();

        public async Task<WaterInstallFee> GetByIdAsync(int id) {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult> AddAsync(CreateWaterInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var entity = new WaterInstallFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                DWaterTypeId = model.DWaterTypeId,
                WInstllFee = model.WInstllFee
            };

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.DWaterTypeId)) {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_DWaterType, 
                                model.DWaterTypeTitle)
                );
        }

        public async Task<ValidationResult> UpdateAsync(UpdateWaterInstallFeeViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.DWaterTypeId, model.Id)) {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.DWaterTypeId = model.DWaterTypeId;
                entity.WInstllFee = model.WInstllFee;

                await _uow.SaveChangesAsync();

                return ValidationResult.Success();
            }

            return ValidationResult.Failed(
                string.Format(ServiceMessages.Logic_DWaterType,
                                model.DWaterTypeTitle)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);

            await _uow.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(int yearId, int organizationId) {
            var items = await _dbSet.Where(_ => _.YearId == yearId)
                                    .Where(_ => _.OrganizationId == organizationId)
                                    .ToListAsync();

            _dbSet.RemoveRange(items);

            await _uow.SaveChangesAsync();
        }

        public async Task<int> CalculationAsync(int yearId, int organizationId) {
            //var sum = _dbSet.Where(_ => _.YearId == yearId)
            //                        .Where(_ => _.OrganizationId == organizationId)
            //                        .GroupBy(_ => _.DWaterTypeId)
            //                        .Select(_ => new {
            //                            _.Key,
            //                            Sum = _.Sum(_ => _.WInstllFee)
            //                        });

            return await _dbSet.Where(_ => _.YearId == yearId)
                               .Where(_ => _.OrganizationId == organizationId)
                               .SumAsync(_ => _.WInstllFee);
        }

        private IQueryable<WaterInstallFee> setFilter(IQueryable<WaterInstallFee> query, WaterInstallFeeFilter filter) {
            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue)
                query = query.Where(x => x.OrganizationId == filter.OrganizationId.Value ||
                                        x.Organization.Parent.Id == filter.OrganizationId.Value ||
                                        x.Organization.Parent.Parent.Id == filter.OrganizationId.Value);

            if (filter.DWaterTypeId.HasValue)
                query = query.Where(x => x.DWaterTypeId == filter.DWaterTypeId.Value);

            if (filter.WInstallFee.HasValue) {
                switch (filter.FeeMode) {
                    case InstallFeeFilterMode.Exact:
                        query = query.Where(x => x.WInstllFee == filter.WInstallFee.Value);
                        break;
                    case InstallFeeFilterMode.GreaterThan:
                        query = query.Where(x => x.WInstllFee >= filter.WInstallFee.Value);
                        break;
                    case InstallFeeFilterMode.LessThan:
                        query = query.Where(x => x.WInstllFee <= filter.WInstallFee.Value);
                        break;
                }
            }

            return query;
        }

        public async Task<PagedResult<WaterInstallFeeViewModel>> GetListAsync(WaterInstallFeeFilter filter) 
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<WaterInstallFeeViewModel> {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            query = setFilter(query, filter);

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.DWaterType)
                                    .Select(x => new WaterInstallFeeViewModel {
                                        Id = x.Id,
                                        DWaterTypeDisplay = x.DWaterType.Title,
                                        DWaterTypeId = x.DWaterTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WInstallFee = x.WInstllFee,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId) {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();

            var result = await getChildrenData(sourceOrgId, sourceYearId, destYearId);

            _dbSet.AddRange(result);

            await _uow.SaveChangesAsync();
        }


        private async Task<IEnumerable<WaterInstallFee>> getChildrenData(int parentOrganizationId, int yearId, int targetYearId) {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<WaterInstallFee>();

            foreach (var org in children) {
                if (await Query()
                            .Where(_ => _.OrganizationId == org.Id)
                            .Where(_ => _.YearId == targetYearId).AnyAsync()) {
                    throw new CopyDestYearHasDataException();
                }

                var data = await Query()
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .ToListAsync();

                foreach (var item in data) {
                    var entity = new WaterInstallFee {
                        DWaterTypeId = item.DWaterTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        WInstllFee = item.WInstllFee
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }


        private IQueryable<WaterInstallFee> setOrder(
            IQueryable<WaterInstallFee> query,
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
                        ? query.OrderByDescending(x => x.DWaterType.DisplayOrder)
                        : query.OrderBy(x => x.DWaterType.DisplayOrder);

                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }


        public async Task ImportExcelAsync(IFormFile fileInfo) {
            var data = await _excelService.ImportAsync<WaterInstallFeeImportModel>(fileInfo);
            
            var records = data.Adapt<List<WaterInstallFee>>();

            int rowIndex = 1;
            
            foreach(var record in records) {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    throw new UserOrganizationAccessException(rowIndex);

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.DWaterTypeId))
                    throw new ImportExcelFileException(rowIndex);

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();
        }


        public async Task<Stream> ExportExcelAsync(WaterInstallFeeFilter filter) {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.DWaterType)
                                    .Select(x => new WaterInstallFeeViewModel {
                                        Id = x.Id,
                                        DWaterTypeDisplay = x.DWaterType.Title,
                                        DWaterTypeId = x.DWaterTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WInstallFee = x.WInstllFee,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int dwaterTypeId,
            int? id = null) { 
            var result = id == null 
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.DWaterTypeId == dwaterTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.DWaterTypeId == dwaterTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
