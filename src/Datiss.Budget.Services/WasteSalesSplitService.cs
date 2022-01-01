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
using LinqKit;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Contracts.Identity;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Excel.Models;
using Mapster;
using Datiss.Budget.Security;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{

    public class WasteSalesSplitService : IWasteSalesSplitService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<WasteSalesSplit> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public WasteSalesSplitService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WasteSalesSplit>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }
        private IQueryable<WasteSalesSplit> Query()
               => _dbSet.AsNoTracking();

        public async Task<WasteSalesSplit> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }
        public async Task<ValidationResult<WasteSalesSplitDTO>> CreateAsync(CreateWasteSalesSplitDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var entity = new WasteSalesSplit
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                WsPipeDiameterId = model.WsPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales
            };

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.WsPipeDiameterId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();
                var result = entity.Adapt<WasteSalesSplitDTO>();
                result.Year = (await _yearSet.FindAsync(entity.YearId)).Year;
                result.OrganizationDisplay = (await _orgDbSet.FindAsync(entity.OrganizationId)).Title;
                result.UserTypeDisplay = (await _constSet.FindAsync(entity.UserTypeId)).Title;
                result.WspipeDiameterDisplay = (await _constSet.FindAsync(entity.WsPipeDiameterId)).Title;
                result.NumberSales = entity.NumberSales;
                result.UnitSales = entity.UnitSales;

                return ValidationResult<WasteSalesSplitDTO>.Success(result);
            }

            return ValidationResult<WasteSalesSplitDTO>.Failed(
                string.Format(ServiceMessages.Logic_WasteSalesSplit,
                                model.UserTypeTitle, model.WsPipeDiameterTitle)
                );
        }
        public async Task<ValidationResult<WasteSalesSplitDTO>> UpdateAsync(UpdateWasteSalesSplitDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.WsPipeDiameterId, model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.UserTypeId = model.UserTypeId;
                entity.WsPipeDiameterId = model.WsPipeDiameterId;
                entity.NumberSales = model.NumberSales;
                entity.UnitSales = model.UnitSales;

                await _uow.SaveChangesAsync();

                var result = new WasteSalesSplitDTO
                {
                    YearId = model.YearId,
                    Year = (await _yearSet.FindAsync(model.YearId)).Year,
                    OrganizationId = model.OrganizationId,
                    OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                    UserTypeId = model.UserTypeId,
                    UserTypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title,
                    WsPipeDiameterId = model.WsPipeDiameterId,
                    WspipeDiameterDisplay = (await _constSet.FindAsync(model.WsPipeDiameterId)).Title,
                    NumberSales = model.NumberSales,
                    UnitSales = model.UnitSales,
                };

                return ValidationResult<WasteSalesSplitDTO>.Success(result);
            }

            return ValidationResult<WasteSalesSplitDTO>.Failed(
               string.Format(ServiceMessages.Logic_WasteSalesSplit,
                                model.UserTypeTitle, model.WsPipeDiameterTitle)
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

            if (self.Count() == 0 && childrens.Count() == 0)
                throw new DeleteNullRecordException();

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
            var result = new List<CalculationItemData>();
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            result.Add(new CalculationItemData
            {
                Key = "WasteSalesSplit_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[WasteSalesSplit_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WasteSalesSplit_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[WasteSalesSplit_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WasteSalesSplit_Cal3",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[WasteSalesSplit_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WasteSalesSplit_Cal4",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[WasteSalesSplit_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WasteSalesSplit_Cal5",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[WasteSalesSplit_Cal5] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WasteSalesSplit_Cal6",
                Value = await _uow.ExecuteScalar<int>(
             "[dbo].[WasteSalesSplit_Cal6] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }
        public async Task<PagedResult<WasteSalesSplitDTO>> GetListAsync(WasteSalesSplitFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<WasteSalesSplitDTO>
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
                                    .Include(x => x.UserType)
                                    .Include(x => x.WsPipeDiameter)
                                    .Select(x => new WasteSalesSplitDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        WspipeDiameterDisplay = x.WsPipeDiameter.Title,
                                        WsPipeDiameterId = x.WsPipeDiameterId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        AverageCapacity = x.AverageCapacity
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
            var result = new List<WasteSalesSplit>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.UserTypeId, item.WsPipeDiameterId))
                        throw new CopyDestYearHasDataException();

                    var entity = new WasteSalesSplit
                    {
                        YearId = destYearId,
                        OrganizationId = item.OrganizationId,
                        UserTypeId = item.UserTypeId,
                        WsPipeDiameterId = item.WsPipeDiameterId,
                        UnitSales = item.UnitSales,
                        NumberSales = item.NumberSales,
                        AverageCapacity = item.AverageCapacity
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

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<WasteSalesSplitImportModel>(fileInfo);

            var records = data.Adapt<List<WasteSalesSplit>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            List<int> notAllowedToInputOrgs = new List<int>();

            foreach (var rec in records)
            {
                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rec.Id)
                        );
                }
                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    notAllowedToInputOrgs.Add(org.Id);
                }
            }

            if (!continueIfAnyOrgMissing)
            {
                var missingOrgs = new List<Organization>();

                foreach (var item in descendents)
                {
                    var existInExcel = records.Any(_ => _.OrganizationId == item.Id);
                    if (!existInExcel)
                        missingOrgs.Add(item);
                }

                if (missingOrgs.Any())
                {
                    string orgNames = "";
                    foreach (var item in missingOrgs)
                    {
                        orgNames += item.Title + ",";
                    }

                    return new ImportResult
                    {
                        Message = string.Format(ServiceMessages.ImportExcelOrgNotInExcel, orgNames),
                        AskToImport = true
                    };
                }
            }

            foreach (var record in records)
            {
                //if organization type is not city or village then pass
                if (notAllowedToInputOrgs.Contains(record.OrganizationId))
                    continue;

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    return ImportResult.Failed(string.Format(ServiceMessages.ImportExcelAccessError, rowIndex));

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.UserTypeId,
                    record.WsPipeDiameterId))
                {
                    return ImportResult.Failed(string.Format(ServiceMessages.ImportExcelLogicError, rowIndex));
                }

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();

            return ImportResult.Succeed(ServiceMessages.ImportExcelSuccess);
        }
        public async Task<IEnumerable<WasteSalesSplitDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new WasteSalesSplitFilterDTO
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
                                    .Include(x => x.UserType)
                                    .Include(x => x.WsPipeDiameter)
                                    .Select(x => new WasteSalesSplitDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WspipeDiameterDisplay = x.WsPipeDiameter.Title,
                                        WsPipeDiameterId = x.WsPipeDiameterId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        AverageCapacity = x.AverageCapacity
                                    }).ToListAsync();

            return items;
        }
        public async Task<Stream> ExportExcelAsync(WasteSalesSplitFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Include(x => x.WsPipeDiameter)
                                    .Select(x => new WasteSalesSplitDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WspipeDiameterDisplay = x.WsPipeDiameter.Title,
                                        WsPipeDiameterId = x.WsPipeDiameterId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        AverageCapacity = x.AverageCapacity
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Private Helper Methods

        private IQueryable<WasteSalesSplit> setOrder(
           IQueryable<WasteSalesSplit> query,
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

                case "usertype":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);

                case "wastediameter":
                    return desc
                        ? query.OrderByDescending(x => x.WsPipeDiameter.DisplayOrder)
                        : query.OrderBy(x => x.WsPipeDiameter.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .Include(x => x.WsPipeDiameter)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.WsPipeDiameter.DisplayOrder);
            }
        }
        private async Task<IQueryable<WasteSalesSplit>> setFilter(
            IQueryable<WasteSalesSplit> query,
            WasteSalesSplitFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<WasteSalesSplit>();

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
            if (filter.UserTypeId.HasValue)
                query = query.Where(x => x.UserTypeId == filter.UserTypeId.Value);
            if (filter.WsPipeDiameterId.HasValue)
                query = query.Where(x => x.WsPipeDiameterId == filter.WsPipeDiameterId.Value);
            
            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                    _.UserType.Title.ToUpper().Contains(filter.Search) ||
                                    _.WsPipeDiameter.Title.ToUpper().Contains(filter.Search));
            }
            return query;
        }
        private async Task<IEnumerable<WasteSalesSplit>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<WasteSalesSplit>();

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
                    var entity = new WasteSalesSplit
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        WsPipeDiameterId = item.WsPipeDiameterId,
                        UnitSales = item.UnitSales,
                        NumberSales = item.NumberSales,
                        AverageCapacity = item.AverageCapacity
                    };
                    result.Add(entity);
                }
                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }
        private async Task<IEnumerable<WasteSalesSplit>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<WasteSalesSplit>();
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
            int userTypeId,
            int wsPipeDiameterId,
            int? id = null)
        {
            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId &&
                                                x.WsPipeDiameterId == wsPipeDiameterId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == userTypeId &&
                                            x.WsPipeDiameterId == wsPipeDiameterId &&
                                            x.Id != id);
            return !result;
        }

        #endregion

    }
}

