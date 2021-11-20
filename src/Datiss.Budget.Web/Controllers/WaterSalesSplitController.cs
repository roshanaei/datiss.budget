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

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WaterSalesSplitController : Controller
    {
        private readonly IWaterSalesSplitService _waterSalesSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;


        public WaterSalesSplitController(
            IWaterSalesSplitService waterSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {
            _waterSalesSplitService = waterSaleSplitService ?? throw new ArgumentNullException(nameof(_waterSalesSplitService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }
        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId)
        {
            var model = new CreateWaterSalesSplitViewModel
            {
                OrganizationId = organizationId,
                YearId = yearId
            };
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWaterSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _waterSalesSplitService.CreateAsync(new CreateWaterSalesSplitDTO { 
                OrganizationId = model.OrganizationId,
                YearId = model.YearId,
                UserTypeId = model.UserTypeId,
                WPipeDiameterId = model.WPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales
            
            });


            if (!result.IsValid)
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
            var entity = await _waterSalesSplitService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWaterSalesSplitViewModel>();
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWaterSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var data = model.Adapt<UpdateWaterSalesSplitDTO>();
            var result = await _waterSalesSplitService.UpdateAsync(data);

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
            var orgSource = (await _organizationService.GetDropDownDataAsync())
               .Adapt<List<DropDownItemViewModel>>();
            int firstOrgId = orgSource.FirstOrDefault().Id;

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            int maxYear = yearSource.Max(_ => _.Id);

            var dwaterSource = (await _constantService.GetByConstantKeyAsync("usertype"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var filterInput = new WaterSalesSplitFilterDTO
            {
                OrderBy = "usertype",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _waterSalesSplitService.GetListAsync(filterInput);
            var model = result.Adapt<WaterSalesSplitIndexViewModel>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);

            model.Filter.YearId = filterInput.YearId;
            model.Filter.OrganizationId = filterInput.OrganizationId;

            return View(model);
        }
        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterSalesSplitIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<WaterSalesSplitFilterDTO>();
            var result = await _waterSalesSplitService.GetListAsync(filterInput);
            model = result.Adapt<WaterSalesSplitIndexViewModel>();

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var yearSource = (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            var dwaterSource = (await _constantService.GetByConstantKeyAsync("usertype"))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetYearSource(yearSource);
            model.SetOrganizationSource(orgSource);
            model.SetFinanceYearFilterSource(yearSource);
            model.SetOrganizationFilterSource(orgSource);
            
            return View(model);

            if (Request.Form["btnFilter"].Count() > 0)
            {

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
