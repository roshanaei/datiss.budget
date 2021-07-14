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
    public class WaterInstallFeeController : Controller
    {

        private readonly IWaterInstallFeeService _waterInstallFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;

        public WaterInstallFeeController(
            IWaterInstallFeeService waterInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService) 
        {
            _waterInstallFeeService = waterInstallFeeService ?? throw new ArgumentNullException(nameof(waterInstallFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId) {
            var model = new AddWaterInstallFeeViewModel {
                OrganizationId = organizationId,
                YearId = yearId
            };

            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(AddWaterInstallFeeViewModel model) 
        {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {

                return View(model);
            }

            var result = await _waterInstallFeeService.AddAsync(new CreateWaterInstallFeeDTO {
                DWaterTypeId = model.DWaterTypeId,
                OrganizationId = model.OrganizationId,
                WInstllFee = model.WInstllFee,
                YearId = model.YearId,
                DWaterTypeTitle = model.DWaterTypeTitle
            });

            if(! result.IsValid) {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            var entity = await _waterInstallFeeService.GetByIdAsync(id);

            if(entity == null) {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWaterInstallFeeViewModel>();
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWaterInstallFeeViewModel model) {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {
                return View(model);
            }

            var result = await _waterInstallFeeService.UpdateAsync(model);

            if(!result.IsValid) {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index", new { page = model._CurrentPage });
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) 
        {
            var filterInput = new WaterInstallFeeFilter {
                OrderBy = "dwatertype",
                PageNumber = page
            };

            var result = await _waterInstallFeeService.GetListAsync(filterInput);

            var model = new WaterInstallFeeIndexViewModel();
            model.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
            model.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync(null));
            model.Model = result;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterInstallFeeFilterViewModel model) 
        {
            if(Request.Form["btnFilter"].Count() > 0) {
                var filterInput = model.Adapt<WaterInstallFeeFilter>();

                var result = await _waterInstallFeeService.GetListAsync(filterInput);

                return View(result);
            }
            
            if(Request.Form["btnCreate"].Count() > 0) {
                int yearId = int.Parse(Request.Form["Filter.YearId"].ToString());
                int orgId = int.Parse(Request.Form["Filter.OrganizationId"].ToString());

                return RedirectToAction("Create", new {
                    organizationId = orgId,
                    yearId = yearId
                });
            }

            return RedirectToAction("Index");
        }
    }
}
