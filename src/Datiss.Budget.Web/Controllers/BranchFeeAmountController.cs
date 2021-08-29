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
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class BranchFeeAmountController : Controller
    {
        private readonly IBranchFeeAmountService _branchFeeAmountService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public BranchFeeAmountController(
            IBranchFeeAmountService branchFeeAmountService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _branchFeeAmountService = branchFeeAmountService ?? throw new ArgumentNullException(nameof(branchFeeAmountService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId,int yearId)
        {
            var model = new AddBranchFeeAmountViewModel
            {
                OrganizationId = organizationId,
                YearId = yearId
            };

            return View(model); 
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(AddBranchFeeAmountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _branchFeeAmountService.AddAsync(new CreateBranchFeeAmountDTO
            {
                YearId = model.YearId,
                OrganizationId = model.OrganizationId,
                UrbanAdjustmentFactor = model.UrbanAdjustmentFactor,
                WasteRateInWater = model.WasteRateInWater,
                WaterBranchingPerHousing = model.WaterBranchingPerHousing,
                TubingCost = model.TubingCost,
                WaterPartnershipAmountDomestic = model.WaterPartnershipAmountDomestic,
                WaterPartnershipAmountNDomestic = model.WaterPartnershipAmountNDomestic,
                WastePartnershipAmountDomestic = model.WastePartnershipAmountDomestic,
                WastePartnershipAmountNDomestic = model.WastePartnershipAmountNDomestic,
                FixCostNote11H = model.FixCostNote11H,
                FixCostNote11NH = model.FixCostNote11NH,
                FixCostNote11HWs = model.FixCostNote11HWs,
                FixCostNote11NHWs = model.FixCostNote11NHWs,
                WsTubingCost = model.WsTubingCost
            });

            if(!result.IsValid)
            {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _branchFeeAmountService.GetByIdAsync(id);

            if(entity == null){
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateBranchFeeAmountViewModel>();
            
            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id,UpdateBranchFeeAmountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _branchFeeAmountService.UpdateAsync(model);

            if (!result.IsValid)
            {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }
            return RedirectToAction("Index", new { page = model._CurrentPage });
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var access = _securityTrimmingService.CanCurrentUserAccess("", "BranchFeeAmountController", "Index");

            var filterInput = new BranchFeeAmountFilter
            {
                OrderBy = "organization",
                PageNumber = page
            };

            var result = await _branchFeeAmountService.GetListAsync(filterInput);

            var model = new BranchFeeAmountIndexViewModel();
            model.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
            model.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync(null));
            model.Model = result;

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BranchFeeAmountFilterViewModel model)
        {
            if (Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = model.Adapt<BranchFeeAmountFilter>();

                var result = await _branchFeeAmountService.GetListAsync(filterInput);
                return View(result);
            }

            if (Request.Form["btnCreate"].Count() > 0)
            {
                int yearId = int.Parse(Request.Form["Filter.YearId"].ToString());
                int orgId = int.Parse(Request.Form["Filter.OrganizationId"].ToString());

                return RedirectToAction("Create", new
                {
                    organizationId = orgId,
                    yearId = yearId
                });
            }

            return RedirectToAction("Index");


        }

       
    }
}
