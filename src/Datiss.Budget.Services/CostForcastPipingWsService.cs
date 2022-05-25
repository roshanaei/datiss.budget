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

    public class CostForcastPipingWsService : ICostForcastPipingWsService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;

        private readonly DbSet<CostForcastPipingWs> _dbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public CostForcastPipingWsService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostForcastPipingWs>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        private IQueryable<CostForcastPipingWs> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostForcastPipingWs> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostForcastPipingWsDTO>> CreateAsync(CreateCostForcastPipingWsDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = model.Adapt<CostForcastPipingWs>();

            try
            {
                await _dbSet.AddAsync(entity);
                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastPipingWsDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }
                var result = entity.Adapt<CostForcastPipingWsDTO>();

                result.DigTypeDisplay = (await _constSet.FindAsync(model.DigTypeId))?.Title;
                result.TubeTypeDisplay = (await _constSet.FindAsync(model.TubeTypeId))?.Title;
                result.DiameterPipeTypeDisplay = (await _constSet.FindAsync(model.DiameterPipeTypeId))?.Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastPipingWsDTO>.Success(result);
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastPipingWsDTO>.Failed(ServiceMessages.Logic_CostForcastPipingDuplicates);
            }


        }

        public async Task<ValidationResult<CostForcastPipingWsDTO>> UpdateAsync(UpdateCostForcastPipingWsDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                var entity = await _dbSet.FindAsync(model.Id);

                entity.YearId = model.YearId;
                entity.DigTypeId = model.DigTypeId;
                entity.TubeTypeId = model.TubeTypeId;
                entity.TubeBuyCost = model.TubeBuyCost;
                entity.NaghabCost = model.NaghabCost;
                entity.TeransheCost = model.TeransheCost;
                entity.DiameterPipeTypeId = model.DiameterPipeTypeId;

                try
                {
                    await _uow.SaveChangesAsync();
                }
                catch
                {
                    return ValidationResult<CostForcastPipingWsDTO>.Failed(
                        string.Format(ServiceMessages.ImportExcelCalculationField)
                        );
                }

                var result = entity.Adapt<CostForcastPipingWsDTO>();
                result.DigTypeDisplay = (await _constSet.FindAsync(model.DigTypeId))?.Title;
                result.TubeTypeDisplay = (await _constSet.FindAsync(model.TubeTypeId))?.Title;
                result.DiameterPipeTypeDisplay = (await _constSet.FindAsync(model.DiameterPipeTypeId))?.Title;
                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                return ValidationResult<CostForcastPipingWsDTO>.Success(result);

            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostForcastPipingWsDTO>.Failed(ServiceMessages.Logic_CostForcastPipingDuplicates);
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

        public async Task<PagedResult<CostForcastPipingWsDTO>> GetListAsync(CostForcastPipingWsFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostForcastPipingWsDTO>
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
                                    .Select(x => x.Adapt<CostForcastPipingWsDTO>())
                                    .ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int destYearId)
        {

            if (sourceYearId == destYearId)
                throw new CopySameYearException();
            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();
            var result = new List<CostForcastPipingWs>();

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
                    var entity = item.Adapt<CostForcastPipingWs>();

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
            var data = await _excelService.ImportAsync<CostForcastPipingWsImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostForcastPipingWs>>();

            int rowIndex = 2;


            var digtypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__DigType);

            var tubetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__TubeType);

            var wastetubetypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                       x.Parent.ConstantKey == ConstantKeys.__WasteTubeType);


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
                if (!await wastetubetypes.AnyAsync(x => x.Id == rec.DiameterPipeTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidTitle, rowIndex, rec.DiameterPipeTypeId)
                        );
                }

                rowIndex++;
            }
            //



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

        public async Task<IEnumerable<CostForcastPipingWsDTO>> GetExportItemsAsync(int yearId)
        {
            var filter = new CostForcastPipingWsFilterDTO
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
                                    .Select(x => x.Adapt<CostForcastPipingWsDTO>())
                                    .ToListAsync();
            return items;
        }

        #region Private Helper Methods

        private async Task<IQueryable<CostForcastPipingWs>> setFilter(
            IQueryable<CostForcastPipingWs> query,
            CostForcastPipingWsFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));
            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostForcastPipingWs>();

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

        private IQueryable<CostForcastPipingWs> setOrder(
           IQueryable<CostForcastPipingWs> query,
           string orderBy = "id",
           bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();

            return query.Include(x => x.TubeType)
                        .Include(x => x.DiameterPipeType)
                        .Include(x => x.DigType)
                        .OrderBy(x => x.TubeType.DisplayOrder)
                        .ThenBy(x => x.DiameterPipeType.DisplayOrder)
                        .ThenBy(x => x.DigType.DisplayOrder);
        }



        private async Task<bool> hasAnyDataAsync(int yearid)
        {
            bool any = await Query().AnyAsync(x => x.YearId == yearid);

            return any;

        }
        #endregion
    }
}

