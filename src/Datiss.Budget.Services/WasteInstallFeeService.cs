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
using Datiss.Budget.Common.Exceptions;
using System.Collections.Generic;
using Datiss.Budget.Entities;
using System.IO;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Services.Excel;
using Mapster;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{
    public class WasteInstallFeeService : IWasteInstallFeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<WasteInstallFee> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public WasteInstallFeeService(
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WasteInstallFee>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<WasteInstallFee> Query()
            => _dbSet.AsNoTracking();

        public async Task<WasteInstallFee> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<WasteInstallFeeDTO>> CreateAsync(CreateWasteInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new WasteInstallFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                DWasteTypeId = model.DWasteTypeId,
                WsInstallFee = model.WsInstallFee
            };

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.DWasteTypeId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                var result = entity.Adapt<WasteInstallFeeDTO>();
                result.DWasteTypeDisplay = (await _constSet.FindAsync(model.DWasteTypeId)).Title;
                result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                result.WsInstallFee = entity.WsInstallFee;

                return ValidationResult<WasteInstallFeeDTO>.Success(result);
            }

            return ValidationResult<WasteInstallFeeDTO>.Failed(
                string.Format(ServiceMessages.Logic_DWasteType,
                                model.DWasteTypeTitle)
                );
        }

        public async Task<ValidationResult<WasteInstallFeeDTO>> UpdateAsync(UpdateWasteInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.DWasteTypeId, model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.DWasteTypeId = model.DWasteTypeId;
                entity.WsInstallFee = model.WsInstallFee;

                await _uow.SaveChangesAsync();

                var result = new WasteInstallFeeDTO
                {
                    OrganizationId = model.OrganizationId,
                    YearId = model.YearId,
                    DWasteTypeId = model.DWasteTypeId,
                    WsInstallFee = model.WsInstallFee,
                    OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                    DWasteTypeDisplay = (await _constSet.FindAsync(model.DWasteTypeId)).Title,
                    Year = (await _yearSet.FindAsync(model.YearId)).Year
                };

                return ValidationResult<WasteInstallFeeDTO>.Success(result);
            }

            return ValidationResult<WasteInstallFeeDTO>.Failed(
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
        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId)
        {
            var organization = await _orgDbSet.FindAsync(organizationId);
            organization.CheckReferenceIsNull(nameof(organization));

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            var self = await _dbSet.Where(_ => _.YearId == yearId)
                                    .Where(_ => _.OrganizationId == organizationId)
                                    .ToListAsync();
            _dbSet.RemoveRange(self);
            var childrens = await getChildren(organizationId, yearId);
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

        public async Task<int> CalculationAsync(int yearId, int organizationId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            var result = await _uow.ExecuteScalarAsync<int>(
                "[dbo].[WasteInstallFees_Cal1] @YearId, @OrganizationId",
                parameters: sqlParams.ToArray());

            return await Task.FromResult(result);
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
                                        WsInstallFee = x.WsInstallFee,
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
            var result = new List<WasteInstallFee>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.DWasteTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new WasteInstallFee
                    {
                        DWasteTypeId = item.DWasteTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        WsInstallFee = item.WsInstallFee
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

        public async Task<Stream> ExportExcelAsync(WasteInstallFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.DWasteType)
                                    .Select(x => new WasteInstallFeeDTO
                                    {
                                        Id = x.Id,
                                        DWasteTypeDisplay = x.DWasteType.Title,
                                        DWasteTypeId = x.DWasteTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WsInstallFee = x.WsInstallFee,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        public async Task<IEnumerable<WasteInstallFeeDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new WasteInstallFeeFilterDTO
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
                                    .Include(x => x.DWasteType)
                                    .Select(x => new WasteInstallFeeDTO
                                    {
                                        Id = x.Id,
                                        DWasteTypeDisplay = x.DWasteType.Title,
                                        DWasteTypeId = x.DWasteTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WsInstallFee = x.WsInstallFee,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return items;
        }

        public async Task ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<WasteInstallFeeImportModel>(fileInfo);

            var records = data.Adapt<List<WasteInstallFee>>();

            int rowIndex = 1;

            foreach (var record in records)
            {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    throw new UserOrganizationAccessException(rowIndex);

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.DWasteTypeId))
                    throw new ImportExcelFileException(rowIndex);

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();
        }
        #region Private Helper Methods
        private async Task<IQueryable<WasteInstallFee>> setFilter(
            IQueryable<WasteInstallFee> query,
            WasteInstallFeeFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

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

            if (filter.WsInstallFee.HasValue)
            {
                switch (filter.FeeMode)
                {
                    case InstallFeeFilterMode.Exact:
                        query = query.Where(x => x.WsInstallFee == filter.WsInstallFee.Value);
                        break;
                    case InstallFeeFilterMode.GreaterThan:
                        query = query.Where(x => x.WsInstallFee >= filter.WsInstallFee.Value);
                        break;
                    case InstallFeeFilterMode.LessThan:
                        query = query.Where(x => x.WsInstallFee <= filter.WsInstallFee.Value);
                        break;
                }
            }

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                    _.DWasteType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }
        private IQueryable<WasteInstallFee> setOrder(
            IQueryable<WasteInstallFee> query,
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

                case "dwastetype":
                    return desc
                        ? query.OrderByDescending(x => x.DWasteType.DisplayOrder)
                        : query.OrderBy(x => x.DWasteType.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.DWasteType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.DWasteType.DisplayOrder);
            }
        }
        private async Task<IEnumerable<WasteInstallFee>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<WasteInstallFee>();

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
                    var entity = new WasteInstallFee
                    {
                        DWasteTypeId = item.DWasteTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        WsInstallFee = item.WsInstallFee
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<WasteInstallFee>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<WasteInstallFee>();
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
            int dwasteTypeId,
            int? id = null)
        {
            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.DWasteTypeId == dwasteTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.DWasteTypeId == dwasteTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
