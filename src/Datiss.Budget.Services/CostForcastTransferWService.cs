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

    public class CostForcastTransferWService : ICostForcastTransferWService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostForcastTransferW> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastTransferWService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastTransferW>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostForcastTransferW> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastTransferW> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastTransferWDTO>> CreateAsync(CreateCostForcastTransferWDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new CostForcastTransferW
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                TransferTypeId = model.TransferTypeId,
                DigTypeId = model.DigTypeId,
                TubeTypeId = model.TubeTypeId,
                Lenth = model.Lenth,
                PipeCost = model.PipeCost,
                RunCost = model.RunCost,
                DiameterPipeTypeId = model.DiameterPipeTypeId,
                TotalCost = model.TotalCost,
                CreditTypeId = model.CreditTypeId,
                ExtensionTypeId = model.ExtensionTypeId,
                SuggestedBudgetTopicTypeId = model.SuggestedBudgetTopicTypeId
            };

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
                    return ValidationResult<CostForcastTransferWDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }
                var result = entity.Adapt<CostForcastTransferWDTO>();

                result.TransferTypeDisplay = (await _constSet.FindAsync(model.TransferTypeId))?.Title;
                result.DigTypeDisplay = (await _constSet.FindAsync(model.DigTypeId))?.Title;
                result.TubeTypeDisplay = (await _constSet.FindAsync(model.TubeTypeId))?.Title;
                result.DiameterPipeTypeDisplay = (await _constSet.FindAsync(model.DiameterPipeTypeId))?.Title;
                result.CreditTypeDisplay = (await _constSet.FindAsync(model.CreditTypeId))?.Title;
                result.ExtensionTypeDisplay = (await _constSet.FindAsync(model.ExtensionTypeId))?.Title;
                result.SuggestedBudgetTopicTypeDisplay = (await _constSet.FindAsync(model.SuggestedBudgetTopicTypeId))?.Title;
                result.OrganizationDisplay = organizationDisplay;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastTransferWDTO>.Success(result);
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastTransferWDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


        }

        public async Task<ValidationResult<CostForcastTransferWDTO>> UpdateAsync(UpdateCostForcastTransferWDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId))?.Title;

            try
            {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.TransferTypeId = model.TransferTypeId;
                    entity.DigTypeId = model.DigTypeId;
                    entity.TubeTypeId = model.TubeTypeId;
                    entity.Lenth = model.Lenth;
                    entity.PipeCost = model.PipeCost;
                    entity.RunCost = model.RunCost;
                    entity.DiameterPipeTypeId = model.DiameterPipeTypeId;
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
                        return ValidationResult<CostForcastTransferWDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<CostForcastTransferWDTO>();
                    result.TransferTypeDisplay = (await _constSet.FindAsync(model.TransferTypeId))?.Title;
                    result.DigTypeDisplay = (await _constSet.FindAsync(model.DigTypeId))?.Title;
                    result.TubeTypeDisplay = (await _constSet.FindAsync(model.TubeTypeId))?.Title;
                    result.DiameterPipeTypeDisplay = (await _constSet.FindAsync(model.DiameterPipeTypeId))?.Title;
                    result.CreditTypeDisplay = (await _constSet.FindAsync(model.CreditTypeId))?.Title;
                    result.ExtensionTypeDisplay = (await _constSet.FindAsync(model.ExtensionTypeId))?.Title;
                    result.SuggestedBudgetTopicTypeDisplay = (await _constSet.FindAsync(model.SuggestedBudgetTopicTypeId))?.Title;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                    return ValidationResult<CostForcastTransferWDTO>.Success(result);
                
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastTransferWDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
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

            IEnumerable<CostForcastTransferW> childrens = new CostForcastTransferW[] { };

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
                Key = "CostForcastTransferW_Cal1",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostForcastTransferW_Cal1] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal2",
                Value = await _uow.ExecuteScalar<long>(
                                    "[dbo].[CostForcastTransferW_Cal2] @YearId, @OrganizationId",
                                    parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal3",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostForcastTransferW_Cal3] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal4",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostForcastTransferW_Cal4] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal5",
                Value = await _uow.ExecuteScalar<long>(
                         "[dbo].[CostForcastTransferW_Cal5] @YearId, @OrganizationId",
                         parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal6",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostForcastTransferW_Cal6] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal7",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostForcastTransferW_Cal7] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });

            result.Add(new CalculationItemData
            {
                Key = "CostForcastTransferW_Cal8",
                Value = await _uow.ExecuteScalar<long>(
             "[dbo].[CostForcastTransferW_Cal8] @YearId, @OrganizationId",
             parameters: sqlParams.ToArray())
            });


            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostForcastTransferWDTO>> GetListAsync(CostForcastTransferWFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastTransferWDTO>
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
                                    .Include(x => x.TransferType)
                                    .Include(x => x.DigType)
                                    .Include(x => x.TubeType)
                                    .Include(x => x.DiameterType)
                                    .Include(x => x.Credit)
                                    .Include(x => x.Extension)
                                    .Include(x => x.SuggestedBudgetTopic)
                                    .Select(x => x.Adapt<CostForcastTransferWDTO>())
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
            var result = new List<CostForcastTransferW>();

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
                    var entity = new CostForcastTransferW
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = destYearId,
                        TransferTypeId = item.TransferTypeId,
                        DigTypeId = item.DigTypeId,
                        TubeTypeId = item.TubeTypeId,
                        Lenth = item.Lenth,
                        PipeCost = item.PipeCost,
                        RunCost = item.RunCost,
                        DiameterPipeTypeId = item.DiameterPipeTypeId,
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
            var data = await _excelService.ImportAsync<CostForcastTransferWImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 25);

            var records = data.Adapt<List<CostForcastTransferW>>();

            int rowIndex = 26;

            var transfertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__TransferType);

            var digtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__DigType);

            var tubetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__TubeType);

            var watertubetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__WaterTubeType);

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
                if (!await transfertypes.AnyAsync(x => x.Id == rec.TransferTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.TransferTypeId)
                        );
                }
                if (!await digtypes.AnyAsync(x => x.Id == rec.DigTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.DigType)
                        );
                }
                if (!await tubetypes.AnyAsync(x => x.Id == rec.TubeTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.TubeTypeId)
                        );
                }
                if (!await watertubetypes.AnyAsync(x => x.Id == rec.DiameterPipeTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.DiameterPipeTypeId)
                        );
                }
                if (!await extensiontypes.AnyAsync(x => x.Id == rec.ExtensionTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.ExtensionTypeId)
                        );
                }
                if (!suggestedbudgettype.Any(x => x.Id == rec.SuggestedBudgetTopicTypeId))
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

        public async Task<IEnumerable<CostForcastTransferWDTO>> GetExportItemsAsync(int yearId, int organizationId)
        {
            var filter = new CostForcastTransferWFilterDTO
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
                                    .Include(x => x.TransferType)
                                    .Include(x => x.DigType)
                                    .Include(x => x.TubeType)
                                    .Include(x => x.DiameterType)
                                    .Include(x => x.Credit)
                                    .Include(x => x.Extension)
                                    .Include(x => x.SuggestedBudgetTopic)
                                    .Select(x => x.Adapt<CostForcastTransferWDTO>())
                                    .ToListAsync();
            return items;
        }

        #region Private Helper Methods

        private async Task<IQueryable<CostForcastTransferW>> setFilter(
            IQueryable<CostForcastTransferW> query,
            CostForcastTransferWFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastTransferW>();

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
                query = query.Where(_ => _.Extension.Title.ToUpper().Contains(filter.Search) ||
                                         _.SuggestedBudgetTopic.Title.ToUpper().Contains(filter.Search) ||
                                         _.TransferType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostForcastTransferW> setOrder(
           IQueryable<CostForcastTransferW> query,
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
                                .Include(x => x.TransferType)
                                .Include(x => x.DigType)
                                .Include(x => x.TubeType)
                                .Include(x => x.DiameterType)
                                .Include(x => x.Credit)
                                .Include(x => x.Extension)
                                .Include(x => x.SuggestedBudgetTopic)
                                .OrderBy(x => x.Organization.DisplayOrder)
                                .ThenBy(x => x.Organization.RowOrder)
                                .ThenBy(x => x.DigType.DisplayOrder)
                                .ThenBy(x => x.TransferType.DisplayOrder)
                                .ThenBy(x => x.Credit.DisplayOrder)
                                .ThenBy(x => x.Extension.DisplayOrder)
                                .ThenBy(x => x.SuggestedBudgetTopic.DisplayOrder)
                                .ThenBy(x => x.Extension.DisplayOrder)
                                .ThenBy(x => x.DiameterType.DisplayOrder);
            }
        }

        private async Task<IEnumerable<CostForcastTransferW>> getChildrenData(
            int parentOrganizationId,
            int yearId,
            int targetYearId)
        {

            var children = await _orgDbSet
                .Where(_ => _.Status != EntityStatus.Deleted &&
                            _.ParentId == parentOrganizationId)
                .ToListAsync();

            var result = new List<CostForcastTransferW>();

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

                    var entity = new CostForcastTransferW
                    {
                        OrganizationId = item.OrganizationId,
                        YearId = targetYearId,
                        TransferTypeId = item.TransferTypeId,
                        DigTypeId = item.DigTypeId,
                        TubeTypeId = item.TubeTypeId,
                        Lenth = item.Lenth,
                        PipeCost = item.PipeCost,
                        RunCost = item.RunCost,
                        DiameterPipeTypeId = item.DiameterPipeTypeId,
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
        private async Task<IEnumerable<CostForcastTransferW>> getChildren(
            int parentOrganizationId,
            int yearId)
        {
            var children = await _orgDbSet
                .Where(_ => _.ParentId == parentOrganizationId)
                .ToListAsync();
            var result = new List<CostForcastTransferW>();
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
