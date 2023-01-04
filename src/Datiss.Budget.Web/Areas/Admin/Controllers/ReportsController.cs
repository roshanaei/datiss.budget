using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.WebToolkit;
using Datiss.Budget.Resources;
using Datiss.Budget.Enum;
using Mapster;
using Datiss.Budget.Common;
using System.Collections.Generic;

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
        public const string ACTION_Delete = nameof(Delete);

        private readonly string _indexFilterKey = $"{Name}_{ACTION_Index}_filter";
        private const string _reportExt = ".mrt";

        private readonly IReportService _reportService;
        private readonly IConstantService _constantService;
        public ReportsController(
            IReportService reportService,
            IConstantService constantService) {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            var filter = new ReportFilterDTO();
            filter.PageSize = 100;
            var myfilter = TempData.Get<AdminReportFilterViewModel>(_indexFilterKey);
            if(myfilter != null) {
                filter = myfilter.Adapt<ReportFilterDTO>();
                TempData.Put(_indexFilterKey, filter);
            }

            var categorySource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ReportCategory))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            filter.PageNumber = page;
            var result = await _reportService.GetAdminListAsync(filter);
            var model = result.Adapt<AdminReportIndexViewModel>();
            model.Filter = filter.Adapt<AdminReportFilterViewModel>();

            model.SetCategoriesFilterSource(categorySource, filter.CategoryId);

            return View(model);
        }

        [HttpPost("{page?}")]
        public async Task<IActionResult> Index(AdminReportIndexViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var filter = model.Filter.Adapt<ReportFilterDTO>();
            filter.PageSize = 100;

            TempData.Put(_indexFilterKey, filter);

            var result = await _reportService.GetAdminListAsync(filter);
            model = result.Adapt<AdminReportIndexViewModel>();
            model.Filter = filter.Adapt<AdminReportFilterViewModel>();

            var categorySource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ReportCategory))
                .Adapt<IEnumerable<DropDownItemViewModel>>();
            model.SetCategoriesFilterSource(categorySource, filter.CategoryId);

            return View(model);
        }
        
        [HttpGet("create")]
        public async Task<IActionResult> Create() {

            var model = new CreateReportViewModel();

            var categories = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ReportCategory))
                .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetCatergorySource(categories);

            return View(model);
        }

        [HttpPost("create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var categories = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ReportCategory))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            model.FixParams();
            if (!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                return View(model);
            }

            var fileData = model.ReportFile.GetFormFileBytes();
            if (fileData == null) {
                model.AddError(ViewMessages.Report_File_Req);
                return View(model);
            }

            if (!model.ReportFile.HasFileExtension(_reportExt)) {
                model.AddError(ViewMessages.Report_Ext);
                return View(model);
            }

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

            model.SetCatergorySource(categories);


            return View(model);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var data = await _reportService.GetAsync(id);
                var model = data.Adapt<UpdateReportViewModel>();

                var categories = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ReportCategory))
                    .Adapt<IEnumerable<DropDownItemViewModel>>();

                model.SetCatergorySource(categories,model.CategoryTypeId);


                if (data.Status == EntityStatus.Enabled)
                    model.Enabled = true;
                if(model.Params.Any())
                    model.LoadParams();

                return View(model);
            }
            catch {
                return NotFound();
            }
        }

        [HttpPost("edit/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateReportViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var categories = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__ReportCategory))
                .Adapt<IEnumerable<DropDownItemViewModel>>();


            model.SetCatergorySource(categories, model.CategoryTypeId);

            model.FixParams();
            if (!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                return View(model);
            }
            byte[] fileData = null;
            if(model.ReportFile.IsNotNullOrEmpty()) {
                fileData = model.ReportFile.GetFormFileBytes();
                if (!model.ReportFile.HasFileExtension(_reportExt)) {
                    model.AddError(ViewMessages.Report_Ext);
                    return View(model);
                }
            }

            model.ReadParams();

            var data = model.Adapt<UpdateReportData>();
            data.FileData = fileData;
            data.Status = model.Enabled 
                ? Enum.EntityStatus.Enabled 
                : Enum.EntityStatus.Disbaled;

            try {
                var result = await _reportService.UpdateAsync(data);
                if(result.NotValid) {
                    model.AddError(result.Message);
                }
                else {
                    return RedirectToAction(ACTION_Index);
                }
            }
            catch(Exception ex) {
                model.AddError(ViewMessages.SystemError);
            }

            return View(model);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                await _reportService.DeleteAsync(id);

                return Json(new {
                    success = true
                });
            }
            catch(NullReferenceException) {
                return NotFound();
            }
            catch(Exception ex) {
                return Json(new {
                    hasError = true,
                    message = ViewMessages.SystemError
                });
            }
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var report = await _reportService.GetAsync(id);

            if (report == null)
                return NotFound();

            return File(report.FileData, "application/octet-stream", $"{report.Name}.mrt");
        }

    }
}
