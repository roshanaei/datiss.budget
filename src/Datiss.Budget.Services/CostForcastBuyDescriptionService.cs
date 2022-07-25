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

    public class CostForcastBuyDescriptionService : ICostForcastBuyDescriptionService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;

        private readonly DbSet<CostForcastBuyDescription> _dbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastBuyDescriptionService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastBuyDescription>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private IQueryable<CostForcastBuyDescription> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastBuyDescription> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastBuyDescriptionDTO>> CreateAsync(CreateCostForcastBuyDescriptionDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = model.Adapt<CostForcastBuyDescription>();

            try
            {
                await _dbSet.AddAsync(entity);
                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastBuyDescriptionDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }
                var result = entity.Adapt<CostForcastBuyDescriptionDTO>();

                result.AssetTypeDisplay = (await _constSet.FindAsync(model.AssetTypeId))?.Title;
                result.AssetDetailTypeDisplay = (await _constSet.FindAsync(model.AssetDetailTypeId))?.Title;
                result.MeasurementTypeDisplay = (await _constSet.FindAsync(model.MeasurementTypeId))?.Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastBuyDescriptionDTO>.Success(result);
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastBuyDescriptionDTO>.Failed(ServiceMessages.Logic_CostForcastBuyDescriptionDuplicates);
            }


        }

        public async Task<ValidationResult<CostForcastBuyDescriptionDTO>> UpdateAsync(UpdateCostForcastBuyDescriptionDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                var entity = await _dbSet.FindAsync(model.Id);

                entity.YearId = model.YearId;
                entity.AssetTypeId = model.AssetTypeId;
                entity.AssetDetailTypeId = model.AssetDetailTypeId;
                entity.MeasurementTypeId = model.MeasurementTypeId;
                entity.UnitPrice = model.UnitPrice;

                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastBuyDescriptionDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }

                var result = entity.Adapt<CostForcastBuyDescriptionDTO>();
                result.AssetTypeDisplay = (await _constSet.FindAsync(model.AssetTypeId))?.Title;
                result.AssetDetailTypeDisplay = (await _constSet.FindAsync(model.AssetDetailTypeId))?.Title;
                result.MeasurementTypeDisplay = (await _constSet.FindAsync(model.MeasurementTypeId))?.Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastBuyDescriptionDTO>.Success(result);

            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastBuyDescriptionDTO>.Failed(ServiceMessages.Logic_CostForcastBuyDescriptionDuplicates);
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

        public async Task<SubscriptionDeleteDataResult> HardDeleteAllAsync(int yearId)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var self = await _dbSet.Where(_ => _.YearId == yearId)
                                    .ToListAsync();

            if (self.Count() == 0)
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

        public async Task<PagedResult<CostForcastBuyDescriptionDTO>> GetListAsync(CostForcastBuyDescriptionFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastBuyDescriptionDTO>
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
                                    .Include(x => x.Asset)
                                    .Include(x => x.AssetDetail)
                                    .Include(x => x.Measurement)
                                    .Select(x => x.Adapt<CostForcastBuyDescriptionDTO>())
                                    .ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int destYearId)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            var result = new List<CostForcastBuyDescription>();

            if (await Query()
                        .Where(_ => _.YearId == destYearId).AnyAsync())
                throw new CopyDestYearHasDataException();

            var selfData = await Query().Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    item.YearId = destYearId;
                    item.Id = 0;
                    var entity = item.Adapt<CostForcastBuyDescription>();

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

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId)
        {
            var data = await _excelService.ImportAsync<CostForcastBuyDescriptionImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostForcastBuyDescription>>();

            int rowIndex = 3;


            var assetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectType);

            var assetypeDetails = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectDetailType);

            var measurementtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__MeasurementType);


            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull($"Year not found with id: {yearId}");

            foreach (var rec in records)
            {
                rec.YearId = yearId;

                if (year == null || year.Status == EntityStatus.Disbaled)
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidFinanceYear, rowIndex, rec.YearId)
                        );
                }


                if (!await assetypes.AnyAsync(x => x.Id == rec.AssetTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.Asset)
                        );
                }
                
                if (!await measurementtypes.AnyAsync(x => x.Id == rec.MeasurementTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.MeasurementTypeId)
                        );
                }

                var asset = await _constSet.FindAsync(rec.AssetTypeId);
                
                var assetDetailTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                         x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectDetailType &&
                                         x.ConstantKey.Contains(asset.ConstantKey.Split(new char[] { '.', '.' })[1]));
                if(!assetDetailTypes.Any())
                    assetDetailTypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                             x.Parent.ConstantKey == ConstantKeys.__FinanceSubjectDetailType &&
                                                             x.ConstantKey.Contains("Dash"));

                if (!assetDetailTypes.Any(x => x.Id == rec.AssetDetailTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.AssetDetailTypeId)
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
                    string.Format(ServiceMessages.ImportExcelCalculationField)
                    );
            }

            return ImportResult.Succeed(
                string.Format(ServiceMessages.ImportExcelSuccess)
                );
        }

        public async Task<IEnumerable<CostForcastBuyDescriptionDTO>> GetExportItemsAsync(int yearId)
        {
            var filter = new CostForcastBuyDescriptionFilterDTO
            {
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.Asset)
                                    .Include(x => x.AssetDetail)
                                    .Include(x => x.Measurement)
                                    .Select(x => x.Adapt<CostForcastBuyDescriptionDTO>())
                                    .ToListAsync();
            return items;
        }

        #region Private Helper Methods

        private async Task<IQueryable<CostForcastBuyDescription>> setFilter(
            IQueryable<CostForcastBuyDescription> query,
            CostForcastBuyDescriptionFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastBuyDescription>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Asset.Title.ToUpper().Contains(filter.Search) ||
                                         _.AssetDetail.Title.ToUpper().Contains(filter.Search) ||
                                         _.Measurement.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostForcastBuyDescription> setOrder(
           IQueryable<CostForcastBuyDescription> query,
           string orderBy = "id",
           bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();

            return query.Include(x => x.Asset)
                        .Include(x => x.AssetDetail)
                        .Include(x => x.Measurement)
                        .OrderBy(x => x.Asset.DisplayOrder)
                        .ThenBy(x => x.AssetDetail.DisplayOrder)
                        .ThenBy(x => x.Measurement.DisplayOrder);
        }



        private async Task<bool> hasAnyDataAsync(int yearid)
        {
            bool any = await Query().AnyAsync(x => x.YearId == yearid);

            return any;

        }
        #endregion
    }
}

