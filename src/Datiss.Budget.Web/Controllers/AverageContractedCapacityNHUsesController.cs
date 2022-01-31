using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
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
    [Authorize]
    [Route("[controller]")]
    public class AverageContractedCapacityNHUsesController : Controller
    {
        public const string Name = "WaterInstallFee";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_Calculation = nameof(Calculation);
        public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<AverageContractedCapacityNHUsesController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IAverageContractedCapacityNHUsesService _averageContractedCapacityNHUsesService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public AverageContractedCapacityNHUsesController(
            ILogger<AverageContractedCapacityNHUsesController> logger,
            IWebHostEnvironment environment,
            IAverageContractedCapacityNHUsesService averageContractedCapacityNHUsesService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _averageContractedCapacityNHUsesService = averageContractedCapacityNHUsesService ?? throw new ArgumentNullException(nameof(averageContractedCapacityNHUsesService));
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
        [HasPermission(claimType: Name, actionType: PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateAverageContractedCapacityNHUsesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateAverageContractedCapacityNHUsesDTO>();

            var result = await _averageContractedCapacityNHUsesService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<AverageContractedCapacityNHUsesViewModel>());
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, actionType: PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateAverageContractedCapacityNHUsesViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateAverageContractedCapacityNHUsesDTO>();
            var result = await _averageContractedCapacityNHUsesService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<AverageContractedCapacityNHUsesViewModel>()
            );
        }


    }
}
