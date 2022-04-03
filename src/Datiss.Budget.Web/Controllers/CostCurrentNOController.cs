using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using ClosedXML.Extensions;
using Datiss.Budget.Reports.Excel;
using Microsoft.Extensions.Logging;
using Datiss.Budget.Common;
using Datiss.Budget.Enum;
using Datiss.Budget.Security;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CostCurrentNOController : Controller
    {
        public const string Name = "CostCurrentNO";
        public const string ACTION_Create = nameof(Create);


        private readonly ILogger<CostCurrentNOController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostCurrentNOService _costCurrentNOService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostCurrentNOController(
           ILogger<CostCurrentNOController> logger,
           IWebHostEnvironment environment,
           ICostCurrentNOService costCurrentNOService,
           IOrganizationService organizationService,
           IFinanceYearService financeYearService,
           IConstantService constantService,
           ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costCurrentNOService = costCurrentNOService ?? throw new ArgumentNullException(nameof(costCurrentNOService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        //[HttpPost("[action]")]
        //[HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        //public async Task<IActionResult> Create(CreateCostCurrentBankFeeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.AddError(ViewMessages.InvalidData);
        //        return Json(model);
        //    }
        //    var data = model.Adapt<CreateCostCurrentBankFeeDTO>();

        //    var result = await _costCurrentBankFeeService.CreateAsync(data);

        //    if (!result.IsValid)
        //    {
        //        model.AddError(result.Message);
        //        return Json(model);
        //    }

        //    return Json(result.Result.Adapt<CostCurrentBankFeeViewModel>());
        //}

    }
}
