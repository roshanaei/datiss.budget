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
using System.Data.SqlClient;
using Datiss.Budget.Common;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{
    public class IncomeCurrentWNHService : IIncomeCurrentWNHService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentWNH> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeCurrentWNHService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentWNH>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentWNH> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentWNH> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeCurrentWNHDTO>> CreateAsync(CreateIncomeCurrentWNHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeCurrentWNH
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                NumberUser = model.NumberUser,
                UnitUser = model.UnitUser,
                AvgConsumeUser = model.AvgConsumeUser,
                ConsumptionUser = model.ConsumptionUser,
                Capacity = model.Capacity,
                Cost = model.Cost,
                Income = model.Income,
                ExcessIncome = model.ExcessIncome,
                SeasonalIncome = model.SeasonalIncome,
                Note3Price = model.Note3Price,
                Note3Income = model.Note3Income,
                SubscriptionIncome = model.SubscriptionIncome,
                TotalIncome = model.TotalIncome,
                Diff_ConsWsVolume = model.Diff_ConsWsVolume,
                Note2Income = model.Note2Income,
                WasteVolume = model.WasteVolume,
            };

            var usertypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentWNHDTO>();
                    result.UserTypeDisplay = usertypeDisplay;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberUser = model.NumberUser;
                    result.UnitUser = model.UnitUser;
                    result.AvgConsumeUser = model.AvgConsumeUser;
                    result.ConsumptionUser = model.ConsumptionUser;
                    result.Capacity = model.Capacity;
                    result.Cost = model.Cost;
                    result.Income = model.Income;
                    result.ExcessIncome = model.ExcessIncome;
                    result.SeasonalIncome = model.SeasonalIncome;
                    result.Note3Price = model.Note3Price;
                    result.Note3Income = model.Note3Income;
                    result.SubscriptionIncome = model.SubscriptionIncome;
                    result.TotalIncome = model.TotalIncome;
                    result.Diff_ConsWsVolume = model.Diff_ConsWsVolume;
                    result.Note2Income = model.Note2Income;
                    result.WasteVolume = model.WasteVolume;

                    return ValidationResult<IncomeCurrentWNHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWNHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWNHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                usertypeDisplay, organizationDisplay)
                );


        }

        public async Task<ValidationResult<IncomeCurrentWNHDTO>> UpdateAsync(UpdateIncomeCurrentWNHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var usertypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.NumberUser = model.NumberUser;
                    entity.UnitUser = model.UnitUser;
                    entity.AvgConsumeUser = model.AvgConsumeUser;
                    entity.ConsumptionUser = model.ConsumptionUser;
                    entity.Capacity = model.Capacity;
                    entity.Cost = model.Cost;
                    entity.Income = model.Income;
                    entity.ExcessIncome = model.ExcessIncome;
                    entity.SeasonalIncome = model.SeasonalIncome;
                    entity.Note3Price = model.Note3Price;
                    entity.Note3Income = model.Note3Income;
                    entity.SubscriptionIncome = model.SubscriptionIncome;
                    entity.TotalIncome = model.TotalIncome;
                    entity.Diff_ConsWsVolume = model.Diff_ConsWsVolume;
                    entity.Note2Income = model.Note2Income;
                    entity.WasteVolume = model.WasteVolume;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentWNHDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        NumberUser = model.NumberUser,
                        UnitUser = model.UnitUser,
                        AvgConsumeUser = model.AvgConsumeUser,
                        ConsumptionUser = model.ConsumptionUser,
                        Capacity = model.Capacity,
                        Cost = model.Cost,
                        Income = model.Income,
                        ExcessIncome = model.ExcessIncome,
                        SeasonalIncome = model.SeasonalIncome,
                        Note3Price = model.Note3Price,
                        Note3Income = model.Note3Income,
                        SubscriptionIncome = model.SubscriptionIncome,
                        TotalIncome = model.TotalIncome,
                        Diff_ConsWsVolume = model.Diff_ConsWsVolume,
                        Note2Income = model.Note2Income,
                        WasteVolume = model.WasteVolume,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = usertypeDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<IncomeCurrentWNHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWNHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWNHDTO>.Failed(
                string.Format(ServiceMessages.Logic_DiameterPipeOrgDuplicate,
                usertypeDisplay, organizationDisplay)
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
            var result = new List<CalculationItemData>();
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[IncomeCurrentWNH_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[IncomeCurrentWNH_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal3",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal3] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal4",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal4] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal5",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal5] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal6",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal6] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal7",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal7] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal8",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal8] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal9",
                Value = await _uow.ExecuteScalar<int>(
                                     "[dbo].[IncomeCurrentWNH_Cal9] @YearId, @OrganizationId",
                                     parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeCurrentWNH_Cal9",
                Value = await _uow.ExecuteScalar<int>(
                                 "[dbo].[IncomeCurrentWNH_Cal9] @YearId, @OrganizationId",
                                 parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<IncomeCurrentWNHDTO>> GetListAsync(IncomeCurrentWNHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<IncomeCurrentWNHDTO>
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
                                    .Select(x => new IncomeCurrentWNHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Capacity = x.Capacity,
                                        Cost = x.Cost,
                                        Income = x.Income,
                                        ExcessIncome = x.ExcessIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        TotalIncome = x.TotalIncome,
                                        Diff_ConsWsVolume = x.Diff_ConsWsVolume,
                                        Note2Income = x.Note2Income,
                                        WasteVolume = x.WasteVolume,
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
            var result = new List<IncomeCurrentWNH>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.UserTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentWNH
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                        ConsumptionUser = item.ConsumptionUser,
                        Capacity = item.Capacity,
                        Cost = item.Cost,
                        Income = item.Income,
                        ExcessIncome = item.ExcessIncome,
                        SeasonalIncome = item.SeasonalIncome,
                        Note3Price = item.Note3Price,
                        Note3Income = item.Note3Income,
                        SubscriptionIncome = item.SubscriptionIncome,
                        TotalIncome = item.TotalIncome,
                        Diff_ConsWsVolume = item.Diff_ConsWsVolume,
                        Note2Income = item.Note2Income,
                        WasteVolume = item.WasteVolume,
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
            var data = await _excelService.ImportAsync<IncomeCurrentWNHImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<IncomeCurrentWNH>>();

            int rowIndex = 1;

            var usertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                 x.Parent.ConstantKey == ConstantKeys.__UserType &&
                                                 x.ConstantKey != ConstantKeys.__House);
            var descendents = await _organizationService
                             .GetAllDescendentsAsync(_userContext.OrganizationId);

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
            //Start UserType
            var missingUserType = new List<Constant>();
            string orgTitle = "";
            foreach (var org in existOrgs)
            {
                foreach (var item in usertypes)
                {
                    var existUserTypeInExcel = records.Any(_ => _.UserTypeId == item.Id &&
                                              _.OrganizationId == org.Id);
                    if (!existUserTypeInExcel)
                    {
                        missingUserType.Add(item);
                        orgTitle = org.Title;
                    }

                }
            }
            if (missingUserType.Any())
            {
                string userTypeNames = "";
                foreach (var item in missingUserType)
                {
                    userTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUserTypeOrgNotInExcel, userTypeNames, orgTitle));
            }
            //end

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
                    record.UserTypeId))
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

        public async Task<IEnumerable<IncomeCurrentWNHDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new IncomeCurrentWNHFilterDTO
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
                                    .Select(x => new IncomeCurrentWNHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Capacity = x.Capacity,
                                        Cost = x.Cost,
                                        Income = x.Income,
                                        ExcessIncome = x.ExcessIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        TotalIncome = x.TotalIncome,
                                        Diff_ConsWsVolume = x.Diff_ConsWsVolume,
                                        Note2Income = x.Note2Income,
                                        WasteVolume = x.WasteVolume,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(IncomeCurrentWNHFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new IncomeCurrentWNHDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        AvgConsumeUser = x.AvgConsumeUser,
                                        ConsumptionUser = x.ConsumptionUser,
                                        Capacity = x.Capacity,
                                        Cost = x.Cost,
                                        Income = x.Income,
                                        ExcessIncome = x.ExcessIncome,
                                        SeasonalIncome = x.SeasonalIncome,
                                        Note3Price = x.Note3Price,
                                        Note3Income = x.Note3Income,
                                        SubscriptionIncome = x.SubscriptionIncome,
                                        TotalIncome = x.TotalIncome,
                                        Diff_ConsWsVolume = x.Diff_ConsWsVolume,
                                        Note2Income = x.Note2Income,
                                        WasteVolume = x.WasteVolume,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<IncomeCurrentWNH>> setFilter(
            IQueryable<IncomeCurrentWNH> query,
            IncomeCurrentWNHFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<IncomeCurrentWNH>();

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


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ =>_.UserType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<IncomeCurrentWNH> setOrder(
           IQueryable<IncomeCurrentWNH> query,
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

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.UserType)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.UserType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<IncomeCurrentWNH>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<IncomeCurrentWNH>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.UserTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new IncomeCurrentWNH
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        AvgConsumeUser = item.AvgConsumeUser,
                        ConsumptionUser = item.ConsumptionUser,
                        Capacity = item.Capacity,
                        Cost = item.Cost,
                        Income = item.Income,
                        ExcessIncome = item.ExcessIncome,
                        SeasonalIncome = item.SeasonalIncome,
                        Note3Price = item.Note3Price,
                        Note3Income = item.Note3Income,
                        SubscriptionIncome = item.SubscriptionIncome,
                        TotalIncome = item.TotalIncome,
                        Diff_ConsWsVolume = item.Diff_ConsWsVolume,
                        Note2Income = item.Note2Income,
                        WasteVolume = item.WasteVolume
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<IncomeCurrentWNH>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<IncomeCurrentWNH>();
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
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == userTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
