using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.IdentityToolkit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Datiss.Budget.Security
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        int UserId { get; }
        string DisplayName { get; }
        int? OrganizationId { get; }

    }

    public class UserContext : IUserContext
    {

        public UserContext(IHttpContextAccessor httpContextAccessor) {
            httpContextAccessor.CheckArgumentIsNull(nameof(httpContextAccessor));
            Principal = httpContextAccessor.HttpContext?.User;
            IsAuthenticated = Principal?.Identity.IsAuthenticated ?? false;

            if (IsAuthenticated)
                loadData(httpContextAccessor.HttpContext);
        }

        protected ClaimsPrincipal Principal { get; }

        public bool IsAuthenticated { get; }

        public int UserId { get; protected set; }

        public string DisplayName { get; protected set; }

        public int? OrganizationId { get; set; }

        private void loadData(HttpContext httpContext) {
            UserId = Principal.Identity.GetUserId();
            DisplayName = Principal.Identity.GetUserDisplayName();
            OrganizationId = Principal.Identity.GetOrganizationId();
        }
    }
}
