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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<Subscription> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;


        public SubscriptionService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Subscription>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<Subscription> Query()
            => _dbSet.AsNoTracking();

        public async Task<Subscription> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<SubscriptionDTO>> CreateAsync(CreateSubscriptionDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new Subscription
            {
                YearId = model.YearId,
                UserTypeId = model.UserTypeId,
                SubW = model.SubW,
                SubWs = model.SubWs
            };
            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<SubscriptionDTO>();
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.SubW = entity.SubW;
                    result.SubWs = entity.SubWs;

                    return ValidationResult<SubscriptionDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<SubscriptionDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<SubscriptionDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeYearDuplicate,
                model.UserTypeTitle)
                );
        }

        public async Task<ValidationResult<SubscriptionDTO>> UpdateAsync(UpdateSubscriptionDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.UserTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.SubW = model.SubW;
                    entity.SubWs = model.SubWs;

                    await _uow.SaveChangesAsync();

                    var result = new SubscriptionDTO
                    {
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        SubW = model.SubW,
                        SubWs = model.SubWs,
                        UserTypeDisplay = model.UserTypeTitle,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<SubscriptionDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<SubscriptionDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<SubscriptionDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeYearDuplicate,
                model.UserTypeTitle)
                );
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

        public async Task<IEnumerable<CalculationItemData>> CalculationAsync(int yearId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>
            {
                new SqlParameter("YearId", yearId),
            };

            var result = new List<CalculationItemData>();
            result.Add(new CalculationItemData
            {
                Key = "",
                Value = await _uow.ExecuteScalar<int>(
                        "",
                        parameters: sqlParams.ToArray())
            });

            return await Task.FromResult(result);
        }

        public async Task<PagedResult<SubscriptionDTO>> GetListAsync(SubscriptionFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<SubscriptionDTO>
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
                                    .Include(x => x.UserType)
                                    .Select(x => new SubscriptionDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId,
                                        SubW = x.SubW,
                                        SubWs = x.SubWs
                                    }).ToListAsync();

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

            var result = new List<Subscription>();

            var selfData = await Query().Where(_ => _.YearId == sourceYearId)
                                        .ToListAsync();

            if (selfData.Any())
            {
                foreach (var item in selfData)
                {
                    if (!await checkLogicAsync(destYearId, item.UserTypeId))
                        throw new CopyDestYearHasDataException();

                    var entity = new Subscription
                    {
                        UserTypeId = item.UserTypeId,
                        YearId = destYearId,
                        SubW = item.SubW,
                        SubWs = item.SubWs
                    };
                    result.Add(entity);
                }
            }

            _dbSet.AddRange(result);

            await _uow.SaveChangesAsync();
        }

        public async Task<ImportResult> ImportExcelAsync(IFormFile fileInfo, int yearId)
        {
            var data = await _excelService.ImportAsync<SubscriptionImportModel>
                (fileInfo, sheetIndex: 0, minRowNum: 2);

            var records = data.Adapt<List<Subscription>>();

            int rowIndex = 1;

            var usertypes = _constSet.Where(x => x.Status != EntityStatus.Deleted &&
                                                   x.Parent.ConstantKey == ConstantKeys.__UserType);

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

                if (!await usertypes.AnyAsync(x => x.Id == rec.UserTypeId))
                {
                    return ImportResult.Failed(
                        string.Format(ServiceMessages.ImportExcelInvalidUserType, rowIndex + 2, rec.UserTypeId)
                        );
                }

                rowIndex++;
            }

            //Start Missing Type
            var missingUserType = new List<Constant>();

            foreach (var usert in usertypes)
            {
                var existUserTypeInExcel = records.Any(_ => _.UserTypeId == usert.Id);
                if (!existUserTypeInExcel)
                {
                    missingUserType.Add(usert);
                }
            }

            if (missingUserType.Any())
            {
                string userTypeNames = "";
                foreach (var item in missingUserType)
                {
                    userTypeNames += " - " + item.Title + "<br>";
                }
                return ImportResult.Failed(
                    string.Format(ServiceMessages.ImportExcelUserTypeNotInExcel, userTypeNames));
            }
            //End

            rowIndex = 1;

            foreach (var record in records)
            {
                if (!await checkLogicAsync(
                    record.YearId,
                    record.UserTypeId))
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

        public async Task<IEnumerable<SubscriptionDTO>> GetExportItemsAsync(int yearId)
        {
            var filter = new SubscriptionFilterDTO
            {
                YearId = yearId
            };
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.UserType)
                                    .Select(x => new SubscriptionDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        SubW = x.SubW,
                                        SubWs = x.SubWs,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            return items;
        }

        public async Task<Stream> ExportExcelAsync(SubscriptionFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var query = Query();

            query = await setFilter(query, filter);

            query = setOrder(query, filter.OrderBy, filter.OrderDesc);

            var items = await query
                                    .Include(x => x.FinanceYear)
                                    .Include(x => x.UserType)
                                    .Select(x => new SubscriptionDTO
                                    {
                                        Id = x.Id,
                                        UserTypeDisplay = x.UserType.Title,
                                        UserTypeId = x.UserTypeId,
                                        SubW = x.SubW,
                                        SubWs = x.SubWs,
                                        Year = x.FinanceYear.Year,
                                        YearId = x.YearId
                                    }).ToListAsync();

            var ms = new MemoryStream();

            var result = _excelService.Export(items, ms);

            var mem1 = new MemoryStream(ms.ToArray());

            return mem1;
        }

        #region Private Helper Methods
        private async Task<IQueryable<Subscription>> setFilter(
            IQueryable<Subscription> query,
            SubscriptionFilterDTO filter)
        {
            query.CheckArgumentIsNull(nameof(query));

            filter.CheckArgumentIsNull(nameof(filter));

            var predicate = PredicateBuilder.New<Subscription>();

            if (filter.YearId.HasValue)
                query = query.Where(x => x.YearId == filter.YearId.Value);

            if (filter.UserTypeId.HasValue)
                query = query.Where(x => x.UserTypeId == filter.UserTypeId.Value);

            if (filter.Search.IsNotNullOrEmpty())
            {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();

                bool isNum = int.TryParse(filter.Search, out int res);

                if (isNum)
                {
                    query = query.Where(_ => _.SubW.ToString().Contains(filter.Search) ||
                                        _.SubWs.ToString().Contains(filter.Search));
                }
                else
                {
                    query = query.Where(_ => _.UserType.Title.ToUpper().Contains(filter.Search));
                }
            }

            return query;
        }

        private IQueryable<Subscription> setOrder(
           IQueryable<Subscription> query,
           string orderBy = "id",
           bool desc = false)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch (orderBy)
            {
                case "usertype":
                    return desc
                        ? query.OrderByDescending(x => x.UserType.DisplayOrder)
                        : query.OrderBy(x => x.UserType.DisplayOrder);

                default:
                    return query.Include(x => x.UserType)
                                .OrderBy(x => x.UserType.DisplayOrder);
            }
        }

        private async Task<bool> hasAnyDataAsync(int yearid)
        {
            bool any = await Query().AnyAsync(x => x.YearId == yearid);

            if (any)
            {
                return true;
            }
            else
            {
                if (await Query().AnyAsync(x => x.YearId == yearid))
                    return true;
            }

            return false;
        }
        #endregion

        #region Logics

        private async Task<bool> checkLogicAsync(
            int yearId,
            int userTypeId,
            int? id = null)
        {

            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.UserTypeId == userTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.UserTypeId == userTypeId &&
                                            x.Id != id);
            return !result;
        }
        #endregion
    }
}
