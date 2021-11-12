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
    }
}
