using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Security;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Common.Exceptions;
using DNTPersianUtils.Core;
using Datiss.Budget.Resources;
using Mapster;

namespace Datiss.Budget.Services.Identity
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _uow;
        private DbSet<User> _dbSet;
        private readonly IUserContext _userContext;
        private readonly IOrganizationService _organizationService;
        private readonly IApplicationUserManager _userManager;

        public UserService(
            IUnitOfWork uow, 
            IUserContext userContext,
            IOrganizationService organizationService,
            IApplicationUserManager userManager) {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dbSet = _uow.Set<User>();
        }

        private IQueryable<User> Querty()
            => _dbSet.Where(_ => _.Status != EntityStatus.Deleted).AsNoTracking();

        private IQueryable<User> QueryActiveUsers()
            => _dbSet.Where(_ => _.Status != EntityStatus.Enabled).AsNoTracking();


        public async Task<ValidationResult<UserResultDto>> CreateAsync(CreateUserDto model) {
            model.CheckArgumentIsNull(nameof(model));

            var validation = await validateCreateAsync(model);
            if (validation.NotValid)
                return ValidationResult<UserResultDto>
                    .Failed(ValidationMode.Create, validation.Message);

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if(existingUser != null) {
                return ValidationResult<UserResultDto>.Failed(
                    existingUser.Adapt<UserResultDto>(),
                    ValidationMode.Create,
                    string.Format(ServiceMessages.Exist_Username, model.Username));
            }

            var user = new User
            {
                FirstName = model.FirstName.ApplyCorrectYeKe().Trim(),
                LastName = model.LastName.ApplyCorrectYeKe().Trim(),
                Email = model.Email.Trim(),
                NationalCode = model.NationalCode,
                PhoneNumber = model.PhoneNumber,
                PositionId = model.PositionId,
                Status = EntityStatus.Enabled,
                TwoFactorEnabled = false,
                UserName = model.Username.ApplyCorrectYeKe().Trim(),
                OrganizationId = model.OrganizationId
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded) {
                
            }
        }


        public async Task<bool> HasAccessToOrganizationAsync(int organizationId) {
            if(_userContext.OrganizationId == null 
                || _userContext.OrganizationId == organizationId)
                    return true;

            return await _organizationService.IsDescendentOfAsync(
                _userContext.OrganizationId.Value, 
                organizationId
            );
        }


        #region private methods
        
        private void validateRequiredFields(
            string userName,
            string firstName,
            string lastName,
            string nationalCode) 
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new RequiredFieldException(nameof(userName));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new RequiredFieldException(nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldException(nameof(lastName));

            if (string.IsNullOrWhiteSpace(nationalCode))
                throw new RequiredFieldException(nameof(nationalCode));
        }

        private async Task validateNationalCodeAsync(string nationalCode, bool checkForDuplicate = false) {
            if (string.IsNullOrWhiteSpace(nationalCode))
                throw new RequiredFieldException(nameof(nationalCode));

            if (!nationalCode.IsValidIranianNationalCode())
                throw new InvalidNationalCodeException(nationalCode);

            if(checkForDuplicate) {
                //Check if the user with the given NationalCode already existed in DB.
                var user = await _dbSet.FirstOrDefaultAsync(_ => _.NationalCode == nationalCode);
                if (user != null)
                    throw new UserNationalCodeAlreadyExistException(nationalCode);
            }
        }

        private async Task<ValidationResult> validateCreateAsync(CreateUserDto model) {
            model.CheckArgumentIsNull(nameof(model));

            try {
                validateRequiredFields(model.Username, model.FirstName, model.LastName, model.NationalCode);
            }
            catch(RequiredFieldException ex) {
                if (ex.FieldName == "firstName")
                    ValidationResult.Failed(ServiceMessages.Req_FirstName);
                if (ex.FieldName == "lastName")
                    ValidationResult.Failed(ServiceMessages.Req_LastName);
                if (ex.FieldName == "nationalCode")
                    ValidationResult.Failed(ServiceMessages.Req_NationalCode);
            }
            
            try {
                await validateNationalCodeAsync(model.NationalCode, checkForDuplicate: true);
            }
            catch(InvalidNationalCodeException ex) {
                ValidationResult.Failed(ServiceMessages.Invalid_NationalCode);
            }
            catch(UserNationalCodeAlreadyExistException) {
                ValidationResult.Failed(string.Format(
                    ServiceMessages.Exist_NationalCode, model.NationalCode)
                );
            }

            return ValidationResult.Success();
        }

        #endregion

    }
}
