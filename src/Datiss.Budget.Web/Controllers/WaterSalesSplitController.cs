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

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId)
        {
            var model = new AddWaterSalesSplitViewModel
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
            model.WPipeDiameterTypeSourse = wPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(AddWaterSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wpipediametertype");
            model.WPipeDiameterTypeSourse = wPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _waterSalesSplitService.AddAsync(new CreateWaterSalesSplitDTO { 
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
            model.WPipeDiameterTypeSourse = wPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
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
            model.WPipeDiameterTypeSourse = wPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _waterSalesSplitService.UpdateAsync(model);

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
            var filterInput = new WaterSalesSplitFilter
            {
                OrderBy = "usertype",
                PageNumber = page
            };

            var result = await _waterSalesSplitService.GetListAsync(filterInput);

            var model = new WaterSalesSplitIndexViewModel();
            model.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
            model.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync());
            model.Model = result;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterSalesSplitFilterViewModel model)
        {
            if (Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = model.Adapt<WaterSalesSplitFilter>();

                var result = await _waterSalesSplitService.GetListAsync(filterInput);

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
