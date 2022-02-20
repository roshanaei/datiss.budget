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
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class IncomeForcastService : IIncomeForcastService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeForcast> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeForcastService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeForcast>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeForcast> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeForcast> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeForcastDTO>> CreateAsync(CreateIncomeForcastDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeForcast
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                NumberUser = model.NumberUser,
                UnitUser = model.UnitUser,
                WaterBranchIncome = model.WaterBranchIncome,
                WaterInstllIncome = model.WaterInstllIncome,
                WaterNote2Income = model.WaterNote2Income,
                WaterNote3Income = model.WaterNote3Income,
                WNote11Income = model.WNote11Income
            };
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<IncomeForcastDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<IncomeForcastDTO>();
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberUser = entity.NumberUser;
                    result.UnitUser = entity.UnitUser;
                    result.WaterBranchIncome = entity.WaterBranchIncome;
                    result.WaterInstllIncome = entity.WaterInstllIncome;
                    result.WaterNote2Income = entity.WaterNote2Income;
                    result.WaterNote3Income = entity.WaterNote3Income;
                    result.WNote11Income = entity.WNote11Income;

                    return ValidationResult<IncomeForcastDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeForcastDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeForcastDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                                model.UserTypeTitle, organizationDisplay)
                );
        }

        public async Task<ValidationResult<IncomeForcastDTO>> UpdateAsync(UpdateIncomeForcastDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
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
                    entity.WaterBranchIncome = model.WaterBranchIncome;
                    entity.WaterInstllIncome = model.WaterInstllIncome;
                    entity.WaterNote2Income = model.WaterNote2Income;
                    entity.WaterNote3Income = model.WaterNote3Income;
                    entity.WNote11Income = model.WNote11Income;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeForcastDTO
                    {
                        UserTypeDisplay = model.UserTypeTitle,
                        OrganizationDisplay = organizationDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        NumberUser = entity.NumberUser,
                        UnitUser = entity.UnitUser,
                        WaterBranchIncome = entity.WaterBranchIncome,
                        WaterInstllIncome = entity.WaterInstllIncome,
                        WaterNote2Income = entity.WaterNote2Income,
                        WaterNote3Income = entity.WaterNote3Income,
                        WNote11Income = entity.WNote11Income
                    };

                    return ValidationResult<IncomeForcastDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeForcastDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeForcastDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                                model.UserTypeTitle, organizationDisplay)
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
                Key = "IncomeForcast_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                          "[dbo].[IncomeForcast_Cal1] @YearId, @OrganizationId",
                          parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeForcast_Cal2",
                Value = await _uow.ExecuteScalar<int>(
                          "[dbo].[IncomeForcast_Cal2] @YearId, @OrganizationId",
                          parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeForcast_Cal3",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[IncomeForcast_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeForcast_Cal4",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[IncomeForcast_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "IncomeForcast_Cal5",
                Value = await _uow.ExecuteScalar<int>(
                         "[dbo].[IncomeForcast_Cal5] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }


        public async Task<PagedResult<IncomeForcastDTO>> GetListAsync(IncomeForcastFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));
            var result = new PagedResult<IncomeForcastDTO>
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
                                    .Select(x => new IncomeForcastDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        WaterBranchIncome = x.WaterBranchIncome,
                                        WaterInstllIncome = x.WaterInstllIncome,
                                        WaterNote2Income = x.WaterNote2Income,
                                        WaterNote3Income = x.WaterNote3Income,
                                        WNote11Income = x.WNote11Income
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
            var result = new List<IncomeForcast>();

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

                    var entity = new IncomeForcast
                    {
                        YearId = item.YearId,
                        OrganizationId = item.OrganizationId,
                        UserTypeId = item.UserTypeId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        WaterBranchIncome = item.WaterBranchIncome,
                        WaterInstllIncome = item.WaterInstllIncome,
                        WaterNote2Income = item.WaterNote2Income,
                        WaterNote3Income = item.WaterNote3Income,
                        WNote11Income = item.WNote11Income
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
            var data = await _excelService.ImportAsync<IncomeForcastImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<IncomeForcast>>();

            int rowIndex = 1;

            var usertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted && 
                                                 x.Parent.ConstantKey == ConstantKeys.__UserType);

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


        public async Task<IEnumerable<IncomeForcastDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new IncomeForcastFilterDTO
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
                                    .Select(x => new IncomeForcastDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        WaterBranchIncome = x.WaterBranchIncome,
                                        WaterInstllIncome = x.WaterInstllIncome,
                                        WaterNote2Income = x.WaterNote2Income,
                                        WaterNote3Income = x.WaterNote3Income,
                                        WNote11Income = x.WNote11Income
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(IncomeForcastFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new IncomeForcastDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        NumberUser = x.NumberUser,
                                        UnitUser = x.UnitUser,
                                        WaterBranchIncome = x.WaterBranchIncome,
                                        WaterInstllIncome = x.WaterInstllIncome,
                                        WaterNote2Income = x.WaterNote2Income,
                                        WaterNote3Income = x.WaterNote3Income,
                                        WNote11Income = x.WNote11Income
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<IncomeForcast>> setFilter(
            IQueryable<IncomeForcast> query,
            IncomeForcastFilterDTO filter)
        {

            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<IncomeForcast>();

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
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) || 
                                         _.UserType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<IncomeForcast> setOrder(
           IQueryable<IncomeForcast> query,
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

        private async Task<IEnumerable<IncomeForcast>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted && 
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<IncomeForcast>();

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
                    var entity = new IncomeForcast
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        NumberUser = item.NumberUser,
                        UnitUser = item.UnitUser,
                        WaterBranchIncome = item.WaterBranchIncome,
                        WaterInstllIncome = item.WaterInstllIncome,
                        WaterNote2Income = item.WaterNote2Income,
                        WaterNote3Income = item.WaterNote3Income,
                        WNote11Income = item.WNote11Income
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<IncomeForcast>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<IncomeForcast>();
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
            int UserTypeId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == UserTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == UserTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
