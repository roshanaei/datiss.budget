using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Enum;
using Datiss.Budget.Common;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts.Identity;
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
        private readonly IDateService _dateService;
        private readonly IOrganizationService _organizationService;
        private readonly IApplicationUserManager _userManager;

        public UserService(
            IUnitOfWork uow, 
            IUserContext userContext,
            IDateService dateService,
            IOrganizationService organizationService,
            IApplicationUserManager userManager) {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _dateService = dateService ?? throw new ArgumentNullException(nameof(DateService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dbSet = _uow.Set<User>();
        }

        private IQueryable<User> Query()
            => _dbSet.Where(_ => _.Status != EntityStatus.Deleted).AsNoTracking();

        private IQueryable<User> QueryActiveUsers()
            => _dbSet.Where(_ => _.Status != EntityStatus.Enabled).AsNoTracking();


        public async Task<ValidationResult<UserResultDTO>> CreateAsync(CreateUserDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            var validation = await validateCreateAsync(model);
            if (validation.NotValid)
                return ValidationResult<UserResultDTO>
                    .Failed(ValidationMode.Create, validation.Message);

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if(existingUser != null) {
                return ValidationResult<UserResultDTO>.Failed(
                    existingUser.Adapt<UserResultDTO>(),
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
                OrganizationId = model.OrganizationId,
                CreatedDateTime = _dateService.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded) {
                throw new CreateUserException(result.Errors);
            }

            var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);
            if (!passwordResult.Succeeded) {
                return ValidationResult<UserResultDTO>.Failed(
                    user.Adapt<UserResultDTO>(),
                    ValidationMode.Update,
                    ServiceMessages.Err_Password_Format);
            }

            return ValidationResult<UserResultDTO>
                    .Success(user.Adapt<UserResultDTO>());
        }

        public async Task<ValidationResult<UserResultDTO>> UpdateAsync(UpdateUserDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            var validation = await validateUpdateAsync(model);
            if (validation.NotValid)
                return ValidationResult<UserResultDTO>
                    .Failed(ValidationMode.Update, validation.Message);

            var existingUser = await _dbSet.FirstOrDefaultAsync(_ => _.UserName.ToUpper() == model.Username.ToUpper()
                                                                        && _.Id != model.Id);
            if(existingUser != null) {
                return ValidationResult<UserResultDTO>.Failed(
                    existingUser.Adapt<UserResultDTO>(),
                    ValidationMode.Update,
                    string.Format(ServiceMessages.Exist_Username, model.Username));
            }

            var user = await _dbSet.FindAsync(model.Id);
            user.CheckReferenceIsNull(nameof(user));
            user.FirstName = model.FirstName.ApplyCorrectYeKe().Trim();
            user.LastName = model.LastName.ApplyCorrectYeKe().Trim();
            user.Email = model.Email.Trim();
            user.NationalCode = model.NationalCode;
            user.PhoneNumber = model.PhoneNumber;
            user.PositionId = model.PositionId;
            user.Status = model.Status;
            user.UserName = model.Username;
            user.OrganizationId = model.OrganizationId;

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded) {
                throw new UpdateUserException(result.Errors);
            }

            if (!string.IsNullOrWhiteSpace(model.Password)) {
                var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if(!passwordResult.Succeeded) {
                    return ValidationResult<UserResultDTO>.Failed(
                        user.Adapt<UserResultDTO>(),
                        ValidationMode.Update,
                        ServiceMessages.Err_Password_Format);
                }
            }

            return ValidationResult<UserResultDTO>
                    .Success(user.Adapt<UserResultDTO>());
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

        private async Task validateNationalCodeAsync(string nationalCode, int? userId = null) {
            if (string.IsNullOrWhiteSpace(nationalCode))
                throw new RequiredFieldException(nameof(nationalCode));

            if (!nationalCode.IsValidIranianNationalCode())
                throw new InvalidNationalCodeException(nationalCode);

            //Check if the user with the given NationalCode already existed in DB.
            if (userId.HasValue) {
                var existingUser = await _dbSet.FirstOrDefaultAsync(_ => _.NationalCode == nationalCode 
                                                                            && _.Id != userId.Value);
                if (existingUser != null)
                    throw new UserNationalCodeAlreadyExistException(nationalCode);
            }
            
            var user = await _dbSet.FirstOrDefaultAsync(_ => _.NationalCode == nationalCode);
            if (user != null)
                throw new UserNationalCodeAlreadyExistException(nationalCode);
        }

        private async Task<ValidationResult> validateCreateAsync(CreateUserDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            try {
                validateRequiredFields(model.Username, model.FirstName, model.LastName, model.NationalCode);
            }
            catch(RequiredFieldException ex) {
                if (ex.FieldName == "userName")
                    ValidationResult.Failed(ServiceMessages.Req_Username);
                if (ex.FieldName == "firstName")
                    ValidationResult.Failed(ServiceMessages.Req_FirstName);
                if (ex.FieldName == "lastName")
                    ValidationResult.Failed(ServiceMessages.Req_LastName);
                if (ex.FieldName == "nationalCode")
                    ValidationResult.Failed(ServiceMessages.Req_NationalCode);
            }
            
            try {
                await validateNationalCodeAsync(model.NationalCode);
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


        private async Task<ValidationResult> validateUpdateAsync(UpdateUserDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            try {
                validateRequiredFields(model.Username, model.FirstName, model.LastName, model.NationalCode);
            }
            catch (RequiredFieldException ex) {
                if (ex.FieldName == "userName")
                    ValidationResult.Failed(ServiceMessages.Req_Username);
                if (ex.FieldName == "firstName")
                    ValidationResult.Failed(ServiceMessages.Req_FirstName);
                if (ex.FieldName == "lastName")
                    ValidationResult.Failed(ServiceMessages.Req_LastName);
                if (ex.FieldName == "nationalCode")
                    ValidationResult.Failed(ServiceMessages.Req_NationalCode);
            }

            try {
                await validateNationalCodeAsync(model.NationalCode, userId : model.Id);
            }
            catch (InvalidNationalCodeException ex) {
                ValidationResult.Failed(ServiceMessages.Invalid_NationalCode);
            }
            catch (UserNationalCodeAlreadyExistException) {
                ValidationResult.Failed(string.Format(
                    ServiceMessages.Exist_NationalCode, model.NationalCode)
                );
            }

            return ValidationResult.Success();
        }

        #endregion

    }
}
