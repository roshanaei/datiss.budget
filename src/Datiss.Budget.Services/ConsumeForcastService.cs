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
using Datiss.Budget.Common;

namespace Datiss.Budget.Services
{
    public class ConsumeForcastService : IConsumeForcastService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<ConsumeForcast> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public ConsumeForcastService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<ConsumeForcast>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<ConsumeForcast> Query()
            => _dbSet.AsNoTracking();

        public async Task<ConsumeForcast> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<ConsumeForcastDTO>> CreateAsync(CreateConsumeForcastDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new ConsumeForcast
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                UsageLayerId = model.UsageLayerId,
                CountUser = model.CountUser,
                UnitUser = model.UnitUser,
                ConsumeUser = model.ConsumeUser,
                AvgConsumeUser = model.AvgConsumeUser
            };

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId,model.UsageLayerId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                var result = entity.Adapt<ConsumeForcastDTO>();
                result.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;
                result.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
                result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                result.CountUser = entity.CountUser;
                result.UnitUser = entity.UnitUser;
                result.ConsumeUser = model.ConsumeUser;
                result.AvgConsumeUser = model.AvgConsumeUser;

                return ValidationResult<ConsumeForcastDTO>.Success(result);
            }

            return ValidationResult<ConsumeForcastDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumeForcast,
                                                model.UserTypeTitle,
                                                model.UsageLayerTitle)
                );
        }

        public async Task<ValidationResult<ConsumeForcastDTO>> UpdateAsync(UpdateConsumeForcastDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if(await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId,model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.YearId = model.YearId;
                entity.OrganizationId = model.OrganizationId;
                entity.UserTypeId = model.UserTypeId;
                entity.UsageLayerId = model.UsageLayerId;
                entity.UnitUser = model.UnitUser;
                entity.CountUser = model.CountUser;
                entity.ConsumeUser = model.ConsumeUser;
                entity.AvgConsumeUser = model.AvgConsumeUser;
                entity.ConsumeUserForcast = model.ConsumeUserForcast;

                await _uow.SaveChangesAsync();

                var result = new ConsumeForcastDTO
                {
                    YearId = model.YearId,
                    OrganizationId = model.OrganizationId,
                    UserTypeId = model.UserTypeId,
                    UsageLayerId = model.UsageLayerId,
                    CountUser = model.CountUser,
                    UnitUser = model.UnitUser,
                    ConsumeUser = model.ConsumeUser,
                    AvgConsumeUser = model.AvgConsumeUser,
                    ConsumeUserForcast = model.ConsumeUserForcast,
                    Year = (await _yearSet.FindAsync(model.YearId)).Year,
                    OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                    UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title,
                    UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title
                };

                return ValidationResult<ConsumeForcastDTO>.Success(result);
            }

            return ValidationResult<ConsumeForcastDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumeForcast,
                                    model.UserTypeTitle,
                                    model.UsageLayerTitle)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);

            await _uow.SaveChangesAsync();
        }

        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId,int organizationId)
        {
            var organization = await _orgDbSet.FindAsync(organizationId);
            organization.CheckReferenceIsNull(nameof(organization));

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

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

        public async Task<int> CalculationAsync(int yearId,int organizationId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter ("YearId",yearId),
                new SqlParameter ("OrganizationId",organizationId)
            };

            var result = await _uow.ExecuteScalarAsync<int>(
                "[dbo].[WaterInstallFees_Cal1] @YearId, @OrganizationId",
                parameters: sqlParams.ToArray());

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<ConsumeForcastDTO>> GetListAsync(ConsumeForcastFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<ConsumeForcastDTO>
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
                                        .Include(x => x.UserType)
                                        .Include(x => x.UsageLayer)
                                        .Select(x => new ConsumeForcastDTO
                                        {
                                            Id = x.Id,
                                            YearId = x.YearId,
                                            Year = x.FinanceYear.Year,
                                            OrganizationId = x.OrganizationId,
                                            OrganizationDisplay = x.Organization.Title,
                                            UserTypeId = x.UserTypeId,
                                            UserTypeTitle = x.UserType.Title,
                                            UsageLayerId = x.UsageLayerId,
                                            UsageLayerTitle = x.UsageLayer.Title,
                                            CountUser = x.CountUser,
                                            UnitUser = x.UnitUser,
                                            ConsumeUser = x.ConsumeUser,
                                            AvgConsumeUser = x.AvgConsumeUser,
                                            ConsumeUserForcast = x.ConsumeUserForcast
                                        }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync( int sourceYearId, int sourceOrgId,int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopySameYearException();
            if (!await hasAnyDataAsync(sourceOrgId, sourceYearId))
                throw new CopyOrgNullDataException();

            var result = new List<ConsumeForcast>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.UserTypeId,item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new ConsumeForcast
                    {
                        YearId = destYearId,
                        OrganizationId = item.OrganizationId,
                        UserTypeId = item.UserTypeId,
                        UsageLayerId = item.UsageLayerId,
                        CountUser = item.CountUser,
                        UnitUser = item.UnitUser,
                        ConsumeUser = item.ConsumeUser,
                        AvgConsumeUser = item.AvgConsumeUser
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
            var data = await _excelService.ImportAsync<ConsumeForcastImportModel>(fileInfo);

            var records = data.Adapt<List<ConsumeForcast>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var usertypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UserType);

            var usagelayers = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UsageLayerType);

            foreach (var rec in records)
            {
                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                var year = await _yearSet.FindAsync(rec.YearId);
                if (year == null || year.Status == Enum.EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex + 1, rec.YearId)
                        );
                }
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg,rowIndex + 1, rec.OrganizationId)
                        );
                }
                if ( !await usertypes.AnyAsync(x => x.Id == rec.UserTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidDWaterType , rowIndex + 1 ,rec.UserTypeId )
                        );
                }
                if (!await usagelayers.AnyAsync( x=> x.Id == rec.UsageLayerId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayerType,rowIndex + 1,rec.UsageLayerId)
                        );
                }
                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg,org.Title, rowIndex + 1)
                        );
                }

                rowIndex++;
            }
            rowIndex = 1;

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
                        orgNames += "- " + item.Title + "<br>";
                    }

                    return new ImportResult
                    {
                        Message = orgNames ,
                        AskToImport = true
                    };
                }
            }

            foreach (var record in records)
            {
                //if organization type is not city or village then pass
                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelAccessError,rowIndex + 1)
                        );

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.UserTypeId,
                    record.UsageLayerId))
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

        public async Task<IEnumerable<ConsumeForcastDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new ConsumeForcastFilterDTO
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
                                    .Include(x => x.UsageLayer)
                                    .Select(x => new ConsumeForcastDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        UserTypeTitle = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        UsageLayerTitle = x.UsageLayer.Title,
                                        CountUser = x.CountUser,
                                        UnitUser = x.UnitUser,
                                        ConsumeUser = x.ConsumeUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumeUserForcast = x.ConsumeUserForcast
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(ConsumeForcastFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Include(x => x.UsageLayer)
                                    .Select(x => new ConsumeForcastDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        UserTypeTitle = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        UsageLayerTitle = x.UsageLayer.Title,
                                        CountUser = x.CountUser,
                                        UnitUser = x.UnitUser,
                                        ConsumeUser = x.ConsumeUser,
                                        AvgConsumeUser = x.AvgConsumeUser
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Privte Helper Methods
        private async Task<IQueryable<ConsumeForcast>> setFilter(
            IQueryable<ConsumeForcast> query,
            ConsumeForcastFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<ConsumeForcast>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue)
            {
                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);

                foreach(var org in organizations)
                {
                    predicate.Or(x => x.OrganizationId == org.Id);
                }

                query = query.Where(predicate);
            }

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(x => x.Organization.Title.ToUpper().Contains(filter.Search) || 
                                         x.UserType.Title.ToUpper().Contains(filter.Search) ||
                                         x.UsageLayer.Title.ToUpper().Contains(filter.Search) ||
                                         x.UnitUser.ToString().ToUpper().Contains(filter.Search)
                                         );   
            }

            return query;
        }

        private IQueryable<ConsumeForcast> setOrder(
            IQueryable<ConsumeForcast> query,
            string orderBy = "id",
            bool desc = false){
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

                case "UserType":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);

                case "UsageLayer":
                    return desc
                        ? query.OrderByDescending(x => x.UsageLayer.DisplayOrder)
                        : query.OrderBy(x => x.UsageLayer.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .Include(x => x.UsageLayer)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.UsageLayer.DisplayOrder);
            }
        }

        private async Task<IEnumerable<ConsumeForcast>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId){

            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<ConsumeForcast>();

            foreach(var org in children){
                if (await Query()
                            .Where(x => x.OrganizationId == org.Id)
                            .Where(x => x.YearId == targetYearId).AnyAsync())
                {
                    throw new CopyDestYearHasDataException();
                }

                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .ToListAsync();

                foreach(var item in data){
                    if (!await checkLogicAsync(targetYearId, org.Id, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new ConsumeForcast
                    {
                        UserTypeId = item.UserTypeId,
                        UsageLayerId = item.UsageLayerId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        CountUser = item.CountUser,
                        UnitUser = item.UnitUser,
                        ConsumeUser = item.ConsumeUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }

        private async Task<IEnumerable<ConsumeForcast>> getChildren( 
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<ConsumeForcast>();
            foreach(var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .ToListAsync();

                foreach(var item in data)
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
                                                   x.YearId == yearid);

            if(any)
            {
                return true;
            }
            else
            {
                if(await Query().Include(x => x.Organization)
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
             int usageLayerId,
             int? id = null) {
            var result = id == null
                   ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                   x.OrganizationId == organizationId &&
                                                   x.UserTypeId == userTypeId &&
                                                   x.UsageLayerId == usageLayerId)

                   : await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId &&
                                                x.UsageLayerId == usageLayerId &&
                                                x.Id != id);

            return !result;
        }
        
        #endregion
    }
}
