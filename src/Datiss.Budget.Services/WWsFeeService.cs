using Datiss.Budget.Common;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class WWsFeeService : IWWsFeeService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<WWsFee> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public WWsFeeService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WWsFee>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<WWsFee> Query()
            => _dbSet.AsNoTracking();

        public async Task<WWsFee> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<WWsFeeDTO>> CreateAsync(CreateWWsFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new WWsFee
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                ActivityType = model.ActivityType,
                UserTypeId = model.UserTypeId,
                UsageLayerId = model.UsageLayerId,
                P1Fee = model.P1Fee,
                P2Fee = model.P2Fee,
                P1Note3 = model.P1Note3,
                P1Note7 = model.P1Note7,
                P2Note3 = model.P2Note3,
                P2Note7 = model.P2Note7                
            };

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            var usageLayerDisplay = (await _constSet.FindAsync(model.UsageLayerId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId,model.ActivityType, model.UserTypeId, model.UsageLayerId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<WWsFeeDTO>();
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.UsageLayerDisplay = usageLayerDisplay;
                    result.ActivityType = model.ActivityType;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.P1Fee = entity.P1Fee;
                    result.P2Fee = entity.P2Fee;
                    result.P1Note3 = entity.P1Note3;
                    result.P1Note7 = entity.P1Note7;
                    result.P2Note3 = entity.P2Note3;
                    result.P2Note7 = entity.P2Note7;

                    return ValidationResult<WWsFeeDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<WWsFeeDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<WWsFeeDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                model.UserTypeTitle, usageLayerDisplay, organizationDisplay)
                );
        }

        public async Task<ValidationResult<WWsFeeDTO>> UpdateAsync(UpdateWWsFeeDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            var usageLayerDisplay = (await _constSet.FindAsync(model.UsageLayerId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ActivityType, model.UserTypeId, model.UsageLayerId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.ActivityType = model.ActivityType;
                    entity.UserTypeId = model.UserTypeId;
                    entity.UsageLayerId = model.UsageLayerId;
                    entity.P1Fee = model.P1Fee;
                    entity.P2Fee = model.P2Fee;
                    entity.P1Note3 = model.P1Note3;
                    entity.P1Note7 = model.P1Note7;
                    entity.P2Note3 = model.P2Note3;
                    entity.P2Note7 = model.P2Note7;

                    await _uow.SaveChangesAsync();

                    var result = new WWsFeeDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        ActivityType = model.ActivityType,
                        UsageLayerId = model.UsageLayerId,
                        P1Fee = model.P1Fee,
                        P2Fee = model.P2Fee,
                        P1Note3 = model.P1Note3,
                        P1Note7 = model.P1Note7,
                        P2Note3 = model.P2Note3,
                        P2Note7 = model.P2Note7,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = model.UserTypeTitle,
                        UsageLayerDisplay = usageLayerDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<WWsFeeDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<WWsFeeDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<WWsFeeDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                model.UserTypeTitle, usageLayerDisplay, organizationDisplay)
                );
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckReferenceIsNull(nameof(entity));

            var year = await _yearSet.FindAsync(entity.YearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();
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

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var self = await _dbSet.Where(_ => _.YearId == yearId)
                                    .Where(_ => _.OrganizationId == organizationId)
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

        public async Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId, int organizationId)
        {

            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };
            var result = new List<CalculationItemData>();
            result.Add(new CalculationItemData
            {
                //Key = "WaterInstallFees_Cal1",
                //Value = await _uow.ExecuteScalar<int>("[dbo].[WwSFees_Cal1] @YearId, @OrganizationId",parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<WWsFeeDTO>> GetListAsync(WWsFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<WWsFeeDTO>
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
                                    .Select(x => new WWsFeeDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        UsageLayerId = x.UsageLayerId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        P1Fee = x.P1Fee,
                                        P2Fee = x.P2Fee,
                                        P1Note3 = x.P1Note3,
                                        P2Note3 = x.P2Note3,
                                        P1Note7 = x.P1Note7,
                                        P2Note7 = x.P2Note7
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

            var result = new List<WWsFee>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.ActivityType, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new WWsFee
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ActivityType = item.ActivityType,
                        UsageLayerId = item.UsageLayerId,
                        P1Fee = item.P1Fee,
                        P2Fee = item.P2Fee,
                        P1Note3 = item.P1Note3,
                        P2Note3 = item.P2Note3,
                        P1Note7 = item.P1Note7,
                        P2Note7 = item.P2Note7
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
            var data = await _excelService.ImportAsync<WWsFeeImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<WWsFee>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var usertypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UserType &&
                                                 x.Status != EntityStatus.Deleted);

            var usagelayers = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UsageLayerType &&
                                                 x.Status != EntityStatus.Deleted);

            var houseUsageLayer = await usagelayers.Where(x => x.ConstantKey != ConstantKeys.__UsageLayerType).ToListAsync();
            var noneHouseUsageLayer = await usagelayers.Where(x => x.ConstantKey == ConstantKeys.__UsageLayerType).ToListAsync();


            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;

                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex + 2, rec.YearId)
                        );
                }
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rowIndex + 2, rec.OrganizationId)
                        );
                }
                if (!await usertypes.AnyAsync(x => x.Id == rec.UserTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUserType, rowIndex + 2, rec.UserTypeId)
                        );
                }
                if (!await usagelayers.AnyAsync(x => x.Id == rec.UsageLayerId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayer, rowIndex + 2, rec.UserTypeId)
                        );
                }

                var userType = await _constSet.FindAsync(rec.UserTypeId);

                if (userType.ConstantKey == ConstantKeys.__House && !houseUsageLayer.Any(x => x.Id == rec.UsageLayerId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayerUserType, rowIndex + 2, rec.UsageLayerId, userType.Title)
                        );
                }

                if (userType.ConstantKey != ConstantKeys.__House && !noneHouseUsageLayer.Any(x => x.Id == rec.UsageLayerId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayerUserType, rowIndex + 2, rec.UsageLayerId, userType.Title)
                        );
                }

                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex + 2)
                        );
                }

                rowIndex++;
            }

            //Start Missing Org
            var missingOrgs = new List<Organization>();
            var existOrgs = new List<Organization>();

            foreach (var item in descendents)
            {
                var existInExcel = records.Any(_ => _.OrganizationId == item.Id);
                if (!existInExcel)
                {
                    if (item.Type == Enum.OrganizationType.City || item.Type == Enum.OrganizationType.Village)
                        missingOrgs.Add(item);
                }
                else
                    existOrgs.Add(item);
            }
            //End

            //Start Missing Type
            var missingUserType = new List<Constant>();
            var missingHUsageLayerType = new List<Constant>();
            var missingNHUsageLayerType = new List<Constant>();

            string orgTitle = "";
            string orgTitleHouse = "";
            string orgTitleNHouse = "";

            string hUsageLayerTitle = "";
            string nHUsageLayerTitle = "";

            foreach (var org in existOrgs)
            {
                foreach (var usert in usertypes)
                {
                    var existUserTypeInExcel = records.Any(_ => _.UserTypeId == usert.Id &&
                                                                _.OrganizationId == org.Id);
                    if (!existUserTypeInExcel)
                    {
                        missingUserType.Add(usert);
                        orgTitle = org.Title;
                    }
                    else 
                    {
                        if (usert.ConstantKey == ConstantKeys.__House)
                        {
                            foreach (var usage in houseUsageLayer)
                            {
                                var exist = records.Any(_ => _.UserTypeId == usert.Id &&
                                                             _.OrganizationId == org.Id &&
                                                             _.UsageLayerId == usage.Id);
                                if (!exist)
                                {
                                    missingHUsageLayerType.Add(usage);
                                    hUsageLayerTitle = usert.Title;
                                    orgTitleHouse = org.Title;
                                }
                            }
                        }
                        else
                        {
                            foreach (var nhusage in noneHouseUsageLayer)
                            {
                                var exist = records.Any(_ => _.UserTypeId == usert.Id &&
                                                             _.OrganizationId == org.Id &&
                                                             _.UsageLayerId == nhusage.Id);
                                if (!exist)
                                {
                                    missingNHUsageLayerType.Add(nhusage);
                                    nHUsageLayerTitle = usert.Title;
                                    orgTitleNHouse = org.Title;
                                }
                            }
                        }
                    }
                }
            }

            if (missingUserType.Any())
            {
                string userTypeNames = "";
                foreach (var item in missingUserType)
                {
                    userTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUserTypeOrgNotInExcel, userTypeNames, orgTitle));
            }

            if (missingHUsageLayerType.Any())
            {
                string usageTypeNames = "";
                foreach (var item in missingHUsageLayerType)
                {
                    usageTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUsageLayerUserTypeOrgNotInExcel, usageTypeNames, hUsageLayerTitle, orgTitleHouse));
            }

            if (missingNHUsageLayerType.Any())
            {
                string usageTypeNames = "";
                foreach (var item in missingNHUsageLayerType)
                {
                    usageTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUsageLayerUserTypeOrgNotInExcel, usageTypeNames, nHUsageLayerTitle, orgTitleNHouse));
            }
            //End

            rowIndex = 1;

            if (!continueIfAnyOrgMissing)
            {
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
                    throw new UserOrganizationAccessException(rowIndex);

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.ActivityType,
                    record.UserTypeId,
                    record.UsageLayerId))
                {

                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelLogicError, rowIndex + 2)
                        );
                }

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelCalculationField)
                    );
            }

            return ImportResult.Succeed(
                string.Format(ServiceMessages.ImportExcelSuccess)
                );
        }

        public async Task<IEnumerable<WWsFeeDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new WWsFeeFilterDTO
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
                                    .Select(x => new WWsFeeDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        UsageLayerId = x.UsageLayerId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        P1Fee = x.P1Fee,
                                        P2Fee = x.P2Fee,
                                        P1Note3 = x.P1Note3,
                                        P2Note3 = x.P2Note3,
                                        P1Note7 = x.P1Note7,
                                        P2Note7 = x.P2Note7
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(WWsFeeFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Include(x=> x.UsageLayer)
                                    .Select(x => new WWsFeeDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        ActivityType = x.ActivityType,
                                        UsageLayerId = x.UsageLayerId,
                                        UsageLayerDisplay = x.UserType.Title,
                                        P1Fee = x.P1Fee,
                                        P2Fee = x.P2Fee,
                                        P1Note3 = x.P1Note3,
                                        P2Note3 = x.P2Note3,
                                        P1Note7 = x.P1Note7,
                                        P2Note7 = x.P2Note7
                                    }).ToListAsync();

            var ms = new MemoryStream();

            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<WWsFee>> setFilter(
            IQueryable<WWsFee> query,
            WWsFeeFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<WWsFee>();

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

            if (filter.ActivityType.HasValue)
                query = query.Where(x => x.ActivityType == filter.ActivityType);

            if (filter.UsageLayerId.HasValue)
                query = query.Where(x => x.UsageLayerId == filter.UsageLayerId.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();

                bool isNum = int.TryParse(filter.Search, out int res);

                if (isNum)
                {
                    query = query.Where(_ => _.P1Fee.ToString().Contains(filter.Search) ||
                                             _.P2Fee.ToString().Contains(filter.Search) ||
                                             _.P1Note3.ToString().Contains(filter.Search) ||
                                             _.P1Note7.ToString().Contains(filter.Search) ||
                                             _.P2Note3.ToString().Contains(filter.Search) ||
                                             _.P2Note7.ToString().Contains(filter.Search));
                }
                else
                {
                    query = query.Where(_ => _.UserType.Title.ToUpper().Contains(filter.Search)||
                                             _.Organization.Title.ToUpper().Contains(filter.Search)||
                                             _.UsageLayer.Title.ToUpper().Contains(filter.Search));
                }
            }
            return query;
        }

        private IQueryable<WWsFee> setOrder(
           IQueryable<WWsFee> query,
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

                case "usagelayer":
                    return desc
                        ? query.OrderByDescending(x => x.UsageLayer.DisplayOrder)
                        : query.OrderBy(x => x.UsageLayer.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .Include(x => x.UsageLayer)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.ActivityType)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.UsageLayer.DisplayOrder);
            }
        }

        private async Task<IEnumerable<WWsFee>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<WWsFee>();

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
                    var entity = new WWsFee
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ActivityType = item.ActivityType,
                        UsageLayerId = item.UsageLayerId,
                        P1Fee = item.P1Fee,
                        P2Fee = item.P2Fee,
                        P1Note3 = item.P1Note3,
                        P2Note3 = item.P2Note3,
                        P1Note7 = item.P1Note7,
                        P2Note7 = item.P2Note7
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }

        private async Task<IEnumerable<WWsFee>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<WWsFee>();

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
            ActivityType activityType,
            int userTypeId,
            int UsageLayerId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.ActivityType == activityType &&
                                                x.UserTypeId == userTypeId &&
                                                x.UsageLayerId == UsageLayerId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                              x.OrganizationId == organizationId &&
                                              x.ActivityType == activityType &&
                                              x.UserTypeId == userTypeId &&
                                              x.UsageLayerId == UsageLayerId &&
                                              x.Id != id);
            return !result;
        }

        #endregion
    }
}
