using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Resources;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Extensions
{
    public static class EnumSelectListProvider
    {

        public static IEnumerable<SelectListItem> GetEntityStatusItems(EntityStatus? status)
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
                    },
                    new SelectListItem
                    {
                        Text = EnumText.EntityStatus_Deleted,
                        Value = ((int)EntityStatus.Deleted).ToString(),
                        Selected = status == EntityStatus.Deleted
                    }
                };
    }
}
