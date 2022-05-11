using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Services.Contracts.Identity;
using Mapster;
using LinqKit;
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Security;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Resources;
using Datiss.Budget.Enum;
using System.Data.SqlClient;
using Datiss.Budget.Extensions;
using Datiss.Budget.Common;

namespace Datiss.Budget.Services
{
    public class CostCurrentPrescriptionBaseInfoService : ICostCurrentPrescriptionBaseInfoService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<CostCurrentPrescriptionBaseInfo> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;


        public CostCurrentPrescriptionBaseInfoService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<CostCurrentPrescriptionBaseInfo>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<CostCurrentPrescriptionBaseInfo> Query()
            => _dbSet.AsNoTracking();

        public async Task<CostCurrentPrescriptionBaseInfo> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<CostCurrentPrescriptionBaseInfoDTO>> CreateAsync(CreateCostCurrentPrescriptionBaseInfoDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = model.Adapt<CostCurrentPrescriptionBaseInfo>();
            
            try
            {
                    await _dbSet.AddAsync(entity);
                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                    var result = entity.Adapt<CostCurrentPrescriptionBaseInfoDTO>();
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;

                    return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Success(result);
                
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Failed(
                string.Format(ServiceMessages.Logic_YearOrgDuplicate));
        }

        public async Task<ValidationResult<CostCurrentPrescriptionBaseInfoDTO>> UpdateAsync(UpdateCostCurrentPrescriptionBaseInfoDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.YearId = model.YearId;
                    entity.FixSalary = model.FixSalary;
                    entity.HouseRt = model.HouseRt;
                    entity.EmployRight = model.EmployRight;
                    entity.RegionRight = model.RegionRight;
                    entity.Copun = model.Copun;
                    entity.ChildRt = model.ChildRt;
                    entity.StuffRt = model.StuffRt;
                    entity.HardWorkingRt = model.HardWorkingRt;
                    entity.Healths = model.Healths;
                    entity.NewFixSalary = model.NewFixSalary;

                    try
                    {
                        await _uow.SaveChangesAsync();
                    }
                    catch
                    {
                        return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Failed(
                            string.Format(ServiceMessages.ImportExcelCalculationField)
                            );
                    }
                var result = model.Adapt<CostCurrentPrescriptionBaseInfoDTO>();

                result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    

                    return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Success(result);
                
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<CostCurrentPrescriptionBaseInfoDTO>.Failed(
                string.Format(ServiceMessages.Logic_YearOrgDuplicate));
        }

        public async Task HardDeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            entity.CheckArgumentIsNull(nameof(entity));

            var year = await _yearSet.FindAsync(entity.YearId);
            year.CheckArgumentIsNull(nameof(year));

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

        public async Task<PagedResult<CostCurrentPrescriptionBaseInfoDTO>> GetListAsync(CostCurrentPrescriptionBaseInfoFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<CostCurrentPrescriptionBaseInfoDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = Query();

            query = await setFilter(query, filter);

            result.TotalCount = await query.CountAsync();

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query.Include(x => x.FinanceYear)
                                    .Select(x => x.Adapt<CostCurrentPrescriptionBaseInfoDTO>())
                                    .ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task CopyAsync(int sourceYearId, int destYearId)
        {
            if (sourceYearId == destYearId)
                throw new CopySameYearException();

            if (destYearId < sourceYearId)
                throw new CopyDestYearExxeption();

            if (!await hasAnyDataAsync(sourceYearId))
                throw new CopyOrgNullDataException();

            var result = new List<CostCurrentPrescriptionBaseInfo>();

            var selfData = await Query().Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    item.Id = 0;
                    item.YearId = destYearId;
                    var entity = item.Adapt<CostCurrentPrescriptionBaseInfo>();

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
            var data = await _excelService.ImportAsync<CostCurrentPrescriptionBaseInfoImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<CostCurrentPrescriptionBaseInfo>>();

            int rowIndex = 1;

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

        public async Task<IEnumerable<CostCurrentPrescriptionBaseInfoDTO>> GetExportItemsAsync(int yearId)
        {
            var filter = new CostCurrentPrescriptionBaseInfoFilterDTO
            {
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            var items = await query.Include(x => x.FinanceYear)
                                    .Select(x => x.Adapt<CostCurrentPrescriptionBaseInfoDTO>())
                                    .ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(CostCurrentPrescriptionBaseInfoFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            var items = await query.Include(x => x.FinanceYear)
                                    .Select(x => x.Adapt<CostCurrentPrescriptionBaseInfoDTO>())
                                    .ToListAsync();

            var ms = new MemoryStream();

            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Private Helper Methods
        private async Task<IQueryable<CostCurrentPrescriptionBaseInfo>> setFilter(
            IQueryable<CostCurrentPrescriptionBaseInfo> query,
            CostCurrentPrescriptionBaseInfoFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));

            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<CostCurrentPrescriptionBaseInfo>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);


            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();

                query = query.Where(_ => _.FixSalary.ToString().ToUpper().Contains(filter.Search) ||
                                         _.HouseRt.ToString().ToUpper().Contains(filter.Search) ||
                                         _.EmployRight.ToString().ToUpper().Contains(filter.Search) ||
                                         _.RegionRight.ToString().ToUpper().Contains(filter.Search) ||
                                         _.Copun.ToString().ToUpper().Contains(filter.Search)
                );
            }

            return query;
        }

        private async Task<bool> hasAnyDataAsync(int yearid)
        {
            bool any = await Query().AnyAsync(x => x.YearId == yearid);

            if (any)
            {
                return true;
            }

            return false;
        }
        #endregion

    }
}
