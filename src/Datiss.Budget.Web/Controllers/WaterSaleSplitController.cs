using Datiss.Budget.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WaterSaleSplitController : Controller
    {
        private readonly IWaterSaleSplitService _waterSaleSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;


        public WaterSaleSplitController (
            IWaterSaleSplitService waterSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {
            _waterSaleSplitService = waterSaleSplitService ?? throw new ArgumentNullException(nameof(waterSaleSplitService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> Create()
        //{
          
        //}
        public IActionResult Index()
        {
            return View();
        }
    }
}
