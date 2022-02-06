using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Extensions;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Web;
using Mapster;
using Datiss.Budget.Common.GuardToolkit;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class ReportsController : Controller
    {

        public const string Name = "Reports";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);

        private readonly string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";

        private readonly IReportService _reportService;

        public ReportsController(
            IReportService reportService) {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            var filter = new ReportFilterDTO();
            var myfilter = TempData.Get<AdminReportFilterViewModel>(_indexFilterKey);
            if(myfilter != null) {
                filter = myfilter.Adapt<ReportFilterDTO>();
                TempData.Put(_indexFilterKey, filter);
            }
            filter.PageNumber = page;
            var result = await _reportService.GetAdminListAsync(filter);
            var model = result.Adapt<AdminReportIndexViewModel>();
            model.Filter = filter.Adapt<AdminReportFilterViewModel>();

            return View(model);
        }

        [HttpPost("{page?}")]
        public async Task<IActionResult> Index(AdminReportIndexViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var filter = model.Filter.Adapt<ReportFilterDTO>();
            TempData.Put(_indexFilterKey, filter);

            var result = await _reportService.GetAdminListAsync(filter);
            model = result.Adapt<AdminReportIndexViewModel>();
            model.Filter = filter.Adapt<AdminReportFilterViewModel>();

            return View(model);
        }
        
        [HttpGet("create")]
        public async Task<IActionResult> Create() {
            var model = new CreateReportViewModel();
            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var data = await _reportService.GetAsync(id);
                var model = data.Adapt<UpdateReportViewModel>();

                return View(model);
            }
            catch {
                return NotFound();
            }
        }

    }
}
