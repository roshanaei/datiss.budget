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
    public class CostForcastPipingWService : ICostForcastPipingWService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;

        private readonly DbSet<CostForcastPipingW> _dbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastPipingWService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastPipingW>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private IQueryable<CostForcastPipingW> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastPipingW> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastPipingWDTO>> CreateAsync(CreateCostForcastPipingWDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = model.Adapt<CostForcastPipingW>();

            try
            {
                await _dbSet.AddAsync(entity);
                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastPipingWDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }
                var result = entity.Adapt<CostForcastPipingWDTO>();

                result.DigTypeDisplay = (await _constSet.FindAsync(model.DigTypeId))?.Title;
                result.TubeTypeDisplay = (await _constSet.FindAsync(model.TubeTypeId))?.Title;
                result.DiameterPipeTypeDisplay = (await _constSet.FindAsync(model.DiameterPipeTypeId))?.Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastPipingWDTO>.Success(result);
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastPipingWDTO>.Failed(ServiceMessages.Logic_CostForcastPipingDuplicates);
            }


        }

        public async Task<ValidationResult<CostForcastPipingWDTO>> UpdateAsync(UpdateCostForcastPipingWDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                var entity = await _dbSet.FindAsync(model.Id);

                entity.YearId = model.YearId;
                entity.DigTypeId = model.DigTypeId;
                entity.TubeTypeId = model.TubeTypeId;
                entity.TubeBuyCost = model.TubeBuyCost;
                entity.RunCost = model.RunCost; 
                entity.DiameterPipeTypeId = model.DiameterPipeTypeId;

                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastPipingWDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }

                var result = entity.Adapt<CostForcastPipingWDTO>();
                result.DigTypeDisplay = (await _constSet.FindAsync(model.DigTypeId))?.Title;
                result.TubeTypeDisplay = (await _constSet.FindAsync(model.TubeTypeId))?.Title;
                result.DiameterPipeTypeDisplay = (await _constSet.FindAsync(model.DiameterPipeTypeId))?.Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastPipingWDTO>.Success(result);

            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastPipingWDTO>.Failed(ServiceMessages.Logic_CostForcastPipingDuplicates);
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

        public async Task<PagedResult<CostForcastPipingWDTO>> GetListAsync(CostForcastPipingWFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastPipingWDTO>
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
                                    .Include(x => x.DigType)
                                    .Include(x => x.TubeType)
                                    .Include(x => x.DiameterPipeType)
                                    .Select(x => x.Adapt<CostForcastPipingWDTO>())
                                    .ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int destYearId)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            var result = new List<CostForcastPipingW>();

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
                    var entity = item.Adapt<CostForcastPipingW>();

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
            var data = await _excelService.ImportAsync<CostForcastPipingWImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 20);

            var records = data.Adapt<List<CostForcastPipingW>>();

            int rowIndex = 2;


            var digtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__DigType);

            var tubetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__TubeType);

            var watertubetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__WaterTubeType);


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

                rowIndex++;
            }
            //



            rowIndex = 26;

           
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

        public async Task<IEnumerable<CostForcastPipingWDTO>> GetExportItemsAsync(int yearId)
        {
            var filter = new CostForcastPipingWFilterDTO
            {
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.DigType)
                                    .Include(x => x.TubeType)
                                    .Include(x => x.DiameterPipeType)
                                    .Select(x => x.Adapt<CostForcastPipingWDTO>())
                                    .ToListAsync();
            return items;
        }

        #region Private Helper Methods

        private async Task<IQueryable<CostForcastPipingW>> setFilter(
            IQueryable<CostForcastPipingW> query,
            CostForcastPipingWFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastPipingW>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.DigType.Title.ToUpper().Contains(filter.Search) ||
                                         _.TubeType.Title.ToUpper().Contains(filter.Search) ||
                                         _.DiameterPipeType.Title.ToUpper().Contains(filter.Search));
            }

            return query;
        }

        private IQueryable<CostForcastPipingW> setOrder(
           IQueryable<CostForcastPipingW> query,
           string orderBy = "id",
           bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {

                default:
                    return query.Include(x => x.TubeType)
                                .Include(x => x.DiameterPipeType)
                                .Include(x => x.DigType)
                                .OrderBy(x => x.TubeType.DisplayOrder)
                                .ThenBy(x => x.DiameterPipeType.DisplayOrder)
                                .ThenBy(x => x.DigType.DisplayOrder);
            }
        }



        private async Task<bool> hasAnyDataAsync( int yearid)
        {
            bool any = await Query().AnyAsync(x => x.YearId == yearid);

            return any;

        }
        #endregion
    }
}

