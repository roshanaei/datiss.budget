using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;

namespace Datiss.Budget.Services.Identity
{
    /// <summary>
    /// Customizing claims transformation in ASP.NET Core Identity
    /// More info: http://www.dotnettips.info/post/2580
    /// </summary>
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public static readonly string PhotoFileName = nameof(PhotoFileName);

        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationUserManager _userManager;
        private readonly IUnitOfWork _uow;

        public ApplicationClaimsPrincipalFactory(
            IUnitOfWork uow,
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base((UserManager<User>)userManager, (RoleManager<Role>)roleManager, optionsAccessor)
        {
            _uow = uow;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var org = await _uow.Set<Organization>().FindAsync(user.OrganizationId);
            if (org != null)
                user.Organization = org;

            var principal = await base.CreateAsync(user); // adds all `Options.ClaimsIdentity.RoleClaimType -> Role Claims` automatically + `Options.ClaimsIdentity.UserIdClaimType -> userId` & `Options.ClaimsIdentity.UserNameClaimType -> userName`
            //add role claims
            var roles = await _uow.Set<UserRole>()
                .Include(_=> _.Role)
                .ThenInclude(_=> _.Claims)
                .Where(_ => _.UserId == user.Id)
                .ToListAsync();
            foreach(var role in roles) {
                var roleClaims = role.Role.Claims.Where(_ => _.RoleId == role.RoleId);  /*await _uow.Set<RoleClaim>().Where(_ => _.RoleId == role.RoleId).ToListAsync();*/
                foreach(var claim in roleClaims.Where(_=> !string.IsNullOrWhiteSpace(_.ClaimValue))) {
                    addPermissions(user, principal, claim.ClaimType, claim.ClaimValue);
                }
            }
            
            addCustomClaims(user, principal);
            return principal;
        }

        private static void addCustomClaims(User user, IPrincipal principal)
        {
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new Claim(PhotoFileName, user.PhotoFileName ?? string.Empty, ClaimValueTypes.String),

                new Claim(BudgetClaimTypes.OrganizationId,
                                user.OrganizationId.HasValue
                                    ? user.OrganizationId.ToString()
                                    : string.Empty),

                new Claim(BudgetClaimTypes.OrganizationTitle,
                                user.OrganizationId.HasValue
                                    ? user.Organization.Title
                                    : string.Empty)
            });
        }


        private static void addPermissions(User user, IPrincipal principal, string claimType, string claimValue) {
            ((ClaimsIdentity)principal.Identity).AddClaim(
                new Claim(claimType,claimValue)
                );
        }
    }
}