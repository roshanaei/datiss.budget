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
    public class WasteSalesSplitController : Controller
    {
        private readonly IWasteSalesSplitService _wasteSalesSplitService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;


        public WasteSalesSplitController(
            IWasteSalesSplitService wasteSaleSplitService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {
            _wasteSalesSplitService = wasteSaleSplitService ?? throw new ArgumentNullException(nameof(_wasteSalesSplitService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId)
        {
            var model = new CreateWasteSalesSplitViewModel
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

            var wsPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wspipediametertype");
            model.WsPipeDiameterTypeSource = wsPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWasteSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wsPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wspipediametertype");
            model.WsPipeDiameterTypeSource = wsPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _wasteSalesSplitService.CreateAsync(new CreateWasteSalesSplitDTO { 
                OrganizationId = model.OrganizationId,
                YearId = model.YearId,
                UserTypeId = model.UserTypeId,
                WsPipeDiameterId = model.WsPipeDiameterId,
                NumberSales = model.NumberSales,
                UnitSales = model.UnitSales
            
            });


            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _wasteSalesSplitService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWasteSalesSplitViewModel>();
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wsPipeDiameterTypeSource = await _constantService.GetByConstantKeyAsync("wspipediametertype");
            model.WsPipeDiameterTypeSource = wsPipeDiameterTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });
            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWasteSalesSplitViewModel model)
        {
            var userTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.UserTypeSource = userTypeSource.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            var wsPipeDiameterTypeSourse = await _constantService.GetByConstantKeyAsync("wspipediametertype");
            model.WsPipeDiameterTypeSource = wsPipeDiameterTypeSourse.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = model.Adapt<UpdateWasteSalesSplitDTO>();
            var result = await _wasteSalesSplitService.UpdateAsync(data);

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
            var filterInput = new WasteSalesSplitFilterDTO
            {
                OrderBy = "usertype",
                PageNumber = page
            };

            var result = await _wasteSalesSplitService.GetListAsync(filterInput);
            var model = result.Adapt<WasteSalesSplitIndexViewModel>();

            model.SetFinanceYearFilterSource(
                (await _financeYearService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>()
            );
            model.SetOrganizationFilterSource(
                (await _organizationService.GetDropDownDataAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>()
            );

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WasteSalesSplitFilterViewModel model)
        {
            if (Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = model.Adapt<WasteSalesSplitFilterDTO>();

                var result = await _wasteSalesSplitService.GetListAsync(filterInput);

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
