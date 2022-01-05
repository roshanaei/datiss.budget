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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class BranchingRateIncreaseService : IBranchingRateIncreaseService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<BranchingRateIncrease> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public BranchingRateIncreaseService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<BranchingRateIncrease>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<BranchingRateIncrease> Query()
            => _dbSet.AsNoTracking();

        public async Task<BranchingRateIncrease> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<BranchingRateIncreaseDTO>> CreateAsync(CreateBranchingRateIncreaseDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new BranchingRateIncrease
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                WaterRateIncrease = model.WaterRateIncrease,
                WasteRateIncrease = model.WasteRateIncrease,
                WastePersentIncrease = model.WastePersentIncrease,
                FixAmountBusiness = model.FixAmountBusiness,
                CapacityFixAmount = model.CapacityFixAmount,
                WaterInstallRateIncrease = model.WaterInstallRateIncrease,
                WsInstalIncrease = model.WsInstalIncrease,
                WaterFixNote2 = model.WaterFixNote2,
                WasteFixNote2 = model.WasteFixNote2
            };
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<BranchingRateIncreaseDTO>();
                    result.UserTypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
                    result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.WaterRateIncrease = entity.WaterRateIncrease;
                    result.WasteRateIncrease = entity.WasteRateIncrease;
                    result.WastePersentIncrease = entity.WastePersentIncrease;
                    result.FixAmountBusiness = entity.FixAmountBusiness;
                    result.CapacityFixAmount = entity.CapacityFixAmount;
                    result.WaterInstallRateIncrease = entity.WaterInstallRateIncrease;
                    result.WsInstalIncrease = entity.WsInstalIncrease;
                    result.WaterFixNote2 = entity.WaterFixNote2;
                    result.WasteFixNote2 = entity.WasteFixNote2;

                    return ValidationResult<BranchingRateIncreaseDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<BranchingRateIncreaseDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<BranchingRateIncreaseDTO>.Failed(
                string.Format(ServiceMessages.Logic_BranchingRateIncrease,
                model.UserTypeTitle)
                );


        }

        public async Task<ValidationResult<BranchingRateIncreaseDTO>> UpdateAsync(UpdateBranchingRateIncreaseDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.WaterRateIncrease = model.WaterRateIncrease;
                    entity.WasteRateIncrease = model.WasteRateIncrease;
                    entity.WastePersentIncrease = model.WastePersentIncrease;
                    entity.FixAmountBusiness = model.FixAmountBusiness;
                    entity.CapacityFixAmount = model.CapacityFixAmount;
                    entity.WaterInstallRateIncrease = model.WaterInstallRateIncrease;
                    entity.WsInstalIncrease = model.WsInstalIncrease;
                    entity.WaterFixNote2 = model.WaterFixNote2;
                    entity.WasteFixNote2 = model.WasteFixNote2;

                    await _uow.SaveChangesAsync();

                    var result = new BranchingRateIncreaseDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                        UserTypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        WaterRateIncrease = model.WaterRateIncrease,
                        WasteRateIncrease = model.WasteRateIncrease,
                        WastePersentIncrease = model.WastePersentIncrease,
                        FixAmountBusiness = model.FixAmountBusiness,
                        CapacityFixAmount = model.CapacityFixAmount,
                        WaterInstallRateIncrease = model.WaterInstallRateIncrease,
                        WsInstalIncrease = model.WsInstalIncrease,
                        WaterFixNote2 = model.WaterFixNote2,
                        WasteFixNote2 = model.WasteFixNote2
                    };

                    return ValidationResult<BranchingRateIncreaseDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<BranchingRateIncreaseDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }
            return ValidationResult<BranchingRateIncreaseDTO>.Failed(
                string.Format(ServiceMessages.Logic_BranchingRateIncrease,
                model.UserTypeTitle)
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
        public async Task<PagedResult<BranchingRateIncreaseDTO>> GetListAsync(BranchingRateIncreaseFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<BranchingRateIncreaseDTO>
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
                                    .Select(x => new BranchingRateIncreaseDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        WaterRateIncrease = x.WaterRateIncrease,
                                        WasteRateIncrease = x.WasteRateIncrease,
                                        WastePersentIncrease = x.WastePersentIncrease,
                                        FixAmountBusiness = x.FixAmountBusiness,
                                        CapacityFixAmount = x.CapacityFixAmount,
                                        WaterInstallRateIncrease = x.WaterInstallRateIncrease,
                                        WsInstalIncrease = x.WsInstalIncrease,
                                        WaterFixNote2 = x.WaterFixNote2,
                                        WasteFixNote2 = x.WasteFixNote2
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
            var result = new List<BranchingRateIncrease>();

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

                    var entity = new BranchingRateIncrease
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        WaterRateIncrease = item.WaterRateIncrease,
                        WasteRateIncrease = item.WasteRateIncrease,
                        WastePersentIncrease = item.WastePersentIncrease,
                        FixAmountBusiness = item.FixAmountBusiness,
                        CapacityFixAmount = item.CapacityFixAmount,
                        WaterInstallRateIncrease = item.WaterInstallRateIncrease,
                        WsInstalIncrease = item.WsInstalIncrease,
                        WaterFixNote2 = item.WaterFixNote2,
                        WasteFixNote2 = item.WasteFixNote2
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
            var data = await _excelService.ImportAsync<BranchingRateIncreaseImportModel>
                (fileInfo);

            var records = data.Adapt<List<BranchingRateIncrease>>();

            int rowIndex = 1;

            var usertypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__UserType);


            foreach (var rec in records)
            {
                var org = await _orgDbSet.FindAsync(rec.OrganizationId);
                var year = await _yearSet.FindAsync(rec.YearId);
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
                if (org.Type != Enum.OrganizationType.City && org.Type != Enum.OrganizationType.Village)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelNotAllowedOrg, org.Title, rowIndex + 1)
                        );
                }

                rowIndex++;
            }


            //Start UserType
            var missingDWType = new List<Constant>();
            foreach (var item in usertypes)
            {
                var existDWTypeInExcel = records.Any(_ => _.UserTypeId == item.Id);
                if (!existDWTypeInExcel)
                    missingDWType.Add(item);

            }
            if (missingDWType.Any())
            {
                string userTypeNames = "";
                foreach (var item in missingDWType)
                {
                    userTypeNames += "- " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelDWTypeNotInExcel, userTypeNames));
            }
            //end

            rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            if (!continueIfAnyOrgMissing)
            {
                var missingOrgs = new List<Organization>();

                foreach (var item in descendents)
                {
                    var existInExcel = records.Any(_ => _.OrganizationId == item.Id);
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
                    record.UserTypeId))
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

        public async Task<IEnumerable<BranchingRateIncreaseDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new BranchingRateIncreaseFilterDTO
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
                                    .Select(x => new BranchingRateIncreaseDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WaterRateIncrease = x.WaterRateIncrease,
                                        WasteRateIncrease = x.WasteRateIncrease,
                                        WastePersentIncrease = x.WastePersentIncrease,
                                        FixAmountBusiness = x.FixAmountBusiness,
                                        CapacityFixAmount = x.CapacityFixAmount,
                                        WaterInstallRateIncrease = x.WaterInstallRateIncrease,
                                        WsInstalIncrease = x.WsInstalIncrease,
                                        WaterFixNote2 = x.WaterFixNote2,
                                        WasteFixNote2 = x.WasteFixNote2,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(BranchingRateIncreaseFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Include(x => x.UserType)
                                    .Select(x => new BranchingRateIncreaseDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        WaterRateIncrease = x.WaterRateIncrease,
                                        WasteRateIncrease = x.WasteRateIncrease,
                                        WastePersentIncrease = x.WastePersentIncrease,
                                        FixAmountBusiness = x.FixAmountBusiness,
                                        CapacityFixAmount = x.CapacityFixAmount,
                                        WaterInstallRateIncrease = x.WaterInstallRateIncrease,
                                        WsInstalIncrease = x.WsInstalIncrease,
                                        WaterFixNote2 = x.WaterFixNote2,
                                        WasteFixNote2 = x.WasteFixNote2,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }


        #region Private Helper Methods

        private async Task<IQueryable<BranchingRateIncrease>> setFilter(
            IQueryable<BranchingRateIncrease> query,
            BranchingRateIncreaseFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<BranchingRateIncrease>();

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

        private IQueryable<BranchingRateIncrease> setOrder(
           IQueryable<BranchingRateIncrease> query,
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
                                .ThenBy(x => x.UserType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<BranchingRateIncrease>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<BranchingRateIncrease>();

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

                    var entity = new BranchingRateIncrease
                    {
                        UserTypeId = item.UserTypeId,
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        WaterRateIncrease = item.WaterRateIncrease,
                        WasteRateIncrease = item.WasteRateIncrease,
                        WastePersentIncrease = item.WastePersentIncrease,
                        FixAmountBusiness = item.FixAmountBusiness,
                        CapacityFixAmount = item.CapacityFixAmount,
                        WaterInstallRateIncrease = item.WaterInstallRateIncrease,
                        WsInstalIncrease = item.WsInstalIncrease,
                        WaterFixNote2 = item.WaterFixNote2,
                        WasteFixNote2 = item.WasteFixNote2,
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<BranchingRateIncrease>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<BranchingRateIncrease>();
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
                if (await Query().Include(x => x.Organization)
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
