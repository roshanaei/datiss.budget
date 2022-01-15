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
            => new List<SelectListItem>
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
    }
}
