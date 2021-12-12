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
using System.Data.SqlClient;

namespace Datiss.Budget.Services
{

    public class WaterInstallFeeService : IWaterInstallFeeService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        
        private readonly DbSet<WaterInstallFee> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public WaterInstallFeeService(
            IUserContext userContext,
            IUnitOfWork uow, 
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WaterInstallFee>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<WaterInstallFee> Query()
            => _dbSet.AsNoTracking();

        public async Task<WaterInstallFee> GetByIdAsync(int id) {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<WaterInstallFeeDTO>> CreateAsync(CreateWaterInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new WaterInstallFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                DWaterTypeId = model.DWaterTypeId,
                WInstllFee = model.WInstallFee
            };

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.DWaterTypeId)) {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                var result = entity.Adapt<WaterInstallFeeDTO>();
                result.DWaterTypeDisplay = (await _constSet.FindAsync(model.DWaterTypeId)).Title;
                result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                result.WInstallFee = entity.WInstllFee;

                return ValidationResult<WaterInstallFeeDTO>.Success(result);
            }

            return ValidationResult<WaterInstallFeeDTO>.Failed(
                string.Format(ServiceMessages.Logic_DWaterType, 
                                model.DWaterTypeTitle)
                );
        }

        public async Task<ValidationResult<WaterInstallFeeDTO>> UpdateAsync(UpdateWaterInstallFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.DWaterTypeId, model.Id)) {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.DWaterTypeId = model.DWaterTypeId;
                entity.WInstllFee = model.WInstallFee;

                await _uow.SaveChangesAsync();

                var result = new WaterInstallFeeDTO {
                    OrganizationId = model.OrganizationId,
                    YearId = model.YearId,
                    DWaterTypeId = model.DWaterTypeId,
                    WInstallFee = model.WInstallFee,
                    OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                    DWaterTypeDisplay = (await _constSet.FindAsync(model.DWaterTypeId)).Title,
                    Year = (await _yearSet.FindAsync(model.YearId)).Year
                };

                return ValidationResult<WaterInstallFeeDTO>.Success(result);
            }

            return ValidationResult<WaterInstallFeeDTO>.Failed(
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

        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId) {
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

        public async Task<int> CalculationAsync(int yearId, int organizationId) {
            SqlParameter resultParam = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await _uow.ExecuteSqlRawCommandAsync("[dbo].[WaterInstallFees_Cal1] @YearId, @OrganizationId, @Result OUT",
                new 
                {
                    YearId = yearId,
                    OrganizationId = organizationId,
                    Result = resultParam
                }
            );

            return await Task.FromResult(Convert.ToInt32(resultParam.Value));
        }

        public async Task<PagedResult<WaterInstallFeeDTO>> GetListAsync(WaterInstallFeeFilterDTO filter) 
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<WaterInstallFeeDTO> {
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
                                    .Include(x => x.DWaterType)
                                    .Select(x => new WaterInstallFeeDTO {
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
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            if (!await hasAnyDataAsync(sourceOrgId,sourceYearId))
                throw new CopyOrgNullDataException();
            var result = new List<WaterInstallFee>();

            if (await Query()
                        .Where(_ => _.OrganizationId == sourceOrgId)
                        .Where(_ => _.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if(selfData.Any()) {
                foreach(var item in selfData) {
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.DWaterTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new WaterInstallFee {
                        DWaterTypeId = item.DWaterTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        WInstllFee = item.WInstllFee
                    };
                    result.Add(entity);
                }
            }

            var childrens = await getChildrenData(sourceOrgId, sourceYearId, destYearId);

            if(childrens.Any()) {
                result.AddRange(childrens);
            }

            _dbSet.AddRange(result);

            await _uow.SaveChangesAsync();
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, bool continueIfAnyOrgMissing = false) {
            var data = await _excelService.ImportAsync<WaterInstallFeeImportModel>(fileInfo);
            
            var records = data.Adapt<List<WaterInstallFee>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            foreach(var rec in records) {
                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                if(org == null) {
                    return ImportResult.Failed($"سازمان به کد ({rec.Id}) در سیستم یافت نشد.");
                }
             }

            if(!continueIfAnyOrgMissing) {
                var missingOrgs = new List<Organization>();

                foreach (var item in descendents) {
                    var existInExcel = records.Any(_ => _.OrganizationId == item.Id);
                    if (!existInExcel)
                        missingOrgs.Add(item);
                }

                if (missingOrgs.Any()) {
                    string orgNames = "";
                    foreach(var item in missingOrgs) {
                        orgNames += item.Title + ",";
                    }

                    return new ImportResult
                    {
                        Message = $"سازمان های ({orgNames}) در فایل شما اطلاعاتی ندارند. آیا مایل به ادامه هستید؟",
                        AskToImport = true
                    };
                }
            }

            foreach(var record in records) {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    ImportResult.Failed(string.Format(ServiceMessages.ImportExcelAccessError, rowIndex));

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.DWaterTypeId)) {
                        ImportResult.Failed(string.Format(ServiceMessages.ImportExcelLogicError, rowIndex));
                }

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();

            return ImportResult.Succeed("ورود اطلاعات با موفقیت انجام گردید.");
        }

        public async Task<IEnumerable<WaterInstallFeeDTO>> GetExportItemsAsync(WaterInstallFeeFilterDTO filter) {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.DWaterType)
                                    .Select(x => new WaterInstallFeeDTO {
                                        Id = x.Id,
                                        DWaterTypeDisplay = x.DWaterType.Title,
                                        DWaterTypeId = x.DWaterTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WInstallFee = x.WInstllFee,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(WaterInstallFeeFilterDTO filter) {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.DWaterType)
                                    .Select(x => new WaterInstallFeeDTO {
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


        #region Private Helper Methods

        private async Task<IQueryable<WaterInstallFee>> setFilter(
            IQueryable<WaterInstallFee> query, 
            WaterInstallFeeFilterDTO filter) {

            var predicate = PredicateBuilder.New<WaterInstallFee>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue) {
                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);
                foreach (var org in organizations) {
                    predicate.Or(_ => _.OrganizationId == org.Id);
                }

                query = query.Where(predicate);
            }

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

        private async Task<IEnumerable<WaterInstallFee>> getChildrenData(
            int parentOrganizationId, 
            int yearId, 
            int targetYearId) {

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.DWaterTypeId))
                        throw new CopyDestYearHasDataException();

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
        private async Task<IEnumerable<WaterInstallFee>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<WaterInstallFee>();
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
        private async Task<bool> hasAnyDataAsync(int orgid,int yearid)
        {
            bool any = await Query().AnyAsync(x => x.OrganizationId == orgid && 
                                                x.YearId==yearid);
            if(any)
            {
                return true;
            }
            else
            {
                if (await Query().Include(x=>x.Organization)
                                 .AnyAsync(x => x.Organization.ParentId == orgid &&
                                                x.YearId == yearid))
                {
                    return true;
                }
                var childs = await _orgDbSet.Where(x => x.ParentId == orgid).ToListAsync();
                foreach (var child in childs)
                    return await hasAnyDataAsync(child.Id,yearid);
            }

            return false;

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
