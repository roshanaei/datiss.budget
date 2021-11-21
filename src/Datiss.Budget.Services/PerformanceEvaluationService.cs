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

namespace Datiss.Budget.Services
{
    public class PerformanceEvaluationService : IPerformanceEvaluationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<PerformanceEvaluation> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<TablesFiledTitle> _tablesfiledSeT;

        public PerformanceEvaluationService(
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<PerformanceEvaluation>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _tablesfiledSeT = _uow.Set<TablesFiledTitle>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<PerformanceEvaluation> Query()
            => _dbSet.AsNoTracking()
                     .Where(x => x.Status != EntityStatus.Deleted);

        public async Task<PerformanceEvaluation> GetByIdAsync(int id)
        {
            var entity = await Query().SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<PerformanceEvaluationDTO>> CreateAsync(CreatePerformanceEvaluationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new PerformanceEvaluation
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                Status = model.Status,
                TableFieldId = model.TableFieldId,
                Target = model.Target,
                Operation = model.Operation
            };

            if (await checkLogicAsync(model.YearId, model.OrganizationId,model.TableFieldId))
            {
                await _dbSet.AddAsync(entity);
                await _uow.SaveChangesAsync();

                var result = entity.Adapt<PerformanceEvaluationDTO>();
                result.OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                result.TableFieldDisplay = (await _tablesfiledSeT.FindAsync(model.TableFieldId)).Title;
                result.Target = entity.Target;
                result.Operation = entity.Operation;

                return ValidationResult<PerformanceEvaluationDTO>.Success(result);
            }

            return ValidationResult<PerformanceEvaluationDTO>.Failed(
                string.Format(ServiceMessages.Logic_DWaterType,
                                model.TableFieldId)
                );
        }

        public async Task<ValidationResult<PerformanceEvaluationDTO>> UpdateAsync(UpdatePerformanceEvaluationDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            if (await checkLogicAsync(model.YearId, model.OrganizationId, model.TableFieldId, model.Id))
            {
                var entity = await _dbSet.FindAsync(model.Id);
                entity.OrganizationId = model.OrganizationId;
                entity.YearId = model.YearId;
                entity.TableFieldId = model.TableFieldId;
                entity.Target = model.Target;
                entity.Operation = model.Operation;

                await _uow.SaveChangesAsync();

                var result = new PerformanceEvaluationDTO
                {
                    OrganizationId = model.OrganizationId,
                    YearId = model.YearId,
                    TableFieldId = model.TableFieldId,
                    Target = model.Target,
                    Operation = model.Operation,
                    OrganizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title,
                    TableFieldDisplay = (await _tablesfiledSeT.FindAsync(model.TableFieldId)).Title,
                    Year = (await _yearSet.FindAsync(model.YearId)).Year
                };

                return ValidationResult<PerformanceEvaluationDTO>.Success(result);
            }

            return ValidationResult<PerformanceEvaluationDTO>.Failed(
                string.Format(ServiceMessages.Logic_DWaterType,
                                model.TableFieldId)
                );
        }
        public async Task SoftDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);

            entity.CheckArgumentIsNull(nameof(entity));

            entity.Status = EntityStatus.Deleted;

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
                                    .Include(x => x.TablesFiled)
                                    .Select(x => new PerformanceEvaluationDTO
                                    {
                                        Id = x.Id,
                                        TableFieldDisplay = x.TablesFiled.Title,
                                        TableFieldId = x.TableFieldId,
                                        OrganizationDisplay = x.Organization.Title,
                                        OrganizationId = x.OrganizationId,
                                        Target = x.Target,
                                        Operation = x.Operation,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        Status = true
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task ImportExcelAsync(IFormFile fileInfo)
        {
            var data = await _excelService.ImportAsync<PerformanceEvaluationImportModel>(fileInfo);

            var records = data.Adapt<List<PerformanceEvaluation>>();

            int rowIndex = 1;

            foreach (var record in records)
            {

                if (!await _userService.HasAccessToOrganizationAsync(record.OrganizationId))
                    throw new UserOrganizationAccessException(rowIndex);

                if (!await checkLogicAsync(
                    record.YearId,
                    record.OrganizationId,
                    record.TableFieldId))
                    throw new ImportExcelFileException(rowIndex);

                rowIndex++;
            }

            await _dbSet.AddRangeAsync(records);
            await _uow.SaveChangesAsync();
        }

        #region Private Helper Methods

        private async Task<IQueryable<PerformanceEvaluation>> setFilter(
            IQueryable<PerformanceEvaluation> query,
           PerformanceEvaluationFilterDTO filter)
        {

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
            if (filter.tableNames.HasValue)
                query = query.Where(x => x.TablesFiled.TableName == filter.tableNames);
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
                case "year":
                    return desc
                        ? query.OrderByDescending(x => x.FinanceYear.Year)
                        : query.OrderBy(x => x.FinanceYear.Year);

                case "organization":
                    return desc
                        ? query.OrderByDescending(x => x.Organization.Title)
                        : query.OrderBy(x => x.Organization.Title);

                case "displayorder":
                    return desc
                        ? query.OrderByDescending(x => x.TablesFiled.DisplayOrder)
                        : query.OrderBy(x => x.TablesFiled.DisplayOrder);

                default:
                    return desc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
            }
        }
        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int tableFieldId,
            int? id = null)
        {
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
