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

    public class CostForcastConstructionWService : ICostForcastConstructionWService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostForcastConstructionW> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastConstructionWService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastConstructionW>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostForcastConstructionW> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastConstructionW> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastConstructionWDTO>> CreateAsync(CreateCostForcastConstructionWDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new CostForcastConstructionW
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                ProjectDescription = model.ProjectDescription,
                WaterInvestorsTypeId = model.WaterInvestorsTypeId,
                CostCenterTypeId = model.CostCenterTypeId,
                ExploitationAreaTypeId = model.ExploitationAreaTypeId,
                ProgressPercent = model.ProgressPercent,
                CostDone = model.CostDone,
                Amount = model.Amount,
                MeasurementTypeId = model.MeasurementTypeId,
                UnitPrice = model.UnitPrice,
                TotalCost = model.TotalCost,
                CreditTypeId = model.CreditTypeId,
                ExtensionTypeId = model.ExtensionTypeId,
                SuggestedBudgetTopicTypeId = model.SuggestedBudgetTopicTypeId
            };

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId))?.Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ProjectDescription))
                {
                    await _dbSet.AddAsync(entity);
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostForcastConstructionWDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = entity.Adapt<CostForcastConstructionWDTO>();

                    result.WaterInvestorsDisplay = (await _constSet.FindAsync(model.WaterInvestorsTypeId))?.Title; 
                    result.CostCenterDisplay = (await _constSet.FindAsync(model.CostCenterTypeId))?.Title;
                    result.ExploitationAreaDisplay = (await _constSet.FindAsync(model.ExploitationAreaTypeId))?.Title;
                    result.MeasurementDisplay = (await _constSet.FindAsync(model.MeasurementTypeId))?.Title;
                    result.CreditDisplay = (await _constSet.FindAsync(model.CreditTypeId))?.Title;
                    result.ExtensionDisplay = (await _constSet.FindAsync(model.ExtensionTypeId))?.Title;
                    result.SuggestedBudgetTopicDisplay = (await _constSet.FindAsync(model.SuggestedBudgetTopicTypeId))?.Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                    return ValidationResult<CostForcastConstructionWDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastConstructionWDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostForcastConstructionWDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,
                model.ProjectDescription, organizationDisplay)
                );


        }

        public async Task<ValidationResult<CostForcastConstructionWDTO>> UpdateAsync(UpdateCostForcastConstructionWDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId))?.Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.ProjectDescription, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.ProjectDescription = model.ProjectDescription;
                    entity.WaterInvestorsTypeId = model.WaterInvestorsTypeId;
                    entity.CostCenterTypeId = model.CostCenterTypeId;
                    entity.ExploitationAreaTypeId = model.ExploitationAreaTypeId;
                    entity.ProgressPercent = model.ProgressPercent;
                    entity.CostDone = model.CostDone;
                    entity.Amount = model.Amount;
                    entity.MeasurementTypeId = model.MeasurementTypeId;
                    entity.UnitPrice = model.UnitPrice;
                    entity.TotalCost = model.TotalCost;
                    entity.CreditTypeId = model.CreditTypeId;
                    entity.ExtensionTypeId = model.ExtensionTypeId;
                    entity.SuggestedBudgetTopicTypeId = model.SuggestedBudgetTopicTypeId;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostForcastConstructionWDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<CostForcastConstructionWDTO>();
                    result.WaterInvestorsDisplay = (await _constSet.FindAsync(model.WaterInvestorsTypeId))?.Title;
                    result.CostCenterDisplay = (await _constSet.FindAsync(model.CostCenterTypeId))?.Title;
                    result.ExploitationAreaDisplay = (await _constSet.FindAsync(model.ExploitationAreaTypeId))?.Title;
                    result.MeasurementDisplay = (await _constSet.FindAsync(model.MeasurementTypeId))?.Title;
                    result.CreditDisplay = (await _constSet.FindAsync(model.CreditTypeId))?.Title;
                    result.ExtensionDisplay = (await _constSet.FindAsync(model.ExtensionTypeId))?.Title;
                    result.SuggestedBudgetTopicDisplay = (await _constSet.FindAsync(model.SuggestedBudgetTopicTypeId))?.Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                    return ValidationResult<CostForcastConstructionWDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastConstructionWDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostForcastConstructionWDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,
                model.ProjectDescription, organizationDisplay)
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

            IEnumerable<CostForcastConstructionW> childrens = new CostForcastConstructionW[] { };

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
                Key = "CostForcastConstructionW_Cal1",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostForcastConstructionW_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal2",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostForcastConstructionW_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal3",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostForcastConstructionW_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal4",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostForcastConstructionW_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal5",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostForcastConstructionW_Cal5] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal6",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostForcastConstructionW_Cal6] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal7",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostForcastConstructionW_Cal7] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastConstructionW_Cal8",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostForcastConstructionW_Cal8] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });


            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostForcastConstructionWDTO>> GetListAsync(CostForcastConstructionWFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastConstructionWDTO>
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
                                    .Include(x => x.WaterInvestors)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.ExploitationArea)
                                    .Include(x => x.Measurement)
                                    .Include(x => x.Credit)
                                    .Include(x => x.Extension)
                                    .Include(x => x.SuggestedBudgetTopic)
                                    .Select(x => x.Adapt<CostForcastConstructionWDTO>())
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
            var result = new List<CostForcastConstructionW>();

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
                    if (!await checkLogicAsync(destYearId, sourceOrgId, item.ProjectDescription))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostForcastConstructionW
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        ProjectDescription = item.ProjectDescription,
                        WaterInvestorsTypeId = item.WaterInvestorsTypeId,
                        CostCenterTypeId = item.CostCenterTypeId,
                        ExploitationAreaTypeId = item.ExploitationAreaTypeId,
                        ProgressPercent = item.ProgressPercent,
                        CostDone = item.CostDone,
                        Amount = item.Amount,
                        MeasurementTypeId = item.MeasurementTypeId,
                        UnitPrice = item.UnitPrice,
                        TotalCost = item.TotalCost,
                        CreditTypeId = item.CreditTypeId,
                        ExtensionTypeId = item.ExtensionTypeId,
                        SuggestedBudgetTopicTypeId = item.SuggestedBudgetTopicTypeId
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
            var data = await _excelService.ImportAsync<CostForcastConstructionWImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 25);

            var records = data.Adapt<List<CostForcastConstructionW>>();

            int rowIndex = 26;

            var watertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__WaterInvestorsType);

            var costcentertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__CostCenterType);

            var explotationtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__ExploitationAreaType);

            var measurementtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__MeasurementType);

            var credittypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__CreditType);

            var extensiontypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__ExtensionType);

            var suggestedbudgettype = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                        x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectType &&
                                                        x.ConstantKey.Contains(ConstantKeys.__ExtensionNo)).ToList();

            var extentionyestypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                        x.Parent.ConstantKey == ConstantKeys.__SuggestedBudgetTopicType &&
                                                        x.ConstantKey.Contains(ConstantKeys.__ExtensionYes)).ToList();
            foreach (var item in extentionyestypes)
            {
                suggestedbudgettype.Add(item);
            }


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
                if (!await watertypes.AnyAsync(x => x.Id == rec.WaterInvestorsTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.WaterInvestorsTypeId)
                        );
                }
                if (!await costcentertypes.AnyAsync(x => x.Id == rec.CostCenterTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidCostCenterType, rowIndex, rec.CostCenter)
                        );
                }
                if (!await explotationtypes.AnyAsync(x => x.Id == rec.ExploitationAreaTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.ExploitationAreaTypeId)
                        );
                }
                if (!await measurementtypes.AnyAsync(x => x.Id == rec.MeasurementTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.MeasurementTypeId)
                        );
                }
                if (!await extensiontypes.AnyAsync(x => x.Id == rec.ExtensionTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.ExtensionTypeId)
                        );
                }
                if (! suggestedbudgettype.Any(x => x.Id == rec.SuggestedBudgetTopicTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.SuggestedBudgetTopicTypeId)
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

            rowIndex = 26;

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

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.ProjectDescription))
                {

                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelLogicError, rowIndex)
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

        public async Task<IEnumerable<CostForcastConstructionWDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostForcastConstructionWFilterDTO
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
                                    .Include(x => x.WaterInvestors)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.ExploitationArea)
                                    .Include(x => x.Measurement)
                                    .Include(x => x.Credit)
                                    .Include(x => x.Extension)
                                    .Include(x => x.SuggestedBudgetTopic)
                                    .Select(x => x.Adapt<CostForcastConstructionWDTO>())
                                    .ToListAsync();
            return items;
        }

        #region Private Helper Methods

        private async Task<IQueryable<CostForcastConstructionW>> setFilter(
            IQueryable<CostForcastConstructionW> query,
            CostForcastConstructionWFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastConstructionW>();

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
                query = query.Where(_ => _.ProjectDescription.ToUpper().Contains(filter.Search) ||
                                         _.Extension.Title.ToUpper().Contains(filter.Search) ||
                                         _.SuggestedBudgetTopic.Title.ToUpper().Contains(filter.Search) ||
                                         _.WaterInvestors.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostForcastConstructionW> setOrder(
           IQueryable<CostForcastConstructionW> query,
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
                                .Include(x => x.WaterInvestors)
                                .Include(x => x.CostCenter)
                                .Include(x => x.ExploitationArea)
                                .Include(x => x.Measurement)
                                .Include(x => x.Credit)
                                .Include(x => x.Extension)
                                .Include(x => x.SuggestedBudgetTopic)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.CostCenter.DisplayOrder)
                                .ThenBy(x => x.WaterInvestors.DisplayOrder)
                                .ThenBy(x => x.Credit.DisplayOrder)
                                .ThenBy(x => x.Extension.DisplayOrder)
                                .ThenBy(x => x.SuggestedBudgetTopic.DisplayOrder)
                                .ThenBy(x => x.Extension.DisplayOrder)
                                .ThenBy(x => x.Measurement.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostForcastConstructionW>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostForcastConstructionW>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.ProjectDescription))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostForcastConstructionW
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        ProjectDescription = item.ProjectDescription,
                        WaterInvestorsTypeId = item.WaterInvestorsTypeId,
                        CostCenterTypeId = item.CostCenterTypeId,
                        ExploitationAreaTypeId = item.ExploitationAreaTypeId,
                        ProgressPercent = item.ProgressPercent,
                        CostDone = item.CostDone,
                        Amount = item.Amount,
                        MeasurementTypeId = item.MeasurementTypeId,
                        UnitPrice = item.UnitPrice,
                        TotalCost = item.TotalCost,
                        CreditTypeId = item.CreditTypeId,
                        ExtensionTypeId = item.ExtensionTypeId,
                        SuggestedBudgetTopicTypeId = item.SuggestedBudgetTopicTypeId
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<CostForcastConstructionW>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostForcastConstructionW>();
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
            string projectDescription,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                              x.OrganizationId == organizationId &&
                                              x.ProjectDescription.Trim().ToUpper() == projectDescription.Trim().ToUpper())

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                              x.OrganizationId == organizationId &&
                                              x.ProjectDescription.Trim().ToUpper() == projectDescription.Trim().ToUpper() &&
                                              x.Id != id);
            return !result;
        }

        #endregion
    }
}
