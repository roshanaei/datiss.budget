using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Common.IdentityToolkit;

namespace Datiss.Budget.Security
{
    public enum PermissionActionType
    {
        List = 0,
        Create = 1,
        Edit = 2,
        Delete = 3
    }

    public static class PermissionExtensions {

        public static IEnumerable<PermissionActionType> ExtractPermissionActionTypesFromString(this string value) {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var result = new List<PermissionActionType>();
            value = value.ToLower();

            if(value.Contains(BudgetPermissions.Create))
                result.Add(PermissionActionType.Create);
            
            if(value.Contains(BudgetPermissions.Edit))
                result.Add(PermissionActionType.Edit);
            
            if(value.Contains(BudgetPermissions.Delete))
                result.Add(PermissionActionType.Delete);
            
            if(value.Contains(BudgetPermissions.List))
                result.Add(PermissionActionType.List);

            return result;
        }
    }
}
