using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Enum;
using Datiss.Budget.Common;
using Datiss.Budget.Security;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts.Identity;
using DNTPersianUtils.Core;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Datiss.Budget.Services.Identity
{

    public class UserService : IUserService
    {

        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _dbSet;
        private readonly IUserContext _userContext;
        private readonly IDateService _dateService;
        private readonly IOrganizationService _organizationService;
        private readonly IApplicationUserManager _userManager;

        public UserService(
            IUnitOfWork uow,
            IUserContext userContext,
            IDateService dateService,
            IOrganizationService organizationService,
            IApplicationUserManager userManager)
        {
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

        public async Task<UserResultDTO> GetByIdAsync(int id)
        {
            var user = await _dbSet
                .Include(_ => _.Organization)
                .Include(_ => _.Position)
                .Include(_ => _.Roles)
                .SingleOrDefaultAsync(_ => _.Id == id);
            user.CheckReferenceIsNull(nameof(user));

            return await Task.FromResult(user.Adapt<UserResultDTO>());
        }

        public async Task<ValidationResult<UserResultDTO>> CreateAsync(CreateUserDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            //First normalize model data
            model.NationalCode = model.NationalCode?.ToEnglishNumbers();
            model.FirstName = model.FirstName?.ApplyCorrectYeKe().Trim();
            model.LastName = model.LastName?.ApplyCorrectYeKe().Trim();
            model.UserName = model.UserName?.ApplyCorrectYeKe().Trim();
            model.Email = model.Email?.Trim();
            model.PhoneNumber = model.PhoneNumber?.ToEnglishNumbers();

            //Validate
            var validation = await validateCreateAsync(model);
            if (validation.NotValid)
                return ValidationResult<UserResultDTO>
                    .Failed(ValidationMode.Create, validation.Message);

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                return ValidationResult<UserResultDTO>.Failed(
                    existingUser.Adapt<UserResultDTO>(),
                    ValidationMode.Create,
                    string.Format(ServiceMessages.Exist_Username, model.UserName));
            }

            //Create
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                NationalCode = model.NationalCode,
                PhoneNumber = model.PhoneNumber,
                PositionId = model.PositionId,
                Status = EntityStatus.Enabled,
                TwoFactorEnabled = false,
                UserName = model.UserName,
                OrganizationId = model.OrganizationId,
                CreatedDateTime = _dateService.Now
            };

            foreach (var roleId in model.SelectedRoles)
            {
                user.Roles.Add(new UserRole
                {
                    RoleId = roleId
                });
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new CreateUserException(result.Errors);
            }

            //var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);
            //if (!passwordResult.Succeeded) {
            //    return ValidationResult<UserResultDTO>.Failed(
            //        user.Adapt<UserResultDTO>(),
            //        ValidationMode.Update,
            //        ServiceMessages.Err_Password_Format);
            //}

            return ValidationResult<UserResultDTO>
                    .Success(user.Adapt<UserResultDTO>());
        }

        public async Task<ValidationResult<UserResultDTO>> UpdateAsync(UpdateUserDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            //First normalize model data
            model.NationalCode = model.NationalCode?.ToEnglishNumbers();
            model.FirstName = model.FirstName?.ApplyCorrectYeKe().Trim();
            model.LastName = model.LastName?.ApplyCorrectYeKe().Trim();
            model.UserName = model.UserName?.ApplyCorrectYeKe().Trim();
            model.Email = model.Email?.Trim();
            model.PhoneNumber = model.PhoneNumber?.ToEnglishNumbers();

            //Validate model's data
            var validation = await validateUpdateAsync(model);
            if (validation.NotValid)
                return ValidationResult<UserResultDTO>
                    .Failed(ValidationMode.Update, validation.Message);

            var existingUser = await _dbSet.FirstOrDefaultAsync(_ => _.UserName.ToUpper() == model.UserName.ToUpper()
                                                                        && _.Id != model.Id);
            if (existingUser != null)
            {
                return ValidationResult<UserResultDTO>.Failed(
                    existingUser.Adapt<UserResultDTO>(),
                    ValidationMode.Update,
                    string.Format(ServiceMessages.Exist_Username, model.UserName));
            }

            //Update entity
            var user = await _dbSet
                .Include(_ => _.Roles)
                .SingleOrDefaultAsync(_ => _.Id == model.Id);
            user.CheckReferenceIsNull(nameof(user));
            user.FirstName = model.FirstName.ApplyCorrectYeKe().Trim();
            user.LastName = model.LastName.ApplyCorrectYeKe().Trim();
            user.Email = model.Email.Trim();
            user.NationalCode = model.NationalCode;
            user.PhoneNumber = model.PhoneNumber;
            user.PositionId = model.PositionId;
            user.Status = model.Status;
            user.UserName = model.UserName;
            user.OrganizationId = model.OrganizationId;

            user.Roles.Clear();
            foreach (var roleId in model.SelectedRoles)
            {
                user.Roles.Add(new UserRole
                {
                    RoleId = roleId
                });
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new UpdateUserException(result.Errors);

            //if (!string.IsNullOrWhiteSpace(model.Password)) {
            //    var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);
            //    if(!passwordResult.Succeeded) {
            //        return ValidationResult<UserResultDTO>.Failed(
            //            user.Adapt<UserResultDTO>(),
            //            ValidationMode.Update,
            //            ServiceMessages.Err_Password_Format);
            //    }
            //}

            return ValidationResult<UserResultDTO>
                    .Success(user.Adapt<UserResultDTO>());
        }

        public async Task<PagedResult<UserResultDTO>> GetListAsync(UserFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<UserResultDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = await setFilterAsync(_dbSet.AsNoTracking(), filter);

            result.TotalCount = await query.CountAsync();

            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query
                .OrderByDescending(_ => _.Id)
                .Select(x => new UserResultDTO
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Password = x.PasswordHash,
                    PhoneNumber = x.PhoneNumber,
                    PositionId = x.PositionId,
                    PositionTitle = x.Position.Title,
                    CreatedDateTime = x.CreatedDateTime,
                    LastVisitDateTime = x.LastVisitDateTime,
                    IsEmailPublic = x.IsEmailPublic,
                    NationalCode = x.NationalCode,
                    OrganizationId = x.OrganizationId,
                    OrganizationTitle = x.Organization.Title,
                    Status = x.Status
                })
                .ToListAsync();
            return await Task.FromResult(result);
        }

        public async Task<bool> HasAccessToOrganizationAsync(int organizationId)
        {
            if (_userContext.OrganizationId == null
                || _userContext.OrganizationId == organizationId)
                return true;

            return await _organizationService.IsDescendentOfAsync(
                _userContext.OrganizationId.Value,
                organizationId
            );
        }

        public async Task SetUserStatusAsync(int id, EntityStatus status)
        {
            var user = await _dbSet.FindAsync(id);
            user.CheckReferenceIsNull(nameof(user));

            user.Status = status;
            _dbSet.Update(user);
            await _uow.SaveChangesAsync();
        }

        public async Task SetUserPasswordAsync(int userId, string newPassword)
        {
            var user = await _dbSet.FindAsync(userId);
            user.CheckReferenceIsNull(nameof(user));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                throw new UserChangePasswordException(result.Errors);
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

        private async Task validateNationalCodeAsync(string nationalCode, int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
                throw new RequiredFieldException(nameof(nationalCode));

            if (!nationalCode.IsValidIranianNationalCode())
                throw new InvalidNationalCodeException(nationalCode);

            //Check if the user with the given NationalCode already existed in DB.
            if (userId.HasValue)
            {
                var existingUser = await _dbSet.FirstOrDefaultAsync(_ => _.NationalCode == nationalCode
                                                                            && _.Id != userId.Value);
                if (existingUser != null)
                    throw new UserNationalCodeAlreadyExistException(nationalCode);
            }

            var user = await _dbSet.FirstOrDefaultAsync(_ => _.NationalCode == nationalCode);
            if (user != null)
                throw new UserNationalCodeAlreadyExistException(nationalCode);
        }

        private async Task<ValidationResult> validateCreateAsync(CreateUserDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                validateRequiredFields(model.UserName, model.FirstName, model.LastName, model.NationalCode);
            }
            catch (RequiredFieldException ex)
            {
                if (ex.FieldName == "userName")
                    ValidationResult.Failed(ServiceMessages.Req_Username);
                if (ex.FieldName == "firstName")
                    ValidationResult.Failed(ServiceMessages.Req_FirstName);
                if (ex.FieldName == "lastName")
                    ValidationResult.Failed(ServiceMessages.Req_LastName);
                if (ex.FieldName == "nationalCode")
                    ValidationResult.Failed(ServiceMessages.Req_NationalCode);
            }

            try
            {
                await validateNationalCodeAsync(model.NationalCode);
            }
            catch (InvalidNationalCodeException ex)
            {
                ValidationResult.Failed(ServiceMessages.Invalid_NationalCode);
            }
            catch (UserNationalCodeAlreadyExistException)
            {
                ValidationResult.Failed(string.Format(
                    ServiceMessages.Exist_NationalCode, model.NationalCode)
                );
            }

            return ValidationResult.Success();
        }


        private async Task<ValidationResult> validateUpdateAsync(UpdateUserDTO model)
        {
            model.CheckArgumentIsNull(nameof(model));

            try
            {
                validateRequiredFields(model.UserName, model.FirstName, model.LastName, model.NationalCode);
            }
            catch (RequiredFieldException ex)
            {
                if (ex.FieldName == "userName")
                    ValidationResult.Failed(ServiceMessages.Req_Username);
                if (ex.FieldName == "firstName")
                    ValidationResult.Failed(ServiceMessages.Req_FirstName);
                if (ex.FieldName == "lastName")
                    ValidationResult.Failed(ServiceMessages.Req_LastName);
                if (ex.FieldName == "nationalCode")
                    ValidationResult.Failed(ServiceMessages.Req_NationalCode);
            }

            try
            {
                await validateNationalCodeAsync(model.NationalCode, userId: model.Id);
            }
            catch (InvalidNationalCodeException ex)
            {
                ValidationResult.Failed(ServiceMessages.Invalid_NationalCode);
            }
            catch (UserNationalCodeAlreadyExistException)
            {
                ValidationResult.Failed(string.Format(
                    ServiceMessages.Exist_NationalCode, model.NationalCode)
                );
            }

            return ValidationResult.Success();
        }

        private async Task<IQueryable<User>> setFilterAsync(IQueryable<User> query, UserFilterDTO filter)
        {
            filter.CheckArgumentIsNull(nameof(filter));

            if (filter.UserName.IsNotNullOrEmpty())
            {
                filter.UserName = filter.UserName.ToUpper();
                query = query.Where(_ => _.UserName.ToUpper().Contains(filter.UserName));
            }

            if (filter.DisplayName.IsNotNullOrEmpty())
            {
                filter.DisplayName = filter.DisplayName.ApplyCorrectYeKe().ToUpper();
                query = query.Where(_ => _.FirstName.ToUpper().Contains(filter.DisplayName) ||
                                            _.LastName.ToUpper().Contains(filter.DisplayName));
            }

            if (filter.NationalCode.IsNotNullOrEmpty())
            {
                filter.NationalCode = filter.NationalCode.ToEnglishNumbers();
                query = query.Where(_ => _.NationalCode.Contains(filter.NationalCode));
            }

            if (filter.PhoneNumber.IsNotNullOrEmpty())
            {
                filter.PhoneNumber = filter.PhoneNumber.ToEnglishNumbers().Trim();
                query = query.Where(_ => _.PhoneNumber.Contains(filter.PhoneNumber));
            }

            if (filter.OrganizationId.HasValue)
            {
                var predicate = PredicateBuilder.New<User>();

                var organizations = await _organizationService
                    .GetWithChildrenAsync(filter.OrganizationId.Value);

                foreach (var org in organizations)
                    predicate.Or(_ => _.OrganizationId == org.Id);

                query = query.Where(predicate);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(_ => _.Status == filter.Status.Value);
            }

            return query;
        }

        #endregion

    }
}
