using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Excel;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Models;
using Mapster;
using Datiss.Budget.Resources;

namespace Datiss.Budget.Services
{
    public class AverageContractedCapacityNHUsesService : IAverageContractedCapacityNHUsesService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        private readonly DbSet<AverageContractedCapacityNHUses> _dbSet;
        private readonly DbSet<Organization> _orgDbSet;
        private readonly DbSet<FinanceYear> _yearSet;
        private readonly DbSet<Constant> _constSet;

        public AverageContractedCapacityNHUsesService(
            IUserContext userContext,
            IUnitOfWork uow,
            IExcelService excelService,
            IUserService userService,
            IOrganizationService organizationService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<AverageContractedCapacityNHUses>();
            _orgDbSet = _uow.Set<Organization>();
            _yearSet = _uow.Set<FinanceYear>();
            _constSet = _uow.Set<Constant>();
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        private IQueryable<AverageContractedCapacityNHUses> Query()
            => _dbSet.AsNoTracking();

        public async Task<AverageContractedCapacityNHUses> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<ValidationResult<AverageContractedCapacityNHUsesDTO>> CreateAsync(CreateAverageContractedCapacityNHUsesDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            var entity = new AverageContractedCapacityNHUses
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UserTypeId = model.UserTypeId,
                AverageCapacity = model.AverageCapacity,
                AverageCapacityWs = model.AverageCapacityWs,
                AverageCapacityIncome = model.AverageCapacityIncome,
                AverageCapacityWsIncome = model.AverageCapacityWsIncome
            };

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;
            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId))
                {
                    await _dbSet.AddAsync(entity);
                    await _uow.SaveChangesAsync();

                    var result = entity.Adapt<AverageContractedCapacityNHUsesDTO>();
                    result.UserTypeDisplay = model.UserTypeTitle;
                    result.OrganizationDisplay = organizationDisplay;
                    result.Year = (await _yearSet.FindAsync(model.YearId)).Year;
                    result.AverageCapacity = entity.AverageCapacity;
                    result.AverageCapacityWs = entity.AverageCapacityWs;
                    result.AverageCapacityIncome = entity.AverageCapacityIncome;
                    result.AverageCapacityWsIncome = entity.AverageCapacityWsIncome;

                    return ValidationResult<AverageContractedCapacityNHUsesDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<AverageContractedCapacityNHUsesDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<AverageContractedCapacityNHUsesDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                model.UserTypeTitle, organizationDisplay)
                );

        }

        public async Task<ValidationResult<AverageContractedCapacityNHUsesDTO>> UpdateAsync(UpdateAverageContractedCapacityNHUsesDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            model.UserTypeTitle = (await _constSet.FindAsync(model.UserTypeId)).Title;
            var organizationDisplay = (await _orgDbSet.FindAsync(model.OrganizationId)).Title;

            try
            {
                if (await checkLogicAsync(model.YearId, model.OrganizationId, model.UserTypeId, model.Id))
                {
                    var entity = await _dbSet.FindAsync(model.Id);
                    entity.OrganizationId = model.OrganizationId;
                    entity.YearId = model.YearId;
                    entity.UserTypeId = model.UserTypeId;
                    entity.AverageCapacity = model.AverageCapacity;
                    entity.AverageCapacityWs = model.AverageCapacityWs;
                    entity.AverageCapacityIncome = model.AverageCapacityIncome;
                    entity.AverageCapacityWsIncome = model.AverageCapacityWsIncome;

                    await _uow.SaveChangesAsync();

                    var result = new AverageContractedCapacityNHUsesDTO
                    {
                        OrganizationId = model.OrganizationId,
                        YearId = model.YearId,
                        UserTypeId = model.UserTypeId,
                        AverageCapacity = model.AverageCapacity,
                        AverageCapacityWs = model.AverageCapacityWs,
                        AverageCapacityIncome = model.AverageCapacityIncome,
                        AverageCapacityWsIncome = model.AverageCapacityWsIncome,
                        OrganizationDisplay = organizationDisplay,
                        UserTypeDisplay = model.UserTypeTitle,
                        Year = (await _yearSet.FindAsync(model.YearId)).Year
                    };

                    return ValidationResult<AverageContractedCapacityNHUsesDTO>.Success(result);
                }
            }
            catch (DisbaledYearDataInputException)
            {
                return ValidationResult<AverageContractedCapacityNHUsesDTO>.Failed(ServiceMessages.Logic_InputDisableYearData);
            }

            return ValidationResult<AverageContractedCapacityNHUsesDTO>.Failed(
                string.Format(ServiceMessages.Logic_UserTypeDuplicate,
                model.UserTypeTitle, organizationDisplay)
                );
        }

        #region Logics
        private async Task<bool> checkLogicAsync(
            int yearId,
            int organizationId,
            int dwaterTypeId,
            int? id = null)
        {
            var year = await _yearSet.FindAsync(yearId);
            year.CheckReferenceIsNull(nameof(year));

            if (year.Status == EntityStatus.Disbaled)
                throw new DisbaledYearDataInputException();

            var result = id == null
                ? await Query().AnyAsync(x => x.YearId == yearId &&
                                                x.OrganizationId == organizationId &&
                                                x.UserTypeId == dwaterTypeId)

                : await Query().AnyAsync(x => x.YearId == yearId &&
                                            x.OrganizationId == organizationId &&
                                            x.UserTypeId == dwaterTypeId &&
                                            x.Id != id);
            return !result;
        }

        #endregion
    }
}
