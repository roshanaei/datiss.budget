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
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Security;
using Datiss.Budget.Enum;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Common;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{
    public class IncomeCurrentWsHService : IIncomeCurrentWsHService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentWsH> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeCurrentWsHService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentWsH>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentWsH> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentWsH> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeCurrentWsHDTO>> CreateAsync(CreateIncomeCurrentWsHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeCurrentWsH
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                UsageLayerId = model.UsageLayerId,
                NumberUser = model.NumberUser,
                UnitUser = model.UnitUser,
                AvgConsumeUser = model.AvgConsumeUser,
                ConsumptionUser = model.ConsumptionUser,
                Cost = model.Cost,
                Note3Price = model.Note3Price,
                Note3Income = model.Note3Income,
                Income = model.Income,
                SubscriptionIncome = model.SubscriptionIncome,
                SeasonalIncome = model.SeasonalIncome,
                TIncome = model.TIncome,
                Note7Income = model.Note7Income,
                Note7Price = model.Note7Price
            };

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            model.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentWsHDTO>();
                    result.UsageLayerDisplay = model.UsageLayerTitle;
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberUser = model.NumberUser;
                    result.UnitUser = model.UnitUser;
                    result.AvgConsumeUser = model.AvgConsumeUser;
                    result.ConsumptionUser = model.ConsumptionUser;
                    result.Cost = model.Cost;
                    result.Note3Price = model.Note3Price;
                    result.Note3Income = model.Note3Income;
                    result.Income = model.Income;
                    result.SubscriptionIncome = model.SubscriptionIncome;
                    result.SeasonalIncome = model.SeasonalIncome;
                    result.TIncome = model.TIncome;
                    result.Note7Income = model.Note7Income;
                    result.Note7Price = model.Note7Price;

                    return ValidationResult<IncomeCurrentWsHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


            return ValidationResult<IncomeCurrentWsHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                                                model.UserTypeTitle,
                                                model.UsageLayerTitle,
                                                organizationDisplay)
                );
        }

        public async Task<ValidationResult<IncomeCurrentWsHDTO>> UpdateAsync(UpdateIncomeCurrentWsHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            model.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.YearId = model.YearId;
                    entity.OrganizationId = model.OrganizationId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.UsageLayerId = model.UsageLayerId;
                    entity.UnitUser = model.UnitUser;
                    entity.NumberUser = model.NumberUser;
                    entity.UnitUser = model.UnitUser;
                    entity.AvgConsumeUser = model.AvgConsumeUser;
                    entity.ConsumptionUser = model.ConsumptionUser;
                    entity.Cost = model.Cost;
                    entity.Note3Price = model.Note3Price;
                    entity.Note3Income = model.Note3Income;
                    entity.Income = model.Income;
                    entity.SubscriptionIncome = model.SubscriptionIncome;
                    entity.SeasonalIncome = model.SeasonalIncome;
                    entity.TIncome = model.TIncome;
                    entity.Note7Income = model.Note7Income;
                    entity.Note7Price = model.Note7Price;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentWsHDTO
                    {
                        YearId = model.YearId,
                        OrganizationId = model.OrganizationId,
                        UserTypeId = model.UserTypeId,
                        UsageLayerId = model.UsageLayerId,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = model.UserTypeTitle,
                        UsageLayerDisplay = model.UsageLayerTitle,
                        NumberUser = model.NumberUser,
                        UnitUser = model.UnitUser,
                        AvgConsumeUser = model.AvgConsumeUser,
                        ConsumptionUser = model.ConsumptionUser,
                        Cost = model.Cost,
                        Note3Price = model.Note3Price,
                        Note3Income = model.Note3Income,
                        Income = model.Income,
                        SubscriptionIncome = model.SubscriptionIncome,
                        SeasonalIncome = model.SeasonalIncome,
                        TIncome = model.TIncome,
                        Note7Income = model.Note7Income,
                        Note7Price = model.Note7Price,
                    };

                    return ValidationResult<IncomeCurrentWsHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWsHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                                    model.UserTypeTitle,
                                    model.UsageLayerTitle,
                                    organizationDisplay)
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
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };
            var result = new List<CalculationItemData>();

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal1] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal2] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal3",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal3] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal4",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal4] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal5",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal5] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal6",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal6] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal7",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal7] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWsH_Cal8",
                Value = await _uow.ExecuteScalar<int>(
                        "[dbo].[IncomeCurrentWsH_Cal8] @YearId, @OrganizationId",
                        parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<IncomeCurrentWsHDTO>> GetListAsync(IncomeCurrentWsHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<IncomeCurrentWsHDTO>
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
                                        .Select(x => new IncomeCurrentWsHDTO
                                        {
                                            Id = x.Id,
                                            YearId = x.YearId,
                                            Year = x.FinanceYear.Year,
                                            OrganizationId = x.OrganizationId,
                                            OrganizationDisplay = x.Organization.Title,
                                            UserTypeId = x.UserTypeId,
                                            UserTypeDisplay = x.UserType.Title,
                                            UsageLayerId = x.UsageLayerId,
                                            UsageLayerDisplay = x.UsageLayer.Title,
                                            NumberUser = x.NumberUser,
                                            UnitUser = x.UnitUser,
                                            AvgConsumeUser = x.AvgConsumeUser,
                                            ConsumptionUser = x.ConsumptionUser,
                                            Cost = x.Cost,
                                            Note3Price = x.Note3Price,
                                            Note3Income = x.Note3Income,
                                            Income = x.Income,
                                            SubscriptionIncome = x.SubscriptionIncome,
                                            SeasonalIncome = x.SeasonalIncome,
                                            TIncome = x.TIncome,
                                            Note7Income = x.Note7Income,
                                            Note7Price = x.Note7Price
                                        }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopySameYearException();
            if (!await hasAnyDataAsync(sourceOrgId, sourceYearId))
                throw new CopyOrgNullDataException();

            var result = new List<IncomeCurrentWsH>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentWsH
                    {
                        YearId = destYearId,
                        OrganizationId = item.OrganizationId,
                        UserTypeId = item.UserTypeId,
                        UsageLayerId = item.UsageLayerId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                        ConsumptionUser = item.ConsumptionUser,
                        Cost = item.Cost,
                        Note3Price = item.Note3Price,
                        Note3Income = item.Note3Income,
                        Income = item.Income,
                        SubscriptionIncome = item.SubscriptionIncome,
                        SeasonalIncome = item.SeasonalIncome,
                        TIncome = item.TIncome,
                        //Note7Income = item.Note7Income,
                        //Note7Price = item.Note7Price
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
            var data = await _excelService.ImportAsync<IncomeCurrentWsHImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<IncomeCurrentWsH>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var usertypes = _constSet.Where(x => x.Parent.ConstantKey.ToUpper() == ConstantKeys.__UserType.ToUpper() &&
                                                 x.ConstantKey.ToUpper() == ConstantKeys.__House.ToUpper() &&
                                                 x.Status != EntityStatus.Deleted);

            var usagelayer = _constSet.Where(x => x.Parent.ConstantKey.ToUpper() == ConstantKeys.__UsageLayerType.ToUpper() &&
                                                  x.ConstantKey.ToUpper() != ConstantKeys.__UsageLayerType.ToUpper() &&
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
                if (!await usagelayer.AnyAsync(x => x.Id == rec.UsageLayerId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUsageLayer, rowIndex + 2, rec.UserTypeId)
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
            var missingUsageLayer = new List<Constant>();

            string orgTitle = "";
            string userTypeTitle = "";

            foreach (var org in existOrgs)
            {
                if (!string.IsNullOrWhiteSpace(orgTitle))
                {
                    break;
                }
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
                        foreach (var usage in usagelayer)
                        {
                            var existusageLayerInExcel = records.Any(_ => _.UserTypeId == usert.Id &&
                                                                           _.UsageLayerId == usage.Id &&
                                                                           _.OrganizationId == org.Id);

                            if (!existusageLayerInExcel)
                            {
                                missingUsageLayer.Add(usage);
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

            if (missingUsageLayer.Any())
            {
                string wUsageLayerNames = "";
                foreach (var item in missingUsageLayer)
                {
                    wUsageLayerNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUsageLayerUserTypeOrgNotInExcel, wUsageLayerNames, userTypeTitle, orgTitle));
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

        public async Task<IEnumerable<IncomeCurrentWsHDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new IncomeCurrentWsHFilterDTO
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
                                    .Select(x => new IncomeCurrentWsHDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Cost = x.Cost,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        Income = x.Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        TIncome = x.TIncome,
                                        Note7Income = x.Note7Income,
                                        Note7Price = x.Note7Price
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(IncomeCurrentWsHFilterDTO filter)
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
                                    .Select(x => new IncomeCurrentWsHDTO
                                    {
                                        Id = x.Id,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        UsageLayerDisplay = x.UsageLayer.Title,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Cost = x.Cost,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        Income = x.Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        TIncome = x.TIncome,
                                        Note7Income = x.Note7Income,
                                        Note7Price = x.Note7Price
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Privte Helper Methods
        private async Task<IQueryable<IncomeCurrentWsH>> setFilter(
            IQueryable<IncomeCurrentWsH> query,
            IncomeCurrentWsHFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<IncomeCurrentWsH>();

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

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(x => x.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         x.UserType.Title.ToUpper().Contains(filter.Search) ||
                                         x.UsageLayer.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<IncomeCurrentWsH> setOrder(
            IQueryable<IncomeCurrentWsH> query,
            string orderBy = "id",
            bool desc = false)
        {
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
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.UserType.DisplayOrder)
                                .ThenBy(x => x.UsageLayer.DisplayOrder);
            }
        }

        private async Task<IEnumerable<IncomeCurrentWsH>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(x => x.Status != EntityStatus.Deleted &&
                            x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<IncomeCurrentWsH>();

            foreach (var org in children)
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

                foreach (var item in data)
                {
                    if (!await checkLogicAsync(targetYearId, org.Id, item.UserTypeId, item.UsageLayerId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentWsH
                    {
                        UserTypeId = item.UserTypeId,
                        UsageLayerId = item.UsageLayerId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                        ConsumptionUser = item.ConsumptionUser,
                        Cost = item.Cost,
                        Note3Price = item.Note3Price,
                        Note3Income = item.Note3Income,
                        Income = item.Income,
                        SubscriptionIncome = item.SubscriptionIncome,
                        SeasonalIncome = item.SeasonalIncome,
                        TIncome = item.TIncome,
                        Note7Income = item.Note7Income,
                        Note7Price = item.Note7Price
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }

        private async Task<IEnumerable<IncomeCurrentWsH>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<IncomeCurrentWsH>();
            foreach (var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
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
