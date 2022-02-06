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
using Datiss.Budget.Common.WebToolkit;
using Datiss.Budget.Resources;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class ReportsController : Controller
    {

        public const string Name = "Reports";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Create = nameof(Create);
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

        [HttpPost("create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                return View(model);
            }

            var fileData = model.FileData.GetFormFileBytes();
            //if(fileData == null) {
            //    model.AddError("فایل را انتخاب کنید.");
            //    return View(model);
            //}

            //if(!model.FileData.HasFileExtension("mrt")) {
            //    model.AddError("نوع فایل انتخابی برای این گزارش مناسب نیست.");
            //    return View(model);
            //}
            model.ReadParams();

            var data = model.Adapt<CreateReportData>();
            data.FileData = fileData;
            try {
                var result = await _reportService.CreateAsync(data);
                if(result.NotValid) {
                    model.AddError(result.Message);
                }
                else {
                    return RedirectToAction(ACTION_Index);
                }
            }
            catch {
                model.AddError(ViewMessages.SystemError);
            }

            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var data = await _reportService.GetAsync(id);
                var model = data.Adapt<UpdateReportViewModel>();
                if(model.Params.Any())
                    model.LoadParams();

                return View(model);
            }
            catch {
                return NotFound();
            }
        }

    }
}
