using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    public class WaterInstallFeeController : Controller
    {

        private readonly IWaterInstallFeeService _waterInstallFeeService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;

        public WaterInstallFeeController(
            IWaterInstallFeeService waterInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService) 
        {
            _waterInstallFeeService = waterInstallFeeService ?? throw new ArgumentNullException(nameof(waterInstallFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> Index(int page = 1) 
        {
            var filterInput = new WaterInstallFeeFilter {
                OrderBy = "dwatertype",
                PageNumber = page
            };

            var result = await _waterInstallFeeService.GetListAsync(filterInput);

            var model = new WaterInstallFeeIndexViewModel();
            model.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
            model.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync(null));
            model.Model = result;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterInstallFeeFilterViewModel model) 
        {
            var filterInput = model.Adapt<WaterInstallFeeFilter>();

            var result = await _waterInstallFeeService.GetListAsync(filterInput);

            return View(result);
        }
    }
}
