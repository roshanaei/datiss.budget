using Datiss.Budget.Resources;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class IncomeCurrentWsHController : Controller
    {
        public const string Name = "IncomeCurrentWsH";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Index = nameof(Index);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<IncomeCurrentWsHController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IIncomeCurrentWsHService _incomeCurrentWsHService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public IncomeCurrentWsHController(
            ILogger<IncomeCurrentWsHController> logger,
            IWebHostEnvironment environment,
            IIncomeCurrentWsHService incomeCurrentWsHService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _incomeCurrentWsHService = incomeCurrentWsHService ?? throw new ArgumentNullException(nameof(incomeCurrentWsHService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateIncomeCurrentWsHViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<CreateIncomeCurrentWsHDTO>();

            var result = await _incomeCurrentWsHService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<IncomeCurrentWsHViewModel>());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateIncomeCurrentWsHViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateIncomeCurrentWsHDTO>();
            var result = await _incomeCurrentWsHService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<IncomeCurrentWsHViewModel>()
            );
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
