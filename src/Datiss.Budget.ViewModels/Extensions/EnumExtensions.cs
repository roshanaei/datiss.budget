using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.ViewModels.Extensions
{
    public static class EnumExtensions
    {


        public static IEnumerable<SelectListItem> GetOrganizationTypeListItems(OrganizationType? selected = null)
            => new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "شهر",
                    Value = ((int)OrganizationType.City).ToString(),
                    Selected = selected == OrganizationType.City
                }
            };


        public static void AddEmptySelectItem(this IList<SelectListItem> dropdown)
            => dropdown.Insert(0, new SelectListItem
            {
                Text = "[انتخاب کنید]",
                Value = null
            });

    }
}