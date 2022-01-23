using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Entities.DWH;
using System.Threading.Tasks;
using Datiss.Budget.Resources;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Mapster;

namespace Datiss.Budget.Web.Controllers
{

    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WWsFeeController : Controller
    {
        public const string Name = "WWsFee";
        public const string ACTION_Create = nameof(Create);


        private readonly ILogger<WWsFee> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConsumeForcastService _consumeForcastService;
        private readonly IWWsFeeService _wWsFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public WWsFeeController(
            ILogger<WWsFee> logger,
            IWebHostEnvironment environment,
            IWWsFeeService wWsFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wWsFeeService = wWsFeeService ?? throw new ArgumentNullException(nameof(wWsFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWWsFeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateWWsFeeDTO>();

            var result = await _wWsFeeService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<WWsFeeViewModel>());
        }


    }
}
