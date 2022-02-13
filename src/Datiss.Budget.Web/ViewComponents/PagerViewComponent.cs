using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Datiss.Budget.Common.GuardToolkit;

namespace Datiss.Budget.Web
{
    public class PagerViewComponent : ViewComponent
    {

        public PagerViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(PagerViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));

            return View(model);
        }
    }
}
