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

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[Controller]")]
    public class ConsumeForcastController : Controller
    {
        public const string Name = "ConsumeForcast";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        //public const string ACTION_Edit = nameof(Edit);
        //public const string ACTION_Copy = nameof(Copy);
        //public const string ACTION_Delete = nameof(Delete);
        //public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        //public const string ACTION_ImportExcel = nameof(ImportExcel);
        //public const string ACTION_Calculation = nameof(Calculation);
        //public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        //public const string ACTION_ExportExcel = nameof(ExportExcel);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<ConsumeForcastController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConsumeForcastService _consumeForcastService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;


        public ConsumeForcastController(
            ILogger<ConsumeForcastController> logger,
            IWebHostEnvironment environment,
            IConsumeForcastService consumeForcastService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _consumeForcastService = consumeForcastService ?? throw new ArgumentNullException(nameof(consumeForcastService));
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
        public async Task<IActionResult> Create(CreateConsumeForcastViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<CreateConsumeForcastDTO>();

            var result = await _consumeForcastService.CreateAsync(data);

            if(!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<ConsumeForcastViewModel>());
        }


        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {

        }


    }
}
