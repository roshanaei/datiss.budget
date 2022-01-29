using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Common.IdentityToolkit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Datiss.Budget.Security
{

    public class HasPermission : AuthorizeAttribute, IAuthorizationFilter {
        
        private readonly string _claimType;
        private readonly PermissionActionType _actionType;


        public HasPermission(
            string claimType,
            PermissionActionType actionType) {
            if (string.IsNullOrWhiteSpace(claimType))
                throw new ArgumentNullException(nameof(claimType));

            _claimType = claimType;
            _actionType = actionType;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            var user = context.HttpContext.User;
            if(user.Identity == null || !user.Identity.IsAuthenticated)
                context.Result = new ForbidResult();

            var claim = user.Identity.GetUserClaimValue(_claimType);
            var permissions = claim?.ExtractPermissionActionTypesFromString();
            if (permissions == null || !permissions.Contains(_actionType))
                context.Result = new ForbidResult();
        }
    }
}
