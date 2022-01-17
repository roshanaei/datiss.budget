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
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Entities;
using Datiss.Budget.Common.Exceptions;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Mapster;
using Datiss.Budget.Security;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Extensions;
using Datiss.Budget.Common;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services
{
    public class WaterSalesSplitService : IWaterSalesSplitService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<WaterSalesSplit> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public WaterSalesSplitService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<WaterSalesSplit>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<WaterSalesSplit> Query()
              => _dbSet.AsNoTracking();

        public async Task<WaterSalesSplit> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<WaterSalesSplitDTO>> CreateAsync(CreateWaterSalesSplitDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            if (model.NumberSales < 0 || model.UnitSales < 0 || model.AverageCapacity < 0)
            {
                return ValidationResult<WaterSalesSplitDTO>.Failed(ServiceMessages.InvalidData);
            }
            var entity = new WaterSalesSplit
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                WPipeDiameterId = model.WPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales,
                AverageCapacity = model.AverageCapacity
            };

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.WPipeDiameterId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<WaterSalesSplitDTO>();
                    result.UserTypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
                    result.WPipeDiameterDisplay = (await _constSet.FindAsync(model.WPipeDiameterId)).Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberSales = entity.NumberSales;
                    result.UnitSales = model.UnitSales;
                    result.AverageCapacity = model.AverageCapacity;

                    return ValidationResult<WaterSalesSplitDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<WaterSalesSplitDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


            return ValidationResult<WaterSalesSplitDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDiameterPipeDuplicate,
                                model.UserTypeTitle, model.WPipeDiameterTitle, organizationDisplay)
                );
        }

        public async Task<ValidationResult<WaterSalesSplitDTO>> UpdateAsync(UpdateWaterSalesSplitDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.WPipeDiameterId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.WPipeDiameterId = model.WPipeDiameterId;
                    entity.NumberSales = model.NumberSales;
                    entity.UnitSales = model.UnitSales;
                    entity.AverageCapacity = model.AverageCapacity;
                    entity.WInstallationCosts = model.WInstallationCosts;

                    await _uow.SaveChangesAsync();

                    var result = new WaterSalesSplitDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        WPipeDiameterId = model.WPipeDiameterId,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title,
                        WPipeDiameterDisplay = (await _constSet.FindAsync(model.WPipeDiameterId)).Title,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        NumberSales = model.NumberSales,
                        UnitSales = model.UnitSales,
                        AverageCapacity = model.AverageCapacity,
                        WInstallationCosts = model.WInstallationCosts
                    };

                    return ValidationResult<WaterSalesSplitDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<WaterSalesSplitDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<WaterSalesSplitDTO>.Failed(
               string.Format(ServiceMessages.Logic_UserTypeDiameterPipeDuplicate,
                                model.UserTypeTitle, model.WPipeDiameterTitle, organizationDisplay)
               );
        }
        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckArgumentIsNull(nameof(entity));

            var year = await _yearSet.FindAsync(entity.YearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

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
            var result = new List<CalculationItemData>();
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            result.Add(new CalculationItemData
            {
                Key = "WaterSalesSplit_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[WaterSalesSplit_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WaterSalesSplit_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[WaterSalesSplit_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WaterSalesSplit_Cal3",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[WaterSalesSplit_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WaterSalesSplit_Cal4",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[WaterSalesSplit_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WaterSalesSplit_Cal5",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[WaterSalesSplit_Cal5] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "WaterSalesSplit_Cal6",
                Value = await _uow.ExecuteScalar<int>(
             "[dbo].[WaterSalesSplit_Cal6] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
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

            query = await setFilter(query, filter);

            result.TotalCount = await query.CountAsync();

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query.Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Include(x => x.WPipeDiameter)
                                    .Select(x => new WaterSalesSplitDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WPipeDiameterDisplay = x.WPipeDiameter.Title,
                                        WPipeDiameterId = x.WPipeDiameterId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        AverageCapacity = x.AverageCapacity,
                                        WInstallationCosts = x.WInstallationCosts
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
            var result = new List<WaterSalesSplit>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.UserTypeId, item.WPipeDiameterId))
                        throw new CopyDestYearHasDataException();

                    var entity = new WaterSalesSplit
                    {
                        UserTypeId = item.UserTypeId,
                        WPipeDiameterId = item.WPipeDiameterId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        NumberSales = item.NumberSales,
                        UnitSales = item.UnitSales,
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

        public async Task<Stream> ExportExcelAsync(WaterSalesSplitFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Include(x => x.WPipeDiameter)
                                    .Select(x => new WaterSalesSplitDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WPipeDiameterDisplay = x.WPipeDiameter.Title,
                                        WPipeDiameterId = x.WPipeDiameterId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        AverageCapacity = x.AverageCapacity,
                                        WInstallationCosts = x.WInstallationCosts
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        public async Task<IEnumerable<WaterSalesSplitDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new WaterSalesSplitFilterDTO
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
                                    .Include(x => x.WPipeDiameter)
                                    .Select(x => new WaterSalesSplitDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WPipeDiameterDisplay = x.WPipeDiameter.Title,
                                        WPipeDiameterId = x.WPipeDiameterId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberSales = x.NumberSales,
                                        UnitSales = x.UnitSales,
                                        AverageCapacity = x.AverageCapacity,
                                        WInstallationCosts = x.WInstallationCosts
                                    }).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<WaterSalesSplitImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<WaterSalesSplit>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var usertypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UserType &&
                                                 x.Status != EntityStatus.Deleted);

            var waterdiameters = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__WaterDiameter &&
                                                      x.Status != EntityStatus.Deleted);

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
                if (!await waterdiameters.AnyAsync(x => x.Id == rec.WPipeDiameterId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidDiameterPipe, rowIndex + 2, rec.UserTypeId)
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

            //Start Missing Type
            var missingUserType = new List<Constant>();
            var missingWPDiameters = new List<Constant>();

            string orgTitle = "";
            string userTypeTitle = "";

            foreach (var org in existOrgs)
            {
                if (orgTitle != "")
                    break;
                foreach (var usert in usertypes)
                {
                    var existUserTypeInExcel = records.Any(_ => _.UserTypeId == usert.Id &&
                                                                _.OrganizationId == org.Id);
                    if (!existUserTypeInExcel)
                    {
                        missingUserType.Add(usert);
                        orgTitle = org.Title;
                    }
                    else if (!missingUserType.Any())
                    {
                        foreach (var waterd in waterdiameters)
                        {
                            var existWPDiametersInExcel = records.Any(_ => _.UserTypeId == usert.Id &&
                                                                           _.WPipeDiameterId == waterd.Id &&
                                                                           _.OrganizationId == org.Id);

                            if (!existWPDiametersInExcel)
                            {
                                missingWPDiameters.Add(waterd);
                                orgTitle = org.Title;
                                userTypeTitle = usert.Title;
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

            if (missingWPDiameters.Any())
            {
                string wPDiametersNames = "";
                foreach (var item in missingWPDiameters)
                {
                    wPDiametersNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelDiameterPipeUserTypeOrgNotInExcel, wPDiametersNames, userTypeTitle, orgTitle));
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
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelAccessError, rowIndex + 2)
                        );

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.UserTypeId,
                    record.WPipeDiameterId))
                {

                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelLogicError, rowIndex + 2)
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

        #region Private Helper Methods

        private IQueryable<WaterSalesSplit> setOrder(
            IQueryable<WaterSalesSplit> query,
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

                case "waterdiameter":
                    return desc
                        ? query.OrderByDescending(x => x.WPipeDiameter.DisplayOrder)
                        : query.OrderBy(x => x.WPipeDiameter.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .Include(x => x.WPipeDiameter)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.WPipeDiameter.DisplayOrder);
            }
        }
        private async Task<IQueryable<WaterSalesSplit>> setFilter(
            IQueryable<WaterSalesSplit> query,
            WaterSalesSplitFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = LinqKit.PredicateBuilder.New<WaterSalesSplit>();

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
            if (filter.WPipeDiameterId.HasValue)
                query = query.Where(x => x.WPipeDiameterId == filter.WPipeDiameterId.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                    _.UserType.Title.ToUpper().Contains(filter.Search) ||
                                    _.WPipeDiameter.Title.ToUpper().Contains(filter.Search) ||
                                    _.NumberSales.ToString().Contains(filter.Search) ||
                                    _.UnitSales.ToString().Contains(filter.Search) ||
                                    _.WInstallationCosts.ToString().Contains(filter.Search));
            }
            return query;
        }
        private async Task<IEnumerable<WaterSalesSplit>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId &&
                            _.Status != EntityStatus.Deleted)
                .ToListAsync();
            var result = new List<WaterSalesSplit>();
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
                    var entity = new WaterSalesSplit
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        WPipeDiameterId = item.WPipeDiameterId,
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
        private async Task<IEnumerable<WaterSalesSplit>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId &&
                            _.Status != EntityStatus.Deleted)
                .ToListAsync();
            var result = new List<WaterSalesSplit>();
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
            int userTypeId,
            int wPipeDiameterId,
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
                                                x.WPipeDiameterId == wPipeDiameterId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == userTypeId &&
                                            x.WPipeDiameterId == wPipeDiameterId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}

