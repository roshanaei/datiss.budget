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
    [Route("[controller]")]
    public class BranchFeeAmountController : Controller
    {
        private readonly IBranchFeeAmountService _branchFeeAmountService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;


        public BranchFeeAmountController(
            IBranchFeeAmountService branchFeeAmountService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService)
        {
            _branchFeeAmountService = branchFeeAmountService ?? throw new ArgumentNullException(nameof(branchFeeAmountService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId,int yearId)
        {
            var model = new CreateBranchFeeAmountViewModel
            {
                OrganizationId = organizationId,
                YearId = yearId
            };

            return View(model); 
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateBranchFeeAmountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = model.Adapt<CreateBranchFeeAmountDTO>();

            var result = await _branchFeeAmountService.CreateAsync(data);

            if(!result.IsValid)
            {
                model.AddError(result.Message);
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

            var data = model.Adapt<UpdateBranchFeeAmountDTO>();

            var result = await _branchFeeAmountService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return View(model);
            }
            return RedirectToAction("Index", new { page = model._CurrentPage });
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filterInput = new BranchFeeAmountFilterDTO
            {
                OrderBy = "organization",
                PageNumber = page
            };

            var result = await _branchFeeAmountService.GetListAsync(filterInput);
            var model = result.Adapt<BranchFeeAmountIndexViewModel>();

            model.SetFinanceYearFilterSource(
                (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>()
            );
            model.SetOrganizationFilterSource(
                (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>());
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BranchFeeAmountFilterViewModel model)
        {
            if (Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = model.Adapt<BranchFeeAmountFilterDTO>();

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
