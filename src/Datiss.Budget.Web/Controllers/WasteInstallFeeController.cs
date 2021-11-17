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
using Microsoft.AspNetCore.Hosting;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WasteInstallFeeController : Controller
    {
        public const string Name = "WasteInstallFee";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        //public const string ACTION_Copy = nameof(Copy);
        //public const string ACTION_Delete = nameof(Delete);
        //public const string ACTION_ImportExcel = nameof(ImportExcel);
        //public const string ACTION_Calculation = nameof(Calculation);
        //public const string ACTION_DownloadExcelTemplate = nameof(DownloadExcelTemplate);
        //public const string ACTION_ExportExcel = nameof(ExportExcel);

        private readonly IWebHostEnvironment _env;
        private readonly IWasteInstallFeeService _wasteInstallFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;

        public WasteInstallFeeController(
            IWebHostEnvironment environment,
            IWasteInstallFeeService wasteInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService
            )
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _wasteInstallFeeService = wasteInstallFeeService ?? throw new ArgumentNullException(nameof(wasteInstallFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId) {
            var model = new CreateWasteInstallFeeViewModel {
                OrganizationId = organizationId,
                YearId = yearId
            };

            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateWasteInstallFeeViewModel model)
        {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {

                return View(model);
            }

            var result = await _wasteInstallFeeService.CreateAsync(new CreateWasteInstallFeeDTO {
                DWasteTypeId = model.DWasteTypeId,
                OrganizationId = model.OrganizationId,
                WInstllFee = model.WInstllFee,
                YearId = model.YearId,
                DWasteTypeTitle = model.DWasteTypeTitle
            });

            if (!result.IsValid) {
                model.AddError(result.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            var entity = await _wasteInstallFeeService.GetByIdAsync(id);

            if (entity == null) {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWasteInstallFeeViewModel>();
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateWasteInstallFeeViewModel model) {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWasteTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {
                return View(model);
            }

            var result = await _wasteInstallFeeService.UpdateAsync(model.Adapt<UpdateWasteInstallFeeDTO>());

            if (!result.IsValid) {
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

            var filterInput = new WasteInstallFeeFilterDTO
            {
                OrderBy = "dwatertype",
                PageNumber = page,
                YearId = maxYear,
                OrganizationId = firstOrgId
            };

            var result = await _wasteInstallFeeService.GetListAsync(filterInput);
            var model = result.Adapt<WasteInstallFeeIndexViewModel>();

            model.SetFinanceYearFilterSource(yearSource, maxYear);
            model.SetOrganizationFilterSource(orgSource);

            model.Filter.YearId = filterInput.YearId;
            model.Filter.OrganizationId = filterInput.OrganizationId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WasteInstallFeeIndexViewModel viewModel, int page = 1)
        {
            if (Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = viewModel.Filter.Adapt<WasteInstallFeeFilterDTO>();

                var result = await _wasteInstallFeeService.GetListAsync(filterInput);

                viewModel.SetFinanceYearFilterSource(
                    (await _financeYearService.GetDropDownDataAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>()
                );

                viewModel.SetOrganizationFilterSource(
                    (await _organizationService.GetDropDownDataAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>()
                );
                viewModel = result.Adapt<WasteInstallFeeIndexViewModel>();

                return View(viewModel);
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
        //[HttpGet("report")]
        //public async Task<IActionResult> Report() {
        //    StiReport report = new StiReport();
        //    report.Load(_webHost.WebRootPath + "\\report.mrt");

        //    return await StiNetCoreViewer.GetReportResultAsync(this, report);
        //}

        [HttpGet("[action]")]
        public IActionResult ViewerEvent() {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

    }
}
