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
        string FirstName { get; }
        string LastName { get; }
        int? OrganizationId { get; }
        string OrganizationTitle { get; }
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

        public string FirstName { get; protected set; }

        public string LastName { get; protected set; }

        public int? OrganizationId { get; protected set; }

        public string OrganizationTitle { get; protected set; }

        private void loadData(HttpContext httpContext) {
            UserId = Principal.Identity.GetUserId();
            DisplayName = Principal.Identity.GetUserDisplayName();
            FirstName = Principal.Identity.GetUserFirstName();
            LastName = Principal.Identity.GetUserLastName();
            OrganizationId = Principal.Identity.GetOrganizationId();
            OrganizationTitle = Principal.Identity.GetOrganizationTitle();
        }
    }
}
