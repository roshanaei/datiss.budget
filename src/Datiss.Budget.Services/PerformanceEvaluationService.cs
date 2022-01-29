using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Common.Exceptions;
using LinqKit;
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Security;
using Datiss.Budget.Common;
using System.IO;
using Datiss.Budget.Extensions;
using ClosedXML.Excel;

namespace Datiss.Budget.Services
{
    public class PerformanceEvaluationService : IPerformanceEvaluationService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<PerformanceEvaluation> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<TablesFiledTitle> _tableTitleSet;

        public PerformanceEvaluationService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<PerformanceEvaluation>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _tableTitleSet = _uow.Set<TablesFiledTitle>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<PerformanceEvaluation> Query()
            => _dbSet.AsNoTracking().Where(x => x.Status != EntityStatus.Deleted);

        public async Task<PerformanceEvaluation> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<PerformanceEvaluationDTO>> UpdateAsync(UpdatePerformanceEvaluationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.TableFieldId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.Operation = model.Operation;

                    await _uow.SaveChangesAsync();

                    var result = new PerformanceEvaluationDTO
                    {
                        OrganizationId = entity.OrganizationId,
                        YearId = entity.YearId,
                        TableFieldId = entity.TableFieldId,
                        TableFieldDisplay = (await _tableTitleSet.FindAsync(entity.TableFieldId)).Title,
                        OrganizationDisplay = (await _orgDbSet.FindAsync(entity.OrganizationId)).Title,
                        Year = (await _yearSet.FindAsync(entity.YearId)).Year,
                        Month = entity.Month,
                        Operation = entity.Operation,
                        Target = entity.Target
                    };

                    return ValidationResult<PerformanceEvaluationDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<PerformanceEvaluationDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<PerformanceEvaluationDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate)
                );
        }

        public async Task<OrganizationDeleteDataResult> SoftDeleteAsync(int yearId, int organizationId , TablesName tablesName)
        {
            var organization = await _orgDbSet.FindAsync(organizationId);
            organization.CheckReferenceIsNull(nameof(organization));

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var self = await _dbSet.Where(_ => _.YearId == yearId &&
                                               _.Status != EntityStatus.Deleted)
                                    .Where(_ => _.OrganizationId == organizationId)
                                    .Where(_=>_.TablesFiled.TableName == tablesName)
                                    .ToListAsync();
            var childrens = await getChildren(organizationId, yearId , tablesName);

            if (self.Count() == 0 && childrens.Count() == 0)
                throw new DeleteNullRecordException();

            foreach (var item in self)
            {
                item.Status = EntityStatus.Deleted;
            }
            foreach (var item in childrens)
            {
                item.Status = EntityStatus.Deleted;
            }

            var result = new OrganizationDeleteDataResult
            {
                OrganizationTitle = organization.Title,
                Year = year.Year,
                YearTitle = year.Title
            };

            await _uow.SaveChangesAsync();

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<PerformanceEvaluationDTO>> GetListAsync(PerformanceEvaluationFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<PerformanceEvaluationDTO>
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
                                    .Select(x => new PerformanceEvaluationDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        TableFieldId = x.TableFieldId,
                                        TableFieldDisplay = x.TablesFiled.Title,
                                        Target = x.Target,
                                        Month = x.Month,
                                        Operation = x.Operation
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, TablesName tablesName, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<PerformanceEvaluationImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);
            //GET Month
            int month = 0;
            using var stream = new MemoryStream();
            await fileInfo.CopyToAsync(stream);
            using var wbook = new XLWorkbook(stream);
            if (!wbook.Worksheet(1).Row(1).Cell(5).IsEmpty())
            {
                try
                {
                    month = Convert.ToInt32(wbook.Worksheet(1).Row(1).Cell(5).Value);
                    if(month > 12 || month<0)
                        return ImportResult.Failed(
                            ServiceMessages.ImportExcelInvalidMonth
                            );
                }
                catch
                {
                    return ImportResult.Failed(
                        ServiceMessages.ImportExcelInvalidMonth
                        );
                }
            }
            //
            var records = data.Adapt<List<PerformanceEvaluation>>();

            int rowIndex = 1;

            var tableTitle = _tableTitleSet.Where(x => x.ParentId != null &&
                                                       x.Status != EntityStatus.Deleted &&
                                                       x.TableName == tablesName &&
                                                       x.SectionName == SectionName.A);

            var descendents = await _organizationService
                             .GetAllDescendentsAsync(_userContext.OrganizationId);

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;
                rec.Month = month;
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
                if (!await tableTitle.AnyAsync(x => x.Id == rec.TableFieldId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex + 2, rec.TableFieldId)
                        );
                }
                if (org.Type == Enum.OrganizationType.City || org.Type == Enum.OrganizationType.Village)
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
                    if (item.Type == Enum.OrganizationType.Root || item.Type == Enum.OrganizationType.County)
                        missingOrgs.Add(item);
                }
                else
                    existOrgs.Add(item);
            }
            //
            //Start TitleType
            var missingTitleType = new List<TablesFiledTitle>();
            string orgTitle = "";
            foreach (var org in existOrgs)
            {
                foreach (var item in tableTitle)
                {
                    var existTitleTypeInExcel = records.Any(_ => _.TableFieldId == item.Id &&
                                              _.OrganizationId == org.Id);
                    if (!existTitleTypeInExcel)
                    {
                        missingTitleType.Add(item);
                        orgTitle = org.Title;
                    }

                }
            }
            if (missingTitleType.Any())
            {
                string titleTypeNames = "";
                foreach (var item in missingTitleType)
                {
                    titleTypeNames += "- [" + item.Title + "]<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelTitleNotInExcel, titleTypeNames, orgTitle));
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
                    record.TableFieldId))
                {
                    var row = await _dbSet.SingleOrDefaultAsync(x => x.YearId == record.YearId &&
                                                                     x.Status != EntityStatus.Deleted &&
                                                                     x.OrganizationId == record.OrganizationId &&
                                                                     x.TableFieldId == record.TableFieldId);
                    row.Status = EntityStatus.Deleted;
                }

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();

            return ImportResult.Succeed(
                string.Format(ServiceMessages.ImportExcelSuccess)
                );
        }

        public async Task<IEnumerable<PerformanceEvaluationDTO>> GetExportItemsAsync(int yearId, int organizationId, TablesName tablesName)
        {
            var filter = new PerformanceEvaluationFilterDTO
            {
                TableName = tablesName,
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
                                    .Include(x => x.TablesFiled)
                                    .Select(x => new PerformanceEvaluationDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        TableFieldId = x.TableFieldId,
                                        TableFieldDisplay = x.TablesFiled.Title,
                                        Target = x.Target,
                                        Month = x.Month,
                                        Operation = x.Operation
                                    }).ToListAsync();

            return items;
        }


        #region Private Helper Methods

        private async Task<IQueryable<PerformanceEvaluation>> setFilter(
            IQueryable<PerformanceEvaluation> query,
            PerformanceEvaluationFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<PerformanceEvaluation>();

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

            if (filter.TableName.HasValue)
                query = query.Where(x => x.TablesFiled.TableName == filter.TableName);

            if (filter.SectionName.HasValue)
                query = query.Where(x => x.TablesFiled.SectionName == filter.SectionName);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.TablesFiled.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<PerformanceEvaluation> setOrder(
           IQueryable<PerformanceEvaluation> query,
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
                                .Include(x => x.TablesFiled)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.Type)
                                .ThenBy(x => x.Organization.ParentId)
                                .ThenBy(x => x.TablesFiled.DisplayOrder);
            }
        }

        private async Task<IEnumerable<PerformanceEvaluation>> getChildren(
            int parentOrganizationId,
            int yearId , 
            TablesName tablesName)
        {
            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<PerformanceEvaluation>();
            foreach (var org in children)
            {
                var data = await _dbSet
                                .Where(_ => _.Status != EntityStatus.Deleted)
                                .Where(_ => _.YearId == yearId)
                                .Where(_ => _.OrganizationId == org.Id)
                                .Where(_=>_.TablesFiled.TableName == tablesName)
                                .ToListAsync();

                foreach (var item in data)
                {
                    result.Add(item);
                }
                result.AddRange(await getChildren(org.Id, yearId , tablesName));
            }
            return result;
        }
        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int tableFieldId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.TableFieldId == tableFieldId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.TableFieldId == tableFieldId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
