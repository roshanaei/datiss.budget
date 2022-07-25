using System;
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

    public class CostForcastBuyService : ICostForcastBuyService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostForcastBuy> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastBuyService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastBuy>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostForcastBuy> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastBuy> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastBuyDTO>> CreateAsync(CreateCostForcastBuyDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = model.Adapt<CostForcastBuy>();

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId))?.Title;

            try
            {
                await _dbSet.AddAsync(entity);
                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastBuyDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }
                var result = entity.Adapt<CostForcastBuyDTO>();

                result.CreditTypeDisplay = (await _constSet.FindAsync(model.CreditTypeId))?.Title;
                result.AssetTypeDisplay = (await _constSet.FindAsync(model.AssetTypeId))?.Title;
                result.AssetDetailTypeDisplay = (await _constSet.FindAsync(model.AssetDetailTypeId))?.Title;
                result.CostCenterTypeDisplay = (await _constSet.FindAsync(model.CostCenterTypeId))?.Title;
                result.BuyDepartmentDisplay = (await _constSet.FindAsync(model.BuyDepartmentId))?.Title;
                result.MeasurementTypeDisplay = (await _constSet.FindAsync(model.MeasurementTypeId))?.Title;
                result.LocationDisplay = (await _orgDbSet.FindAsync(model.LocationId))?.Title;
                result.OrganizationDisplay = organizationDisplay;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastBuyDTO>.Success(result);
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastBuyDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


        }

        public async Task<ValidationResult<CostForcastBuyDTO>> UpdateAsync(UpdateCostForcastBuyDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId))?.Title;

            try
            {
                var entity = await _dbSet.FindAsync(model.Id);

                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.BuyDescription = model.BuyDescription;
                entity.LocationId = model.LocationId;
                entity.BuyDepartmentId = model.BuyDepartmentId;
                entity.CostCenterTypeId = model.CostCenterTypeId;
                entity.AssetTypeId = model.AssetTypeId;
                entity.AssetDetailTypeId = model.AssetDetailTypeId;
                entity.Amount = model.Amount;
                entity.MeasurementTypeId = model.MeasurementTypeId;
                entity.UnitPrice = model.UnitPrice;
                entity.CreditTypeId = model.CreditTypeId;
                entity.ProposedCost = model.ProposedCost;

                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastBuyDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }

                var result = entity.Adapt<CostForcastBuyDTO>();
                result.CreditTypeDisplay = (await _constSet.FindAsync(model.CreditTypeId))?.Title;
                result.AssetTypeDisplay = (await _constSet.FindAsync(model.AssetTypeId))?.Title;
                result.AssetDetailTypeDisplay = (await _constSet.FindAsync(model.AssetDetailTypeId))?.Title;
                result.CostCenterTypeDisplay = (await _constSet.FindAsync(model.CostCenterTypeId))?.Title;
                result.BuyDepartmentDisplay = (await _constSet.FindAsync(model.BuyDepartmentId))?.Title;
                result.MeasurementTypeDisplay = (await _constSet.FindAsync(model.MeasurementTypeId))?.Title;
                result.LocationDisplay = (await _orgDbSet.FindAsync(model.LocationId))?.Title;
                result.OrganizationDisplay = organizationDisplay;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastBuyDTO>.Success(result);

            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastBuyDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

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

            IEnumerable<CostForcastBuy> childrens = new CostForcastBuy[] { };

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
                Key = "CostForcastBuy_Cal1",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostForcastBuy_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostForcastBuyDTO>> GetListAsync(CostForcastBuyFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastBuyDTO>
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
                                    .Include(x => x.Location)
                                    .Include(x => x.Department)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.Asset)
                                    .Include(x => x.AssetDetail)
                                    .Include(x => x.Measurement)
                                    .Include(x => x.Credit)
                                    .Select(x => x.Adapt<CostForcastBuyDTO>())
                                    .ToListAsync();

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
            var result = new List<CostForcastBuy>();

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
                    item.YearId = destYearId;
                    item.Id = 0;
                    var entity = item.Adapt<CostForcastBuy>();

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
            var data = await _excelService.ImportAsync<CostForcastBuyImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 26);

            var rawRecords = data.Adapt<List<CostForcastBuy>>();
            List<CostForcastBuy> records = new List<CostForcastBuy>();


            foreach (var item in rawRecords)
            {
                if (item.OrganizationId != 0)
                    records.Add(item);
            }

            int rowIndex = 27;

            if (!records.Any())
            {
                return ImportResult.Failed(
                    string.Format(ServiceMessages.EmptyExcel)
                    );
            }


            var buyDepartmentTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                          x.Parent.ConstantKey == ConstantKeys.__BuyDepartmentType);

            var costCenterTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                x.Parent.ConstantKey == ConstantKeys.__CostCenterType);

            var assetTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                  x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectType);

            var credittypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__CreditType);

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
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex, rec.YearId)
                        );
                }
                if (org == null)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rowIndex, rec.OrganizationId)
                        );
                }
                if (!await _orgDbSet.AnyAsync(x => x.Id == rec.LocationId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotExistOrg, rowIndex, rec.LocationId)
                        );
                }
                if (!await buyDepartmentTypes.AnyAsync(x => x.Id == rec.BuyDepartmentId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.BuyDepartmentId)
                        );
                }
                if (!await costCenterTypes.AnyAsync(x => x.Id == rec.CostCenterTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidCostCenterType, rowIndex, rec.CostCenterTypeId)
                        );
                }
                if (!await credittypes.AnyAsync(x => x.Id == rec.CreditTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.CreditTypeId)
                        );
                }
                if (!await assetTypes.AnyAsync(x => x.Id == rec.AssetTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.AssetTypeId)
                        );
                }

                var asset = await _constSet.FindAsync(rec.AssetTypeId);

                var assetDetailTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                         x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectDetailType &&
                                         x.ConstantKey.Contains(asset.ConstantKey.Split(new char[] { '.', '.' })[1]));
                if (!assetDetailTypes.Any())
                    assetDetailTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                             x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectDetailType &&
                                                             x.ConstantKey.Contains("Dash"));

                if (!assetDetailTypes.Any(x => x.Id == rec.AssetDetailTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.AssetDetailTypeId)
                        );
                }

                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex)
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

            rowIndex = 2;

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
                        string.Format(ServiceMessages.ImportExcelAccessError, rowIndex)
                        );

                record.MeasurementTypeId = 295;

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

        public async Task<IEnumerable<CostForcastBuyDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostForcastBuyFilterDTO
            {
                OrganizationId = organizationId,
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query.Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.Location)
                                    .Include(x => x.Department)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.Asset)
                                    .Include(x => x.AssetDetail)
                                    .Include(x => x.Measurement)
                                    .Select(x => x.Adapt<CostForcastBuyDTO>())
                                    .ToListAsync();
            return items;
        }

        #region Private Helper Methods

        private async Task<IQueryable<CostForcastBuy>> setFilter(
            IQueryable<CostForcastBuy> query,
            CostForcastBuyFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastBuy>();

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

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Location.Title.ToUpper().Contains(filter.Search) ||
                                         _.BuyDescription.ToUpper().Contains(filter.Search) ||
                                         _.AssetDetail.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostForcastBuy> setOrder(
           IQueryable<CostForcastBuy> query,
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
                    return query.Include(x => x.FinanceYear)
                                .Include(x => x.Organization)
                                .Include(x => x.Location)
                                .Include(x => x.Department)
                                .Include(x => x.CostCenter)
                                .Include(x => x.Asset)
                                .Include(x => x.AssetDetail)
                                .Include(x => x.Measurement)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.Location.DisplayOrder)
                                .ThenBy(x => x.Location.RowOrder)
                                .ThenBy(x => x.Department.DisplayOrder)
                                .ThenBy(x => x.CostCenter.DisplayOrder)
                                .ThenBy(x => x.Credit.DisplayOrder)
                                .ThenBy(x => x.Asset.DisplayOrder)
                                .ThenBy(x => x.AssetDetail.DisplayOrder)
                                .ThenBy(x => x.Measurement.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostForcastBuy>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostForcastBuy>();

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
                    item.YearId = targetYearId;
                    item.Id = 0;
                    var entity = item.Adapt<CostForcastBuy>();

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<CostForcastBuy>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostForcastBuy>();
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
    }
}