using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Resources;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Extensions
{
    public static class EnumSelectListProvider
    {

        public static IEnumerable<SelectListItem> GetEntityStatusItems(EntityStatus? status = null)
            => new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EnumText.EntityStatus_Enabled,
                        Value = ((int)EntityStatus.Enabled).ToString(),
                        Selected = status == EntityStatus.Enabled
                    },
                    new SelectListItem
                    {
                        Text = EnumText.EntityStatus_Disabled,
                        Value = ((int)EntityStatus.Disbaled).ToString(),
                        Selected = status == EntityStatus.Disbaled
                    }
                    //new SelectListItem
                    //{
                    //    Text = EnumText.EntityStatus_Deleted,
                    //    Value = ((int)EntityStatus.Deleted).ToString(),
                    //    Selected = status == EntityStatus.Deleted
                    //}
                };
        public static IEnumerable<SelectListItem> GetActivityTypeItems(ActivityType? activity = null)
            => new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EnumText.ActivityType_Water,
                        Value = ((int)ActivityType.Water).ToString(),
                        Selected = activity == ActivityType.Water
                    },
                    new SelectListItem
                    {
                        Text = EnumText.ActivityType_Waste,
                        Value = ((int)ActivityType.Waste).ToString(),
                        Selected = activity == ActivityType.Waste
                    }
                };
        public static IEnumerable<SelectListItem> GetOrganizationTypeItem(OrganizationType? type = null)
        {
            var result = new List<SelectListItem>
             {
                    new SelectListItem
                    {
                        Text = EnumText.OrganizationType_County,
                        Value = ((int)OrganizationType.County).ToString(),
                        Selected = type == OrganizationType.County
                    },
                    new SelectListItem
                    {
                        Text = EnumText.OrganizationType_City,
                        Value = ((int)OrganizationType.City).ToString(),
                        Selected = type == OrganizationType.City
                    },
                    new SelectListItem
                    {
                        Text = EnumText.OrganizationType_Village,
                        Value = ((int)OrganizationType.Village).ToString(),
                        Selected = type == OrganizationType.Village
                    }
             };
            if (type.HasValue)
            {
                if (type == OrganizationType.Root)
                {
                    result.Clear();
                    result.Insert(0, new SelectListItem
                    {
                        Value = ((int)OrganizationType.Root).ToString(),
                        Text = EnumText.OrganizationType_Root,
                        Selected = type == OrganizationType.Root
                    });
                }
            }
            return result;
        }
        public static IEnumerable<SelectListItem> GetSectionNameTypeItem(SectionName? sectionName = null)
            => new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "آب",
                        Value = ((int)SectionName.A).ToString(),
                        Selected = sectionName == SectionName.A
                    },
                    new SelectListItem
                    {
                        Text = "فاضلاب",
                        Value = ((int)SectionName.B).ToString(),
                        Selected = sectionName == SectionName.B
                    },
                    new SelectListItem
                    {
                        Text = "انبار و تدارکات",
                        Value = ((int)SectionName.C).ToString(),
                        Selected = sectionName == SectionName.C
                    }
                };

        public static IEnumerable<SelectListItem> GetReportParamTypes(ReportParamType? paramType = null)
            => new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = EnumText.ReportParamType_Text,
                    Value = ((int)ReportParamType.Text).ToString(),
                    Selected = paramType == ReportParamType.Text
                },
                new SelectListItem
                {
                    Text = EnumText.ReportParamType_Year,
                    Value = ((int)ReportParamType.Year).ToString(),
                    Selected = paramType == ReportParamType.Year
                },
                new SelectListItem
                {
                    Text = EnumText.ReportParamType_Organization,
                    Value = ((int)ReportParamType.Organization).ToString(),
                    Selected = paramType == ReportParamType.Organization
                },
                new SelectListItem
                {
                    Text = EnumText.OrganizationType_County,
                    Value = ((int)ReportParamType.County).ToString(),
                    Selected = paramType == ReportParamType.County
                },
                new SelectListItem
                {
                    Text = EnumText.OrganizationType_City,
                    Value = ((int)ReportParamType.City).ToString(),
                    Selected = paramType == ReportParamType.City
                },
                new SelectListItem
                {
                    Text = EnumText.OrganizationType_Village,
                    Value = ((int)ReportParamType.Village).ToString(),
                    Selected = paramType == ReportParamType.Village
                }
            };
        public static IEnumerable<SelectListItem> GetRecordTypeItems(RecordType? recordType = null)
            => new List<SelectListItem>
            {
                 new SelectListItem
                 {
                    Text = EnumText.RecordType_Forcast,
                    Value = ((int)RecordType.Forcast).ToString(),
                    Selected = recordType == RecordType.Forcast
                 },
                 new SelectListItem
                 {
                    Text = EnumText.RecordType_Base,
                    Value = ((int)RecordType.Base).ToString(),
                    Selected = recordType == RecordType.Base
                 }
            };

    }
}
