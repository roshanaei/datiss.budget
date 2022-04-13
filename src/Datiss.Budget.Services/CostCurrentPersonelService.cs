using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Services
{
    public class CostCurrentPersonelService : ICostCurrentPersonelService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostCurrentPersonel> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostCurrentPersonelService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentPersonel>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentPersonel> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostCurrentPersonel> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentPersonelDTO>> UpdateAsync(UpdateCostCurrentPersonelDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                if (await checkLogicAsync(model.YearId, model.PersonelCode, model.RecordType, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.PersonelCode = model.PersonelCode;
                    entity.RecordType = model.RecordType;
                    entity.FirstName = model.FirstName;
                    entity.LastName = model.LastName;
                    entity.GenderId = model.GenderId;
                    entity.GradeTypeId = model.GradeTypeId;
                    entity.ContractTypeId = model.ContractTypeId;
                    entity.JobDepartmentTypeId = model.JobDepartmentTypeId;
                    entity.CostCenterTypeId = model.CostCenterTypeId;
                    entity.JobStatusTypeId = model.JobStatusTypeId;
                    entity.JobStatusDetailTypeId = model.JobStatusDetailTypeId;
                    entity.ExperienceYear = model.ExperienceYear;
                    entity.ExperienceMonth = model.ExperienceMonth;
                    entity.FixSalary = model.FixSalary;
                    entity.EmployRight = model.EmployRight;
                    entity.RegionRight = model.RegionRight;
                    entity.OverTimeValue = model.OverTimeValue;
                    entity.OverTimeCost = model.OverTimeCost;
                    entity.HolidayValue = model.HolidayValue;
                    entity.HolidayCost = model.HolidayCost;
                    entity.ShiftPercent = model.ShiftPercent;
                    entity.ShiftPCost = model.ShiftPCost;
                    entity.MissionCount = model.MissionCount;
                    entity.MissionDayCost = model.MissionDayCost;
                    entity.HardWorkingRt = model.HardWorkingRt;
                    entity.TrafficRt = model.TrafficRt;
                    entity.HouseRt = model.HouseRt;
                    entity.ChildRt = model.ChildRt;
                    entity.StuffRt = model.StuffRt;
                    entity.Education = model.Education;
                    entity.InsuranceMaster = model.InsuranceMaster;
                    entity.InsuranceAging = model.InsuranceAging;
                    entity.HolidayYearly = model.HolidayYearly;
                    entity.MilitaryServiceCost = model.MilitaryServiceCost;
                    entity.UnUseHolidayCount = model.UnUseHolidayCount;
                    entity.WelfareCost = model.WelfareCost;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentPersonelDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<CostCurrentPersonelDTO>();

                    result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId))?.Title;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.GradeTypeDisplay = (await _constSet.FindAsync(model.GradeTypeId))?.Title;
                    result.ContractTypeDisplay = (await _constSet.FindAsync(model.ContractTypeId))?.Title;
                    result.JobDepartmentTypeDisplay = (await _constSet.FindAsync(model.JobDepartmentTypeId))?.Title;
                    result.CostCenterTypeDisplay = (await _constSet.FindAsync(model.CostCenterTypeId))?.Title;
                    result.JobStatusTypeDisplay = (await _constSet.FindAsync(model.JobStatusTypeId))?.Title;
                    result.JobStatusDetailTypeDisplay = (await _constSet.FindAsync(model.JobStatusDetailTypeId))?.Title;
                    result.RecordTypeDispaly = model.RecordType == RecordType.Base ? EnumText.RecordType_Base : EnumText.RecordType_Forcast;

                    return ValidationResult<CostCurrentPersonelDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentPersonelDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentPersonelDTO>.Failed(
                string.Format(ServiceMessages.Exist_Username,
                model.FirstName + " " + model.LastName)
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

        public async Task<OrganizationDeleteDataResult> HardDeleteAsync(int yearId, int organizationId, RecordType recordType)
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

            IEnumerable<CostCurrentPersonel> childrens = new CostCurrentPersonel[] { };

            if (organization.Type == OrganizationType.County || organization.Type == OrganizationType.Root)
            {
                childrens = await getChildren(organizationId, yearId , recordType);
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

        public async Task<PagedResult<CostCurrentPersonelDTO>> GetListAsync(CostCurrentPersonelFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentPersonelDTO>
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
                                        .Include(x => x.Grade)
                                        .Include(x => x.Contract)
                                        .Include(x => x.JobDepartment)
                                        .Include(x => x.JobStatus)
                                        .Include(x => x.JobStatusDetail)
                                        .Select(x => x.Adapt<CostCurrentPersonelDTO>()).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task<ValidationResult> CalculationAsync(int yearId, int organizationId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
                new SqlParameter("OrganizationId", organizationId)
            };

            await _uow.ExecuteScalar<ValidationResult>(
                                    "[dbo].[CurrentIncomeReport_Calculation] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray());


            return ValidationResult.Success();
        }

        public async Task CopyAsync(int sourceYearId, int sourceOrgId, int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopySameYearException();

            var result = new List<CostCurrentPersonel>();

            var selfData = await Query().Where(_ => _.OrganizationId == sourceOrgId)
                                        .Where(_ => _.YearId == sourceYearId)
                                        .Where(_ => _.RecordType == RecordType.Base)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    item.YearId = destYearId;
                    item.Id = 0;
                    var entity = item.Adapt<CostCurrentPersonel>();

                    result.Add(entity);
                }
            }

            var childrens = await getChildrenData(sourceOrgId, sourceYearId, destYearId);

            if (childrens.Any())
            {
                result.AddRange(childrens);
            }

            await _dbSet.AddRangeAsync(result);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                throw new CopyDataBaseException();
            }
        }

        public async Task<IEnumerable<CostCurrentPersonelDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostCurrentPersonelFilterDTO
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
                                    .Include(x => x.Grade)
                                    .Include(x => x.Contract)
                                    .Include(x => x.JobDepartment)
                                    .Include(x => x.JobStatus)
                                    .Include(x => x.JobStatusDetail)
                                    .Select(x => x.Adapt<CostCurrentPersonelDTO>()).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<CostCurrentPersonelImportModel>
               (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentPersonel>>();

            int rowIndex = 1;

            var descendents = await _organizationService
                .GetAllDescendentsAsync(_userContext.OrganizationId);

            var costcentertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                           x.Parent.ConstantKey == ConstantKeys.__CostCenterType);

            var gradetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
               x.Parent.ConstantKey == ConstantKeys.__GradeType);

            var contracttypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
               x.Parent.ConstantKey == ConstantKeys.__ContractType);

            var jobdepartmenttypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
               x.Parent.ConstantKey == ConstantKeys.__JobDepartmentType);


            var jobstatustypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
               x.Parent.ConstantKey == ConstantKeys.__JobStatusType);


            var jobstatusdetailstypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
               x.Parent.ConstantKey == ConstantKeys.__JobStatusDetailsType);

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
                if (!await costcentertypes.AnyAsync(x => x.Id == rec.CostCenterTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUserType, rowIndex + 2, rec.CostCenterTypeId)
                        );
                }
                if (!await gradetypes.AnyAsync(x => x.Id == rec.GradeTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.GradeTypeId)
                        );
                }
                if (!await contracttypes.AnyAsync(x => x.Id == rec.ContractTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.ContractTypeId)
                        );
                }
                if (!await jobdepartmenttypes.AnyAsync(x => x.Id == rec.JobDepartmentTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.JobDepartmentTypeId)
                        );
                }
                if (!await jobstatustypes.AnyAsync(x => x.Id == rec.JobStatusTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.JobStatusTypeId)
                        );
                }
                if (!await jobstatusdetailstypes.AnyAsync(x => x.Id == rec.JobStatusDetailTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.JobStatusDetailTypeId)
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
                    record.PersonelCode,
                    record.RecordType))
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


        #region Privte Helper Methods
        private async Task<IQueryable<CostCurrentPersonel>> setFilter(
            IQueryable<CostCurrentPersonel> query,
            CostCurrentPersonelFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostCurrentPersonel>();

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
                query = query.Where(x => x.CostCenter.Title.ToUpper().Contains(filter.Search) ||
                                         x.FirstName.ToUpper().Contains(filter.Search) ||
                                         x.PersonelCode.ToString().ToUpper().Contains(filter.Search) ||
                                         x.LastName.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostCurrentPersonel> setOrder(
            IQueryable<CostCurrentPersonel> query,
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


                default:
                    return query.Include(x => x.Organization)
                                .Include(x => x.CostCenter)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.LastName)
                                .ThenBy(x => x.FirstName);
            }
        }

        private async Task<IEnumerable<CostCurrentPersonel>> getChildren(
            int parentOrganizationId,
            int yearId,
            RecordType recordType)
        {
            var children = await _orgDbSet
                .Where(x => x.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostCurrentPersonel>();
            foreach (var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .Where(x => x.RecordType == recordType)
                                .ToListAsync();

                foreach (var item in data)
                {
                    result.Add(item);
                }
                result.AddRange(await getChildren(org.Id, yearId, recordType));
            }
            return result;
        }

        private async Task<IEnumerable<CostCurrentPersonel>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(x => x.Status != EntityStatus.Deleted &&
                            x.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostCurrentPersonel>();

            foreach (var org in children)
            {
                var data = await Query()
                                .Where(x => x.YearId == yearId)
                                .Where(x => x.OrganizationId == org.Id)
                                .Where(x => x.RecordType == RecordType.Base)
                                .ToListAsync();

                foreach (var item in data)
                {
                    item.YearId = targetYearId;
                    item.Id = 0;

                    var entity = item.Adapt<CostCurrentPersonel>();
                    

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }
            return result;
        }

        //private async Task<bool> hasAnyDataAsync(int orgid, int yearid)
        //{
        //    bool any = await Query().AnyAsync(x => x.OrganizationId == orgid &&
        //                                        x.YearId == yearid);
        //    if (any)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        var childs = await _organizationService.GetWithChildrenAsync(orgid);
        //        foreach (var child in childs)
        //            if (await Query().AnyAsync(x => x.YearId == yearid && x.OrganizationId == child.Id))
        //                return true;
        //    }

        //    return false;
        //}

        #endregion

        #region Logics
        private async Task<bool> checkLogicAsync(
             int yearId,
             int personalCode,
             RecordType recordType,
             int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                   ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                   x.PersonelCode == personalCode &&
                                                   x.RecordType == recordType)

                   : await Query().AnyAsync(x => x.YearId == yearId &&
                                                 x.PersonelCode == personalCode &&
                                                 x.RecordType == recordType &&
                                                 x.Id != id);

            return !result;
        }

        #endregion
    }
}
