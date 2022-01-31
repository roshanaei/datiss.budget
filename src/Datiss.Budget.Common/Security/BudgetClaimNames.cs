using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Common.IdentityToolkit
{

    public static class BudgetClaimTypes
    {
        public static string OrganizationId = "OrganizationId";

        public static string OrganizationTitle = "OrganizationTitle";

        public static string BranchFeeAmount = nameof(BranchFeeAmount);
        public static string WaterInstallFee = nameof(WaterInstallFee);
        public static string WasteInstallFee = nameof(WasteInstallFee);

        public static string AverageContractedCapacityNHUses = nameof(AverageContractedCapacityNHUses);

    }

    public static class BudgetPermissions {
        public static string List = nameof(List).ToLower();
        public static string Create = nameof(Create).ToLower();
        public static string Edit = nameof(Edit).ToLower();
        public static string Delete = nameof(Delete).ToLower();
    }
}
