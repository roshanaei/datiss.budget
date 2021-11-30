using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class FinanceYearController : Controller
    {
        public const string Name = "FinanceYear";
        //public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        //public const string ACTION_Edit = nameof(Edit);
        //public const string ACTION_Delete = nameof(Delete);

        private readonly IFinanceYearService _financeYearService;

        public FinanceYearController(IFinanceYearService financeYearService){

            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));

        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filterInput = new FinanceYearFilterDTO
            {
                OrderBy = "id",
                OrderDesc = false,
                PageNumber = page
            };

            var result = await _financeYearService.GetListAsync(filterInput);
            var model = result.Adapt<FinanceYearIndexViewModel>();
            model.SetOrganizationStatusFilterSource(
                (await _financeYearService.GetDropDownStatusAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>());
            return View(model);
        }
        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FinanceYearIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<FinanceYearFilterDTO>();

            var result = await _financeYearService.GetListAsync(filterInput);
            model = result.Adapt<FinanceYearIndexViewModel>();
            model.Filter = filterInput.Adapt<FinanceYearFilterViewModel>();
            model.SetOrganizationStatusFilterSource(
               (await _financeYearService.GetDropDownStatusAsync())
               .Adapt<IEnumerable<DropDownItemViewModel>>());
            return View(model);
        }
    }
}
