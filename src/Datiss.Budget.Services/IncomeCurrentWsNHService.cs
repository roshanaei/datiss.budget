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
using Datiss.Budget.Services.Excel.Models;
using Datiss.Budget.Security;
using Datiss.Budget.Enum;
using Microsoft.Data.SqlClient;
using Datiss.Budget.Common;
using Datiss.Budget.Extensions;

namespace Datiss.Budget.Services
{
    public class IncomeCurrentWsNHService : IIncomeCurrentWsNHService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<IncomeCurrentWsNH> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public IncomeCurrentWsNHService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<IncomeCurrentWsNH>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<IncomeCurrentWsNH> Query()
            => _dbSet.AsNoTracking();

        public async Task<IncomeCurrentWsNH> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<IncomeCurrentWsNHDTO>> CreateAsync(CreateIncomeCurrentWsNHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new IncomeCurrentWsNH
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                NumberUser = model.NumberUser,
                UnitUser = model.UnitUser,
                AvgConsumeUser = model.AvgConsumeUser,
                ConsumptionUser = model.ConsumptionUser,
                Capacity = model.Capacity,
                Cost = model.Cost,
                Income = model.Income,
                ExcessIncome = model.ExcessIncome,
                SeasonalIncome = model.SeasonalIncome,
                Note3Price = model.Note3Price,
                Note3Income = model.Note3Income,
                SubscriptionIncome = model.SubscriptionIncome,
                TotalIncome = model.TotalIncome,
                Note7Price = model.Note7Price,
                Note7Income = model.Note7Income
            };

            var usertypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<IncomeCurrentWsNHDTO>();
                    result.UserTypeDisplay = usertypeDisplay;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.NumberUser = model.NumberUser;
                    result.UnitUser = model.UnitUser;
                    result.AvgConsumeUser = model.AvgConsumeUser;
                    result.ConsumptionUser = model.ConsumptionUser;
                    result.Capacity = model.Capacity;
                    result.Cost = model.Cost;
                    result.Income = model.Income;
                    result.ExcessIncome = model.ExcessIncome;
                    result.SeasonalIncome = model.SeasonalIncome;
                    result.Note3Price = model.Note3Price;
                    result.Note3Income = model.Note3Income;
                    result.SubscriptionIncome = model.SubscriptionIncome;
                    result.TotalIncome = model.TotalIncome;
                    result.Note7Price = model.Note7Price;
                    result.Note7Income = model.Note7Income;

                    return ValidationResult<IncomeCurrentWsNHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsNHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWsNHDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                usertypeDisplay, organizationDisplay)
                );
        }

        public async Task<ValidationResult<IncomeCurrentWsNHDTO>> UpdateAsync(UpdateIncomeCurrentWsNHDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var usertypeDisplay = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.NumberUser = model.NumberUser;
                    entity.UnitUser = model.UnitUser;
                    entity.AvgConsumeUser = model.AvgConsumeUser;
                    entity.ConsumptionUser = model.ConsumptionUser;
                    entity.Capacity = model.Capacity;
                    entity.Cost = model.Cost;
                    entity.Income = model.Income;
                    entity.ExcessIncome = model.ExcessIncome;
                    entity.SeasonalIncome = model.SeasonalIncome;
                    entity.Note3Price = model.Note3Price;
                    entity.Note3Income = model.Note3Income;
                    entity.SubscriptionIncome = model.SubscriptionIncome;
                    entity.TotalIncome = model.TotalIncome;
                    entity.Note7Price = model.Note7Price;
                    entity.Note7Income = model.Note7Price;

                    await _uow.SaveChangesAsync();

                    var result = new IncomeCurrentWsNHDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        NumberUser = model.NumberUser,
                        UnitUser = model.UnitUser,
                        AvgConsumeUser = model.AvgConsumeUser,
                        ConsumptionUser = model.ConsumptionUser,
                        Capacity = model.Capacity,
                        Cost = model.Cost,
                        Income = model.Income,
                        ExcessIncome = model.ExcessIncome,
                        SeasonalIncome = model.SeasonalIncome,
                        Note3Price = model.Note3Price,
                        Note3Income = model.Note3Income,
                        SubscriptionIncome = model.SubscriptionIncome,
                        TotalIncome = model.TotalIncome,
                        Note7Price = model.Note7Income,
                        Note7Income = model.Note7Income,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = usertypeDisplay,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<IncomeCurrentWsNHDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<IncomeCurrentWsNHDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<IncomeCurrentWsNHDTO>.Failed(
                string.Format(ServiceMessages.Logic_DiameterPipeOrgDuplicate,
                usertypeDisplay, organizationDisplay)
                );
        }

        #region Logics
        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int userTypeId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == userTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == userTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion

    }
}
