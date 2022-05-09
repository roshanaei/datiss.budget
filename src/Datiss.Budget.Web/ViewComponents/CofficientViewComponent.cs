using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Datiss.Budget.Common.GuardToolkit;

namespace Datiss.Budget.Web
{
    public class CofficientViewComponent : ViewComponent
    {

        public CofficientViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(PagerViewModel model,string controllerName,string actionName,string groupName)
        {
            model.CheckArgumentIsNull(nameof(model));
            model.ControllerName = controllerName;
            model.ActionName = actionName;
            model.GroupName = groupName;
            return View(model);
        }
    }
}
