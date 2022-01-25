using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;

namespace Datiss.Budget.ViewModels
{

    public static class EnumDisplayProvider
    {
        public static string ToDisplay(this EntityStatus status)
            => status switch
            {
                EntityStatus.Deleted => EnumText.EntityStatus_Deleted,
                EntityStatus.Disbaled => EnumText.EntityStatus_Disabled,
                EntityStatus.Enabled => EnumText.EntityStatus_Enabled,
                _ => EnumText.Unknown
            };
        public static string ToDisplay(this OrganizationType orgType)
            => orgType switch
            {
                OrganizationType.Root => EnumText.OrganizationType_Root,
                OrganizationType.County => EnumText.OrganizationType_County,
                OrganizationType.City => EnumText.OrganizationType_City,
                OrganizationType.Village => EnumText.OrganizationType_Village,
                _=>EnumText.Unknown
            };
        public static string ToDisplay(this ActivityType activity)
            => activity switch
            {
                ActivityType.Water => EnumText.ActivityType_Water,
                ActivityType.Waste => EnumText.ActivityType_Waste,
                _=>EnumText.Unknown
            };
        public static string ToDisplay(this TablesName tables)
            => tables switch
            {
                TablesName.CurrentIncome => EnumText.TablesName_CurrentIncome,
                TablesName.CurrentCost => EnumText.TablesName_CurrentCost,
                _=>EnumText.Unknown
            };
        public static string ToDisplay(this CofficientsGroup group)
            => group switch
            {
                CofficientsGroup.CurrentIncome => EnumText.CofficientsGroup_CurrentIncome,
                CofficientsGroup.CurrentCost => EnumText.CofficientsGroup_CurrentCost,
                CofficientsGroup.ForcastCost => EnumText.CofficientsGroup_ForcastCost,
                _ => EnumText.Unknown
            };
    }
}
