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
                _ => EnumText.Unknown
            };
        public static string ToDisplay(this ActivityType activity)
            => activity switch
            {
                ActivityType.Water => EnumText.ActivityType_Water,
                ActivityType.Waste => EnumText.ActivityType_Waste,
                _ => EnumText.Unknown
            };
        public static string ToDisplay(this TablesName tables)
            => tables switch
            {
                TablesName.CurrentIncome => EnumText.TablesName_CurrentIncome,
                TablesName.CurrentCost => EnumText.TablesName_CurrentCost,
                TablesName.OtherCurrentCost => EnumText.TablesName_OtherCurrentCost,
                TablesName.ResourcesFunction => EnumText.TablesName_ResourcesFunction,
                TablesName.ConsumptionFunction => EnumText.TablesName_ConsumptionFunction,
                TablesName.WTotalBudget => EnumText.TablesName_WTotalBudget,
                TablesName.WsTotalBudget => EnumText.TablesName_WsTotalBudget,
                _ => EnumText.Unknown
            };
        public static string ToDisplay(this CofficientsGroup group)
            => group switch
            {
                CofficientsGroup.CurrentIncome => EnumText.CofficientsGroup_CurrentIncome,
                CofficientsGroup.CurrentCost => EnumText.CofficientsGroup_CurrentCost,
                CofficientsGroup.ForcastCost => EnumText.CofficientsGroup_ForcastCost,
                CofficientsGroup.ForcastIncome => EnumText.CofficientsGroup_ForcastIncome,
                _ => EnumText.Unknown
            };

        public static string ToDisplay(this ReportParamType paramType)
            => paramType switch
            {
                ReportParamType.FirstConstant => EnumText.ReportParamType_Constant,
                ReportParamType.SecondConstant => EnumText.ReportParamType_Constant2,
                ReportParamType.ThirdConstant => EnumText.ReportParamType_Constant3,
                ReportParamType.Date => EnumText.ReportParamType_Date,
                ReportParamType.Organization => EnumText.ReportParamType_Organization,
                ReportParamType.Year => EnumText.ReportParamType_Year,
                ReportParamType.Number => EnumText.ReportParamType_Number,
                ReportParamType.Text => EnumText.ReportParamType_Text,
                ReportParamType.County => EnumText.OrganizationType_County,
                ReportParamType.City => EnumText.ReportParamType_City,
                ReportParamType.Village => EnumText.ReportParamType_Village,
                _ => EnumText.Unknown
            };
        public static string ToDisplay(this RecordType recordType)
            => recordType switch
            {
                RecordType.Base => EnumText.RecordType_Base,
                RecordType.Forcast => EnumText.RecordType_Forcast,
                _ => EnumText.Unknown
            };

    }
}
