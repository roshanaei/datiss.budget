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
    public class CostCurrentOtherCofficientService : ICostCurrentOtherCofficientService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;

        private readonly DbSet<CostCurrentOtherCofficient> _dbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostCurrentOtherCofficientService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentOtherCofficient>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private IQueryable<CostCurrentOtherCofficient> Query()
              => _dbSet.AsNoTracking();

        public async Task<CostCurrentOtherCofficient> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentOtherCofficientDTO>> CreateAsync(CreateCostCurrentOtherCofficientDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = model.Adapt<CostCurrentOtherCofficient>();

            try
            {
                if (await checkLogicAsync(model.YearId, model.CostCenterTypeId, model.CCOtherCostsTypeId))
                {
                    await _dbSet.AddAsync(entity);

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentOtherCofficientDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = entity.Adapt<CostCurrentOtherCofficientDTO>();
                    result.CostCenterTypeDisplay = model.CostCenterTypeTitle;
                    result.CCOtherCostsTypeDisplay = model.CCOtherCostsTypeTitle;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.Fee = entity.Fee;

                    return ValidationResult<CostCurrentOtherCofficientDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentOtherCofficientDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


            return ValidationResult<CostCurrentOtherCofficientDTO>.Failed(
                string.Format(ServiceMessages.Logic_CostCenterTypeCCOtherCofficientCostsTypeDuplicate,
                                model.CostCenterTypeTitle, model.CCOtherCostsTypeTitle)
                );
        }

        public async Task<ValidationResult<CostCurrentOtherCofficientDTO>> UpdateAsync(UpdateCostCurrentOtherCofficientDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.CostCenterTypeTitle = (await _constSet.FindAsync(model.CostCenterTypeId)).Title;
            model.CCOtherCostsTypeTitle = (await _constSet.FindAsync(model.CCOtherCostsTypeId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.CostCenterTypeId, model.CCOtherCostsTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.YearId = model.YearId;
                    entity.CostCenterTypeId = model.CostCenterTypeId;
                    entity.CCOtherCostsTypeId = model.CCOtherCostsTypeId;
                    entity.Fee = model.Fee;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentOtherCofficientDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }

                    var result = new CostCurrentOtherCofficientDTO
                    {
                        YearId = model.YearId,
                        CostCenterTypeId = model.CostCenterTypeId,
                        CCOtherCostsTypeId = model.CCOtherCostsTypeId,
                        CostCenterTypeDisplay = model.CostCenterTypeTitle,
                        CCOtherCostsTypeDisplay = model.CCOtherCostsTypeTitle,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        Fee = model.Fee
                    };

                    return ValidationResult<CostCurrentOtherCofficientDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentOtherCofficientDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentOtherCofficientDTO>.Failed(
               string.Format(ServiceMessages.Logic_CostCenterTypeCCOtherCostsTypeDuplicate,
                                model.CostCenterTypeTitle, model.CCOtherCostsTypeTitle)
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
            entity.CheckArgumentIsNull(nameof(entity));

            _dbSet.Remove(entity);
            await _uow.SaveChangesAsync();

        }
        public async Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var self = await _dbSet.Where(_ => _.YearId == yearId)
                                    .ToListAsync();

            if (self.Count() == 0 )
                throw new DeleteNullRecordException();

            _dbSet.RemoveRange(self);

           
            var result = new SubscriptionDeleteDataResult
            {
                Year = year.Year,
                YearTitle = year.Title
            };

            await _uow.SaveChangesAsync();

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<CostCurrentOtherCofficientDTO>> GetListAsync(CostCurrentOtherCofficientFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentOtherCofficientDTO>
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
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.CCOtherCosts)
                                    .Select(x => new CostCurrentOtherCofficientDTO
                                    {
                                        Id = x.Id,
                                        CostCenterTypeDisplay = x.CostCenter.Title,
                                        CostCenterTypeId = x.CostCenterTypeId,
                                        CCOtherCostsTypeDisplay = x.CCOtherCosts.Title,
                                        CCOtherCostsTypeId = x.CCOtherCostsTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        Fee = x.Fee
                                    }).ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            var result = new List<CostCurrentOtherCofficient>();

            if (await Query()
                        .Where(_ => _.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, item.CostCenterTypeId, item.CCOtherCostsTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new CostCurrentOtherCofficient
                    {
                        CostCenterTypeId = item.CostCenterTypeId,
                        CCOtherCostsTypeId = item.CCOtherCostsTypeId,
                        YearId = destYearId,
                        Fee = item.Fee
                    };
                    result.Add(entity);
                }
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

        public async Task<Stream> ExportExcelAsync(CostCurrentOtherCofficientFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.CCOtherCosts)
                                    .Select(x => new CostCurrentOtherCofficientDTO
                                    {
                                        Id = x.Id,
                                        CostCenterTypeDisplay = x.CostCenter.Title,
                                        CostCenterTypeId = x.CostCenterTypeId,
                                        CCOtherCostsTypeDisplay = x.CCOtherCosts.Title,
                                        CCOtherCostsTypeId = x.CCOtherCostsTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        Fee = x.Fee
                                    }).ToListAsync();

            var ms = new MemoryStream();
            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        public async Task<IEnumerable<CostCurrentOtherCofficientDTO>> GetExportItemsAsync(int yearId)
        {
            var filter = new CostCurrentOtherCofficientFilterDTO
            {
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));
            var query = Query();
            query = await setFilter(query, filter);
            query = setOrder(query, filter.OrderBy, filter.OrderDesc);
            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.CostCenter)
                                    .Include(x => x.CCOtherCosts)
                                    .Select(x => new CostCurrentOtherCofficientDTO
                                    {
                                        Id = x.Id,
                                        CostCenterTypeDisplay = x.CostCenter.Title,
                                        CostCenterTypeId = x.CostCenterTypeId,
                                        CCOtherCostsTypeDisplay = x.CCOtherCosts.Title,
                                        CCOtherCostsTypeId = x.CCOtherCostsTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        Fee = x.Fee
                                    }).ToListAsync();

            return items;
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId)
        {
            var data = await _excelService.ImportAsync<CostCurrentOtherCofficientImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentOtherCofficient>>();

            int rowIndex = 3;

            var costCenterTypes = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CostCenterType &&
                                                 x.Status != EntityStatus.Deleted);

            var ccOtherCostsType = _constSet.Where(x => x.Parent.ConstantKey == ConstantKeys.__CCOtherCostsType &&
                                                      x.Status != EntityStatus.Deleted);

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;
                
                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex + 2, rec.YearId)
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
               

                rowIndex++;
            }
            rowIndex = 2;
            await _dbSet.AddRangeAsync(records);
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch
            {
                return ImportResult.Failed(
                    String.Format(ServiceMessages.ImportExcelCalculationField));
            }
            return ImportResult.Succeed(
                String.Format(ServiceMessages.ImportExcelSuccess)
                );
            
        }

        #region Private Helper Methods

        private IQueryable<CostCurrentOtherCofficient> setOrder(
            IQueryable<CostCurrentOtherCofficient> query,
            string orderBy = "id",
            bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {

                case "costcentertype":
                    return desc
                        ? query.OrderByDescending(x => x.CostCenter.DisplayOrder)
                        : query.OrderBy(x => x.CostCenter.DisplayOrder);

                case "ccothercoststype":
                    return desc
                        ? query.OrderByDescending(x => x.CCOtherCosts.DisplayOrder)
                        : query.OrderBy(x => x.CCOtherCosts.DisplayOrder);

                default:
                    return query.Include(x => x.CostCenter)
                                .Include(x => x.CCOtherCosts)
                                .OrderBy(x => x.CostCenter.DisplayOrder)
                                .ThenBy(x => x.CCOtherCosts.DisplayOrder);
            }
        }
        private async Task<IQueryable<CostCurrentOtherCofficient>> setFilter(
            IQueryable<CostCurrentOtherCofficient> query,
            CostCurrentOtherCofficientFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = LinqKit.PredicateBuilder.New<CostCurrentOtherCofficient>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);
            
            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.CostCenter.Title.ToUpper().Contains(filter.Search) ||
                                         _.CCOtherCosts.Title.ToUpper().Contains(filter.Search));
            }
            return query;
        }
        private async Task<bool> hasAnyDataAsync( int yearid)
        {
            bool any = await Query().AnyAsync(x =>  x.YearId == yearid);
            

            return any;

        }
        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
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
                                                x.CostCenterTypeId == costCenterTypeId &&
                                                x.CCOtherCostsTypeId == ccOtherCostsTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.CostCenterTypeId == costCenterTypeId &&
                                            x.CCOtherCostsTypeId == ccOtherCostsTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}

