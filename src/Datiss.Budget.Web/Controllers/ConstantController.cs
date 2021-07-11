using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ConstantController : Controller
    {

        private readonly IConstantService _constantService;

        public ConstantController(IConstantService constantService) {
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));

        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New() 
        {
            var parentList = await _constantService.GetParentsAsync();
            var model = new AddConstantViewModel {
                ParentList = parentList.Select(x=> new SelectListItem { 
                    Value = x.Id.ToString(),
                    Text = x.Title
                })
            };

            return View(model);
        }

    }
}
