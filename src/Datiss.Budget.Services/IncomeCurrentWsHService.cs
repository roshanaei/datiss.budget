using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Common.GuardToolkit;
using Mapster;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Resources;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services
{
    public class IncomeCurrentWsHService : IIncomeCurrentWsHService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentWsH> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeCurrentWsHService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentWsH>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentWsH> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentWsH> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeCurrentWsHDTO>> CreateAsync(CreateIncomeCurrentWsHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeCurrentWsH
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                UsageLayerId = model.UsageLayerId,
                NumberUser = model.NumberUser,
                UnitUser = model.UnitUser,
                AvgConsumeUser = model.AvgConsumeUser,
                ConsumptionUser = model.ConsumptionUser,
                Cost = model.Cost,
                Note3Price = model.Note3Price,
                Note3Income = model.Note3Income,
                Income = model.Income,
                SubscriptionIncome = model.SubscriptionIncome,
                SeasonalIncome = model.SeasonalIncome,
                TIncome = model.TIncome,
                Note7Income = model.Note7Income,
                Note7Price = model.Note7Price
            };

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            model.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentWsHDTO>();
                    result.UsageLayerDisplay = model.UsageLayerTitle;
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.OrganizationDisaplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberUser = model.NumberUser;
                    result.UnitUser = model.UnitUser;
                    result.AvgConsumeUser = model.AvgConsumeUser;
                    result.ConsumptionUser = model.ConsumptionUser;
                    result.Cost = model.Cost;
                    result.Note3Price = model.Note3Price;
                    result.Note3Income = model.Note3Income;
                    result.Income = model.Income;
                    result.SubscriptionIncome = model.SubscriptionIncome;
                    result.SeasonalIncome = model.SeasonalIncome;
                    result.TIncome = model.TIncome;
                    result.Note7Income = model.Note7Income;
                    result.Note7Price = model.Note7Price;

                    return ValidationResult<IncomeCurrentWsHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }


            return ValidationResult<IncomeCurrentWsHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                                                model.UserTypeTitle,
                                                model.UsageLayerTitle,
                                                organizationDisplay)
                );
        }

        public async Task<ValidationResult<IncomeCurrentWsHDTO>> UpdateAsync(UpdateIncomeCurrentWsHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            model.UsageLayerTitle = (await _constSet.FindAsync(model.UsageLayerId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.UsageLayerId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.YearId = model.YearId;
                    entity.OrganizationId = model.OrganizationId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.UsageLayerId = model.UsageLayerId;
                    entity.UnitUser = model.UnitUser;
                    entity.NumberUser = model.NumberUser;
                    entity.UnitUser = model.UnitUser;
                    entity.AvgConsumeUser = model.AvgConsumeUser;
                    entity.ConsumptionUser = model.ConsumptionUser;
                    entity.Cost = model.Cost;
                    entity.Note3Price = model.Note3Price;
                    entity.Note3Income = model.Note3Income;
                    entity.Income = model.Income;
                    entity.SubscriptionIncome = model.SubscriptionIncome;
                    entity.SeasonalIncome = model.SeasonalIncome;
                    entity.TIncome = model.TIncome;
                    entity.Note7Income = model.Note7Income;
                    entity.Note7Price = model.Note7Price;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentWsHDTO
                    {
                        YearId = model.YearId,
                        OrganizationId = model.OrganizationId,
                        UserTypeId = model.UserTypeId,
                        UsageLayerId = model.UsageLayerId,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year,
                        OrganizationDisaplay = organizationDisplay,
                        UserTypeDisplay = model.UserTypeTitle,
                        UsageLayerDisplay = model.UsageLayerTitle,
                        NumberUser = model.NumberUser,
                        UnitUser = model.UnitUser,
                        AvgConsumeUser = model.AvgConsumeUser,
                        ConsumptionUser = model.ConsumptionUser,
                        Cost = model.Cost,
                        Note3Price = model.Note3Price,
                        Note3Income = model.Note3Income,
                        Income = model.Income,
                        SubscriptionIncome = model.SubscriptionIncome,
                        SeasonalIncome = model.SeasonalIncome,
                        TIncome = model.TIncome,
                        Note7Income = model.Note7Income,
                        Note7Price = model.Note7Price,
                    };

                    return ValidationResult<IncomeCurrentWsHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWsHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeUsageLayerDuplicate,
                                    model.UserTypeTitle,
                                    model.UsageLayerTitle,
                                    organizationDisplay)
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




        #region Logics
        private async Task<bool> checkLogicAsync(
             int yearId,
             int organizationId,
             int userTypeId,
             int usageLayerId,
             int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                   ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                   x.OrganizationId == organizationId &&
                                                   x.UserTypeId == userTypeId &&
                                                   x.UsageLayerId == usageLayerId)

                   : await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId &&
                                                x.UsageLayerId == usageLayerId &&
                                                x.Id != id);

            return !result;
        }

        #endregion
    }
}
