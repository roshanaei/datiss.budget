using Datiss.Budget.Resources;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Controllers
{
    public class CostCurrentElectricityController : Controller
    {
        public const string Name = "CostCurrentElectricity";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Copy = nameof(Copy);
        public const string ACTION_Delete = nameof(Delete);
        public const string ACTION_DeleteRecords = nameof(DeleteRecords);
        public const string ACTION_ImportExcel = nameof(ImportExcel);
        public const string ACTION_ExportExcel = nameof(ExportExcel);
        public const string ACTION_GetExcelTemplate = nameof(GetExcelTemplate);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly ILogger<CostCurrentElectricityController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICostCurrentElectricityService _costCurrentElectricityService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public CostCurrentElectricityController(
            ILogger<CostCurrentElectricityController> logger,
            IWebHostEnvironment environment,
            ICostCurrentElectricityService costCurrentElectricityService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _costCurrentElectricityService = costCurrentElectricityService ?? throw new ArgumentNullException(nameof(costCurrentElectricityService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Create)]
        public async Task<IActionResult> Create(CreateCostCurrentElectricityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateCostCurrentElectricityDTO>();

            var result = await _costCurrentElectricityService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result.Result.Adapt<CreateCostCurrentElectricityViewModel>());
        }

        [HttpPost("[action]")]
        [HasPermission(claimType: Name, PermissionActionType.Edit)]
        public async Task<IActionResult> Edit(UpdateNHCoViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateCostCurrentElectricityDTO>();
            var result = await _costCurrentElectricityService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(
                result.Result.Adapt<CostCurrentElectricityViewModel>()
            );
        }

        #region Private Helper Methods
        private string getCalcTitle(string key)
            => key switch
            {
                "CostCurrentElectricity_Cal1" => SPTitles.CostCurrentElectricity_Cal1,
                "CostCurrentElectricity_Cal2" => SPTitles.CostCurrentElectricity_Cal2,
                _ => ""
            };
        #endregion

    }
}
