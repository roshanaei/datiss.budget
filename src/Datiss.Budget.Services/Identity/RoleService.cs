using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Resources;
using Mapster;
using DNTPersianUtils.Core;

namespace Datiss.Budget.Services.Identity
{

    public class RoleService : IRoleService
    {

        private readonly IApplicationRoleManager _roleManager;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _dbSet;

        public RoleService(
            IUnitOfWork uow,
            IApplicationRoleManager roleManager) 
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Role>();
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RoleDTO>> GetAllAsync() 
        {
            var result = await _dbSet.AsNoTracking()
                .Include(_=> _.Claims)
                .Where(_=> !_.IsConstantRole)
                .Select(_=> _.Adapt<RoleDTO>())
                .ToListAsync();

            return await Task.FromResult(result);
        }

        /// <inheritdoc />
        public async Task<RoleDTO> GetByIdAsync(int id)
        {
            var role = await _dbSet
                .Include(_ => _.Claims)
                .SingleOrDefaultAsync(_ => _.Id == id);

            var result = role.Adapt<RoleDTO>();
            foreach(var claim in role.Claims) 
            {
                result.Claims.Add(new RoleClaimDTO
                {
                    ClaimType = claim.ClaimType,
                    ClaimValue = claim.ClaimValue,
                    RoleId = role.Id,
                    RoleTitle = role.Title
                });
            }

            return await Task.FromResult(result);
        }

        /// <inheritdoc />
        public async Task<ValidationResult> CreateAsync(CreateRoleDTO model) 
        {
            normalizeModel(model);

            if (await existByNameAsync(model.Name))
                return ValidationResult.Failed(
                    ValidationMode.Create, 
                    ServiceMessages.Role_Name_Exist);

            if (await existByTitleAsync(model.Title))
                return ValidationResult.Failed(
                    ValidationMode.Create,
                    ServiceMessages.Role_Title_Exist);

            var role = model.Adapt<Role>();
            foreach(var claim in model.SelectedClaims) 
            {
                role.Claims.Add(new RoleClaim
                {
                    ClaimType = claim.Key,
                    ClaimValue = claim.Value
                });
            }

            _dbSet.Add(role);
            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        /// <inheritdoc />
        public async Task<ValidationResult> UpdateAsync(UpdateRoleDTO model) 
        {
            normalizeModel(model);

            if (await existByNameAsync(model.Name, model.Id))
                return ValidationResult.Failed(
                    ValidationMode.Update,
                    ServiceMessages.Role_Name_Exist);

            if (await existByTitleAsync(model.Title, model.Id))
                return ValidationResult.Failed(
                    ValidationMode.Update,
                    ServiceMessages.Role_Title_Exist);

            var role = await _dbSet.FindAsync(model.Id);
            role.CheckReferenceIsNull(nameof(role));
            role.Name = model.Name;
            role.Title = model.Title;
            role.Description = model.Description;
            _dbSet.Update(role);
            await _uow.SaveChangesAsync();

            return ValidationResult.Success();
        }

        #region helper methods

        private void normalizeModel(CreateRoleDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            model.Name = model.Name?.ApplyCorrectYeKe();
            model.Title = model.Title?.ApplyCorrectYeKe();
            model.Description = model.Description?.ApplyCorrectYeKe();
        }

        private void normalizeModel(UpdateRoleDTO model) {
            model.CheckArgumentIsNull(nameof(model));

            model.Name = model.Name?.ApplyCorrectYeKe();
            model.Title = model.Title?.ApplyCorrectYeKe();
            model.Description = model.Description?.ApplyCorrectYeKe();
        }

        private async Task<bool> existByNameAsync(string name, int? roleId = null)
            => roleId.HasValue
                ? await _dbSet.AnyAsync(_ => _.NormalizedName == name.ToUpper() && _.Id != roleId)
                : await _dbSet.AnyAsync(_ => _.NormalizedName == name.ToUpper());

        public async Task<bool> existByTitleAsync(string title, int? roleId = null)
            => roleId.HasValue
                ? await _dbSet.AnyAsync(_ => _.Title.ToUpper() == title.ToUpper() && _.Id != roleId)
                : await _dbSet.AnyAsync(_ => _.Title.ToUpper() == title.ToUpper());

        #endregion
    }
}
