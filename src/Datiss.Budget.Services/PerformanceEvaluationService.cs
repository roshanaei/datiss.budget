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
            => _dbSet.AsNoTracking();

        public async Task<PerformanceEvaluation> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<PerformanceEvaluationDTO>> UpdateAsync(UpdatePerformanceEvaluationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.TableFieldId , model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.Month = model.Month;
                    entity.Target = model.Target;
                    entity.Operation = model.Operation;
                    entity.Status = model.Status;


                    await _uow.SaveChangesAsync();


                    var result = new PerformanceEvaluationDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        TableFieldId = model.TableFieldId,
                        TableFieldDisplay = (await _tableTitleSet.FindAsync(model.TableFieldId)).Title,
                        OrganizationDisplay = organizationDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        Month = model.Month,
                        Operation = model.Operation,
                        Target = model.Target
                    };

                    return ValidationResult<PerformanceEvaluationDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<PerformanceEvaluationDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<PerformanceEvaluationDTO>.Failed(
                string.Format(ServiceMessages.Logic_TitleDuplicate,organizationDisplay)
                );
        }

        public async Task HardDeleteAsync(int yearId, int organizationId)
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

            foreach (var item in self)
            {
                item.Status = EntityStatus.Deleted;
            }
            foreach (var item in childrens)
            {
                item.Status = EntityStatus.Deleted;
            }

            await _uow.SaveChangesAsync();
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
                                        Target = x.Target,
                                        Month = x.Month,
                                        Operation = x.Operation
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId, bool continueIfAnyOrgMissing = false)
        {
            var data = await _excelService.ImportAsync<PerformanceEvaluationImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<PerformanceEvaluation>>();

            int rowIndex = 1;

            var table = _tableTitleSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.TableName == TablesName.CurrentCost);
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
                //if (!await dwatertypes.AnyAsync(x => x.Id == rec.DWaterTypeId))
                //{
                //    return ImportResult.Failed(
                //        string.Format(ServiceMessages.ImportExcelInvalidDiameterPipe, rowIndex + 2, rec.DWaterTypeId)
                //        );
                //}
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
            //Start DWaterType
            //var missingDWType = new List<Constant>();
            //string orgTitle = "";
            //foreach (var org in existOrgs)
            //{
            //    foreach (var item in dwatertypes)
            //    {
            //        var existDWTypeInExcel = records.Any(_ => _.DWaterTypeId == item.Id &&
            //                                  _.OrganizationId == org.Id);
            //        if (!existDWTypeInExcel)
            //        {
            //            missingDWType.Add(item);
            //            orgTitle = org.Title;
            //        }

            //    }
            //}
            //if (missingDWType.Any())
            //{
            //    string dWaterTypeNames = "";
            //    foreach (var item in missingDWType)
            //    {
            //        dWaterTypeNames += "- [" + item.Title + "]<br>";
            //    }
            //    return ImportResult.Failed(
            //        string.Format(ServiceMessages.ImportExcelDiameterPipeOrgNotInExcel, dWaterTypeNames, orgTitle));
            //}
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

        public async Task<IEnumerable<PerformanceEvaluationDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new PerformanceEvaluationFilterDTO
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
                                    .Select(x => new PerformanceEvaluationDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        Target = x.Target,
                                        Month = x.Month,
                                        Operation = x.Operation
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(PerformanceEvaluationFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Organization)
                                    .Select(x => new PerformanceEvaluationDTO
                                    {
                                        Id = x.Id,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        Target = x.Target,
                                        Month = x.Month,
                                        Operation = x.Operation
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
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

        private async Task<IEnumerable<PerformanceEvaluation>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<PerformanceEvaluation>();

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
                    if (!await checkLogicAsync(targetYearId, org.Id, item.TableFieldId))
                        throw new CopyDestYearHasDataException();

                    var entity = new PerformanceEvaluation
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        TableFieldId = item.TableFieldId,
                        Target = item.Target,
                        Month = item.Month,
                        Operation = item.Operation
                    };

                    result.Add(entity);
                }

                result.AddRange(await getChildrenData(org.Id, yearId, targetYearId));
            }

            return result;
        }
        private async Task<IEnumerable<PerformanceEvaluation>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<PerformanceEvaluation>();
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
