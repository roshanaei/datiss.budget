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
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<ConsumeForcastWsDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
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
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
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

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<ConsumeForcastWsDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
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
                return ValidationResult<ConsumeForcastWsDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }
            return ValidationResult<ConsumeForcastWsDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
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
                Key = "ConsumeForcastWs_Cal1",
                DecimalValue = await _uow.ExecuteScalar<decimal>(
                                    "[dbo].[ConsumeForcastWs_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "ConsumeForcastWs_Cal2",
                DecimalValue = await _uow.ExecuteScalar<decimal>(
                                    "[dbo].[ConsumeForcastWs_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "ConsumeForcastWs_Cal3",
                DecimalValue = await _uow.ExecuteScalar<decimal>(
                         "[dbo].[ConsumeForcastWs_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "ConsumeForcastWs_Cal4",
                DecimalValue = await _uow.ExecuteScalar<decimal>(
                         "[dbo].[ConsumeForcastWs_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
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

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                throw new CopyDataBaseException();
            }
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<ConsumeForcastWsImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<ConsumeForcastWs>>();

            int rowIndex = 1;
            //Organization
            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);
            //Constant
            var usertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                 x.Parent.ConstantKey == ConstantKeys.__UserType);

            var usagelayers = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__UsageLayerType);

            var houseUsageLayer = await usagelayers.Where(x => x.ConstantKey != ConstantKeys.__UsageLayerType).ToListAsync();
            var noneHouseUsageLayer = await usagelayers.Where(x => x.ConstantKey == ConstantKeys.__UsageLayerType).ToListAsync();

            //Year
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;

                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                if (year == null || year.Status == Enum.EntityStatus.Disbaled)
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
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayer, rowIndex + 2, rec.UsageLayerId)
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

            //
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
            //

            //Start Check Type
            var missingUType = new List<Constant>();
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
                    var existUTypeInExcel = records.Any(_ => _.UserTypeId == usert.Id &&
                                                              _.OrganizationId == org.Id);
                    if (!existUTypeInExcel)
                    {
                        missingUType.Add(usert);
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

            if (missingUType.Any())
            {
                string uTypeNames = "";
                foreach (var item in missingUType)
                {
                    uTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUserTypeOrgNotInExcel, uTypeNames, orgTitle));
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

            //End Check Type

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
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelAccessError, rowIndex + 2)
                        );

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
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
                                         x.UsageLayer.Title.ToUpper().Contains(filter.Search));

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
                                .ThenBy(x => x.Organization.RowOrder)
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
