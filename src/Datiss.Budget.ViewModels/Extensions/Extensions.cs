using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Datiss.Budget.ViewModels
{

    public static class Extensions
    {

        public static IList<SelectListItem> AddEmptySelectListItem(this IList<SelectListItem> items) {
            items.Insert(0, new SelectListItem {
                Value = null,
                Text = "[انتخاب کنید]"
            });

            return items;
        }

        public static string ToStringArray(this IEnumerable<SelectListItem> items) {
            string result = "";
            foreach(var item in items) {
                result += $"'{item.Value}':'{item.Text}',";
            }
            if (result == "") return result;

            return result.Substring(0, result.Length -1);
        }
    }
}
