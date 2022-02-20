using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Common.GuardToolkit;

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

        public static PagerViewModel ToPager<T>(this PagedViewModel<T> model) where T: class
        {
            model.CheckArgumentIsNull(nameof(model));
            
            return new PagerViewModel
            {
                PageNumber = model.PageNumber,
                PagesCount = model.PagesCount,
                PageSize = model.PageSize,
                TotalCount = model.TotalCount
            };
        }
    }
}
