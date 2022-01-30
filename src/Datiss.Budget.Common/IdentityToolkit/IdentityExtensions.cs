using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Datiss.Budget.Common.IdentityToolkit
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2580
    /// And http://www.dotnettips.info/post/2579
    /// </summary>
    public static class IdentityExtensions
    {
        public static void AddErrorsFromResult(this ModelStateDictionary modelStat, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelStat.AddModelError("", error.Description);
            }
        }

        /// <summary>
        /// IdentityResult errors list to string
        /// </summary>
        public static string DumpErrors(this IdentityResult result, bool useHtmlNewLine = false)
        {
            var results = new StringBuilder();
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    var errorDescription = error.Description;
                    if (string.IsNullOrWhiteSpace(errorDescription))
                    {
                        continue;
                    }

                    if (!useHtmlNewLine)
                    {
                        results.AppendLine(errorDescription);
                    }
                    else
                    {
                        results.Append(errorDescription).AppendLine("<br/>");
                    }
                }
            }
            return results.ToString();
        }

        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
            => identity?.FindFirst(claimType)?.Value;

        public static string GetUserClaimValue(this IIdentity identity, string claimType) {
            var identity1 = identity as ClaimsIdentity;
            return identity1?.FindFirstValue(claimType);
        }
        public static string GetUserFirstName(this IIdentity identity)
            => identity?.GetUserClaimValue(ClaimTypes.GivenName);

        public static string GetUserLastName(this IIdentity identity)
           => identity?.GetUserClaimValue(ClaimTypes.Surname);

        public static string GetUserFullName(this IIdentity identity)
            => $"{GetUserFirstName(identity)} {GetUserLastName(identity)}";

        public static int GetUserId(this IIdentity identity) {
            var value = identity?.GetUserClaimValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("UserId claim not found.");

            if (int.TryParse(value, out int userId))
                return userId;

            throw new InvalidOperationException("UserId value is not a valid Int64 number.");
        }

        public static string GetUserDisplayName(this IIdentity identity)
            => identity
                    ?.GetUserClaimValue(ClaimTypes.Name)
                    ?? identity.GetUserFullName();

        public static int? GetOrganizationId(this IIdentity identity) {
            var claim_value = identity?.
                GetUserClaimValue(BudgetClaimTypes.OrganizationId);
            if (string.IsNullOrWhiteSpace(claim_value))
                return null;
            // TODO : Check security Issue with this claim
            return int.Parse(claim_value);
        }

        public static string GetOrganizationTitle(this IIdentity identity)
            => identity?.GetUserClaimValue(BudgetClaimTypes.OrganizationTitle);
                    
    }
}