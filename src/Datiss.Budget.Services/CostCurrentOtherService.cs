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
    public class CostCurrentOtherService : ICostCurrentOtherService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostCurrentOther> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostCurrentOtherService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentOther>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentOther> Query()
              => _dbSet.AsNoTracking();

        public async Task<CostCurrentOther> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentOtherDTO>> CreateAsync(CreateCostCurrentOtherDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new CostCurrentOther
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                CostCenterTypeId = model.CostCenterTypeId,
                CCOtherCostsTypeId = model.CCOtherCostsTypeId,
                BaseFee = model.BaseFee,
                LastYearFee = model.LastYearFee,
            };

            model.CostCenterTypeTitle = (await _constSet.FindAsync(model.CostCenterTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.CostCenterTypeId, model.CCOtherCostsTypeId))
                {
                    await _dbSet.AddAsync(entity);

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentOtherDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<CostCurrentOtherDTO>();
                    result.CostCenterTypeDisplay = (await _constSet.FindAsync(model.CostCenterTypeId)).Title;
                    result.CCOtherCostsTypeDisplay = (await _constSet.FindAsync(model.CCOtherCostsTypeId)).Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.BaseFee = entity.BaseFee;
                    result.LastYearFee = model.LastYearFee;

                    return ValidationResult<CostCurrentOtherDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentOtherDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


            return ValidationResult<CostCurrentOtherDTO>.Failed(
                string.Format(ServiceMessages.Logic_CostCenterTypeCCOtherCostsTypeDuplicate,
                                model.CostCenterTypeTitle, model.CCOtherCostsTypeTitle, organizationDisplay)
                );
        }

        public async Task<ValidationResult<CostCurrentOtherDTO>> UpdateAsync(UpdateCostCurrentOtherDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.CostCenterTypeTitle = (await _constSet.FindAsync(model.CostCenterTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.CostCenterTypeId, model.CCOtherCostsTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.CostCenterTypeId = model.CostCenterTypeId;
                    entity.CCOtherCostsTypeId = model.CCOtherCostsTypeId;
                    entity.BaseFee = model.BaseFee;
                    entity.LastYearFee = model.LastYearFee;
                    entity.ForcastFee = model.ForcastFee;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentOtherDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = new CostCurrentOtherDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        CostCenterTypeId = model.CostCenterTypeId,
                        CCOtherCostsTypeId = model.CCOtherCostsTypeId,
                        OrganizationDisplay = organizationDisplay,
                        CostCenterTypeDisplay = (await _constSet.FindAsync(model.CostCenterTypeId)).Title,
                        CCOtherCostsTypeDisplay = (await _constSet.FindAsync(model.CCOtherCostsTypeId)).Title,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        BaseFee = model.BaseFee,
                        LastYearFee = model.LastYearFee,
                        ForcastFee = model.ForcastFee
                    };

                    return ValidationResult<CostCurrentOtherDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentOtherDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentOtherDTO>.Failed(
               string.Format(ServiceMessages.Logic_CostCenterTypeCCOtherCostsTypeDuplicate,
                                model.CostCenterTypeTitle, model.CCOtherCostsTypeTitle, organizationDisplay)
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

            IEnumerable<CostCurrentOther> childrens = new CostCurrentOther[] { };

            if (organization.Type == OrganizationType.County || organization.Type == OrganizationType.Root)
            {
                childrens = await getChildren(organizationId, yearId);
            }

            if (self.Count() == 0 && childrens.Count() == 0)
                throw new DeleteNullRecordException();

            _dbSet.RemoveRange(self);

            if (childrens.Any())
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
                Key = "CostCurrentOther_Cal1",
                Value = await _uow.ExecuteScalar<int>(
                                    "[dbo].[CostCurrentOther_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostCurrentOtherDTO>> GetListAsync(CostCurrentOtherFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentOtherDTO>
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
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.CCOtherCosts)
                                    .Select(x => new CostCurrentOtherDTO
                                    {
                                        Id = x.Id,
                                        CostCenterTypeDisplay = x.CostCenter.Title,
                                        CostCenterTypeId = x.CostCenterTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        CCOtherCostsTypeDisplay = x.CCOtherCosts.Title,
                                        CCOtherCostsTypeId = x.CCOtherCostsTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        BaseFee = x.BaseFee,
                                        LastYearFee = x.LastYearFee,
                                        ForcastFee = x.ForcastFee
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
            var result = new List<CostCurrentOther>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.CostCenterTypeId, item.CCOtherCostsTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentOther
                    {
                        CostCenterTypeId = item.CostCenterTypeId,
                        CCOtherCostsTypeId = item.CCOtherCostsTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        BaseFee = item.BaseFee,
                        LastYearFee = item.LastYearFee
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

        public async Task<Stream> ExportExcelAsync(CostCurrentOtherFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.CCOtherCosts)
                                    .Select(x => new CostCurrentOtherDTO
                                    {
                                        Id = x.Id,
                                        CostCenterTypeDisplay = x.CostCenter.Title,
                                        CostCenterTypeId = x.CostCenterTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        CCOtherCostsTypeDisplay = x.CCOtherCosts.Title,
                                        CCOtherCostsTypeId = x.CCOtherCostsTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        BaseFee = x.BaseFee,
                                        LastYearFee = x.LastYearFee,
                                        ForcastFee = x.ForcastFee
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        public async Task<IEnumerable<CostCurrentOtherDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostCurrentOtherFilterDTO
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
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.CCOtherCosts)
                                    .Select(x => new CostCurrentOtherDTO
                                    {
                                        Id = x.Id,
                                        CostCenterTypeDisplay = x.CostCenter.Title,
                                        CostCenterTypeId = x.CostCenterTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        CCOtherCostsTypeDisplay = x.CCOtherCosts.Title,
                                        CCOtherCostsTypeId = x.CCOtherCostsTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        BaseFee = x.BaseFee,
                                        LastYearFee = x.LastYearFee,
                                        ForcastFee = x.ForcastFee
                                    }).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostCurrentOtherImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentOther>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var costCenterTypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CostCenterType &&
                                                 x.Status != EntityStatus.Deleted);

            var ccOtherCostsType = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CCOtherCostsType &&
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
                if (!await costCenterTypes.AnyAsync(x => x.Id == rec.CostCenterTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidCostCenterType, rowIndex + 2, rec.CostCenterTypeId)
                        );
                }
                if (!await ccOtherCostsType.AnyAsync(x => x.Id == rec.CCOtherCostsTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.CostCenterTypeId)
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
            var missingCostCenterType = new List<Constant>();
            var missingCCOtherTypes = new List<Constant>();

            string orgTitle = "";
            string costCenterTypeTitle = "";

            foreach (var org in existOrgs)
            {
                if (!string.IsNullOrWhiteSpace(orgTitle))
                    break;
                foreach (var usert in costCenterTypes)
                {
                    if (!string.IsNullOrWhiteSpace(costCenterTypeTitle))
                        break;
                    var existCostCenterTypeInExcel = records.Any(_ => _.CostCenterTypeId == usert.Id &&
                                                                _.OrganizationId == org.Id);
                    if (!existCostCenterTypeInExcel)
                    {
                        missingCostCenterType.Add(usert);
                        orgTitle = org.Title;
                    }
                    else if (!missingCostCenterType.Any())
                    {
                        foreach (var waterd in ccOtherCostsType)
                        {
                            var existCCOthersInExcel = records.Any(_ => _.CostCenterTypeId == usert.Id &&
                                                                           _.CCOtherCostsTypeId == waterd.Id &&
                                                                           _.OrganizationId == org.Id);

                            if (!existCCOthersInExcel)
                            {
                                missingCCOtherTypes.Add(waterd);
                                orgTitle = org.Title;
                                costCenterTypeTitle = usert.Title;
                            }
                        }
                    }
                }
            }

            if (missingCostCenterType.Any())
            {
                string costCenterTypeNames = "";
                foreach (var item in missingCostCenterType)
                {
                    costCenterTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelCostCenterTypeOrgNotInExcel, costCenterTypeNames, orgTitle));
            }

            if (missingCCOtherTypes.Any())
            {
                string ccOthersNames = "";
                foreach (var item in missingCCOtherTypes)
                {
                    ccOthersNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelCCOtherCostsTypeCostCenterTypeOrgNotInExcel, ccOthersNames, costCenterTypeTitle, orgTitle));
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
                    record.CostCenterTypeId,
                    record.CCOtherCostsTypeId))
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

        #region Private Helper Methods

        private IQueryable<CostCurrentOther> setOrder(
            IQueryable<CostCurrentOther> query,
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

                case "costcentertype":
                    return desc
                        ? query.OrderByDescending(x => x.CostCenter.DisplayOrder)
                        : query.OrderBy(x => x.CostCenter.DisplayOrder);

                case "ccothercoststype":
                    return desc
                        ? query.OrderByDescending(x => x.CCOtherCosts.DisplayOrder)
                        : query.OrderBy(x => x.CCOtherCosts.DisplayOrder);

                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.CostCenter)
                                .Include(x => x.CCOtherCosts)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.CostCenter.DisplayOrder)
                                .ThenBy(x => x.CCOtherCosts.DisplayOrder);
            }
        }
        private async Task<IQueryable<CostCurrentOther>> setFilter(
            IQueryable<CostCurrentOther> query,
            CostCurrentOtherFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = LinqKit.PredicateBuilder.New<CostCurrentOther>();

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
            if (filter.CostCenterTypeId.HasValue)
                query = query.Where(x => x.CostCenterTypeId == filter.CostCenterTypeId.Value);
            if (filter.CCOtherCostsTypeId.HasValue)
                query = query.Where(x => x.CCOtherCostsTypeId == filter.CCOtherCostsTypeId.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Organization.Title.ToUpper().Contains(filter.Search) ||
                                         _.CostCenter.Title.ToUpper().Contains(filter.Search) ||
                                         _.CCOtherCosts.Title.ToUpper().Contains(filter.Search));
            }
            return query;
        }
        private async Task<IEnumerable<CostCurrentOther>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId &&
                            _.Status != EntityStatus.Deleted)
                .ToListAsync();
            var result = new List<CostCurrentOther>();
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
                    var entity = new CostCurrentOther
                    {
                        CostCenterTypeId = item.CostCenterTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        CCOtherCostsTypeId = item.CCOtherCostsTypeId,
                        LastYearFee = item.LastYearFee,
                        BaseFee = item.BaseFee,
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<CostCurrentOther>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostCurrentOther>();
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
            int costCenterTypeId,
            int ccOtherCostsTypeId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.CostCenterTypeId == costCenterTypeId &&
                                                x.CCOtherCostsTypeId == ccOtherCostsTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.CostCenterTypeId == costCenterTypeId &&
                                            x.CCOtherCostsTypeId == ccOtherCostsTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}

