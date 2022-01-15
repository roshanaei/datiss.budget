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
    public class ConsumeForcastWsService : IConsumeForcastWsService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<ConsumeForcastWs> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public ConsumeForcastWsService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<ConsumeForcastWs>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<ConsumeForcastWs> Query()
            => _dbSet.AsNoTracking();

        public async Task<ConsumeForcastWs> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<ConsumeForcastWsDTO>> CreateAsync(CreateConsumeForcastWsDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new ConsumeForcastWs
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

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;

            model.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;

            try
            {
                if(await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<ConsumeForcastWsDTO>();
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                    result.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
                    result.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;
                    result.CountUser = entity.CountUser;
                    result.UnitUser = entity.UnitUser;
                    result.ConsumeUser = entity.ConsumeUser;
                    result.AvgConsumeUser = entity.AvgConsumeUser;

                    return ValidationResult<ConsumeForcastWsDTO>.Success(result);
                }
            }
            catch
            {
                return ValidationResult<ConsumeForcastWsDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<ConsumeForcastWsDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumeForcastWs,
                model.UserTypeTitle,
                model.UserTypeTitle)
                );
        }

        public async Task<ValidationResult<ConsumeForcastWsDTO>> UpdateAsync(UpdateConsumeForcastWsDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            model.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.UsageLayerId = model.UsageLayerId;
                    entity.CountUser = model.CountUser;
                    entity.UnitUser = model.UnitUser;
                    entity.ConsumeUser = model.ConsumeUser;
                    entity.AvgConsumeUser = model.AvgConsumeUser;
                    entity.ConsumeUserForcast = model.ConsumeUserForcast;

                    await _uow.SaveChangesAsync();

                    var result = new ConsumeForcastWsDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        UsageLayerId = model.UsageLayerId,
                        CountUser = model.CountUser,
                        UnitUser = model.UnitUser,
                        ConsumeUser = model.ConsumeUser,
                        AvgConsumeUser = model.AvgConsumeUser,
                        ConsumeUserForcast = model.ConsumeUserForcast,
                        OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title,
                        UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title
                    };
                    return ValidationResult<ConsumeForcastWsDTO>.Success(result);
                }
            }
            catch(DisbaledYearDataInputException)
            {
                return ValidationResult<ConsumeForcastWsDTO>.Failed(ServiceMessages.Logic_ConsumeForcastWs);
            }
            return ValidationResult<ConsumeForcastWsDTO>.Failed(
                string.Format(ServiceMessages.Logic_ConsumeForcastWs,
                model.UserTypeTitle,
                model.UsageLayerTitle)
                );
            }
        
        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            entity.CheckReferenceIsNull(nameof(entity));

            var year = await _yearSet.FindAsync(entity.YearId);
            year.CheckReferenceIsNull(nameof(entity));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();
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

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

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

        public async Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId,int organizationId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId",yearId),
                new SqlParameter("OrganizationId",organizationId)
            };

            var result = new List<CalculationItemData>();

            result.Add(new CalculationItemData { 
                Key = "ConsumeForcastWs_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                    "[dbo].[ConsumeForcastWs_Cal1] @YearId, @OrganizationId",
                    parameters :sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<ConsumeForcastWsDTO>> GetListAsync (ConsumeForcastWsFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<ConsumeForcastWsDTO>
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
                                      .Select(x => new ConsumeForcastWsDTO 
                                      {
                                          Id = x.Id,
                                          Year = x.FinanceYear.Year,
                                          YearId = x.YearId,
                                          OrganizationDisplay = x.Organization.Title,
                                          OrganizationId = x.OrganizationId,
                                          UserTypeTitle = x.UserType.Title,
                                          UserTypeId = x.UserTypeId,
                                          UsageLayerTitle = x.UsageLayer.Title,
                                          UsageLayerId = x.UsageLayerId,
                                          CountUser = x.CountUser,
                                          UnitUser = x.UnitUser,
                                          ConsumeUser = x.ConsumeUser,
                                          AvgConsumeUser = x.AvgConsumeUser,
                                          ConsumeUserForcast = x.ConsumeUserForcast

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
            
            var result = new List<ConsumeForcastWs>();

            if (await Query()
                        .Where(x => x.OrganizationId == sourceOrgId)
                        .Where(x => x.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(x => x.OrganizationId == sourceOrgId)
                                        .Where(x => x.YearId == sourceYearId)
                                        .ToListAsync();
            if (selfData.Any())
            {
                foreach(var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, sourceYearId, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new ConsumeForcastWs
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

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<ConsumeForcastWsImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<ConsumeForcastWs>>();

            int rowIndex = 1;

            var usertypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UserType);

            var usagetypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UsageLayerType);

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;

                var org = await _orgDbSet.FindAsync(rec.OrganizationId);

                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex + 1, rec.YearId)
                        );
                }

                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rowIndex + 1, rec.OrganizationId)
                        );
                }

                if (!await usertypes.AnyAsync(x => x.Id == rec.UserTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidDWaterType, rowIndex + 1, rec.UserTypeId)
                        );
                }

                if (!await usagetypes.AnyAsync(x => x.Id == rec.UsageLayerId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayerType, rowIndex + 1, rec.UsageLayerId)
                        );
                }

                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex + 1)
                        );
                }

                rowIndex++;
            }

            //Strat UserType
            var missingUserType = new List<Constant>();

            foreach (var item in usertypes)
            {
                var existUserTypeInExcel = records.Any(x => x.UserTypeId == item.Id);
                if (!existUserTypeInExcel)
                    missingUserType.Add(item);
            }
            if (missingUserType.Any())
            {
                string userTypeNames = "";
                foreach (var item in missingUserType)
                {
                    userTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelDWTypeNotInExcel, userTypeNames));

            }
            //End UserType

            //start Usagelayer
            //End UsageLayer

            rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            if (!continueIfAnyOrgMissing)
            {
                var missingOrgs = new List<Organization>();

                foreach (var item in descendents)
                {
                    var existInExcel = records.Any(x => x.OrganizationId == item.Id);
                    if (!existInExcel)
                        if (item.Type == Enum.OrganizationType.City || item.Type == Enum.OrganizationType.Village)
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
                        Message = orgNames,
                        AskToImport = true
                    };
                }
            }
            foreach (var record in records)
            {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelAccessError, rowIndex + 1)
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
        
        public async Task<IEnumerable<ConsumeForcastWsDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new ConsumeForcastWsFilterDTO
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
                                    .Select(x => new ConsumeForcastWsDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        UserTypeTitle = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        UsageLayerTitle = x.UsageLayer.Title,
                                        UsageLayerId = x.UsageLayerId,
                                        CountUser = x.CountUser,
                                        UnitUser = x.UnitUser,
                                        ConsumeUser = x.ConsumeUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumeUserForcast = x.ConsumeUserForcast
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(ConsumeForcastWsFilterDTO filter)
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
                                    .Select(x => new ConsumeForcastWsDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        UserTypeTitle = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        UsageLayerTitle = x.UsageLayer.Title,
                                        UsageLayerId = x.UsageLayerId,
                                        CountUser = x.CountUser,
                                        UnitUser = x.UnitUser,
                                        ConsumeUser = x.ConsumeUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumeUserForcast = x.ConsumeUserForcast
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Private Helper Methods
        private async Task<IQueryable<ConsumeForcastWs>> setFilter(
            IQueryable<ConsumeForcastWs> query,
            ConsumeForcastWsFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));

            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<ConsumeForcastWs>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.OrganizationId.HasValue)
            {
                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);

                foreach (var org in organizations)
                {
                    predicate.Or(x => x.OrganizationId == org.Id);
                }

                query = query.Where(predicate);
            }

            if(filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(x => x.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         x.UserType.Title.ToUpper().Contains(filter.Search) ||
                                         x.UsageLayer.Title.ToUpper().Contains(filter.Search) ||
                                         x.CountUser.ToString().ToUpper().Contains(filter.Search) ||
                                         x.UnitUser.ToString().ToUpper().Contains(filter.Search) ||
                                         x.ConsumeUser.ToString().ToUpper().Contains(filter.Search) ||
                                         x.AvgConsumeUser.ToString().ToUpper().Contains(filter.Search) ||
                                         x.ConsumeUserForcast.ToString().ToUpper().Contains(filter.Search));

            }
            return query;
        }

        private IQueryable<ConsumeForcastWs> setOrder(
            IQueryable<ConsumeForcastWs> query,
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
                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .Include(x => x.UsageLayer)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.UsageLayer.DisplayOrder);
            }
        }

        private async Task<IEnumerable<ConsumeForcastWs>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<ConsumeForcastWs>();

            foreach(var org in children)
            {
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

                foreach(var item in data)
                {
                    if (!await checkLogicAsync(targetYearId, org.Id, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new ConsumeForcastWs
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        UserTypeId = item.UserTypeId,
                        UsageLayerId = item.UsageLayerId,
                        CountUser = item.CountUser,
                        UnitUser = item.UnitUser,
                        ConsumeUser = item.ConsumeUser,
                        AvgConsumeUser = item.AvgConsumeUser
                    };

                    result.Add(entity);
                }
                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }

        private async Task<IEnumerable<ConsumeForcastWs>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<ConsumeForcastWs>();

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
            if (any)
            {
                return true;
            }
            else
            {
                var childs = await _organizationService.GetWithChildrenAsync(orgid);
                foreach (var child in childs)
                    if (await Query().AnyAsync(x => x.YearId == yearid && x.OrganizationId == child.Id))
                        return true;
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
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);

            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

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
