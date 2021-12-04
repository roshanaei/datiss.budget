using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Models;
using Mapster;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Resources;
using Datiss.Budget.Common.Exceptions;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class FinanceYearController : Controller
    {
        public const string Name = "FinanceYear";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_Delete = nameof(Delete);

        private readonly IFinanceYearService _financeYearService;

        public FinanceYearController(IFinanceYearService financeYearService){

            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));

        }
        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var filterInput = new FinanceYearFilterDTO
            {
                OrderBy = "id",
                OrderDesc = true,
                PageNumber = page
            };
            var result = await _financeYearService.GetListAsync(filterInput);
            var model = result.Adapt<FinanceYearIndexViewModel>();
            return View(model);
        }
        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FinanceYearIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<FinanceYearFilterDTO>();
            var result = await _financeYearService.GetListAsync(filterInput);
            model = result.Adapt<FinanceYearIndexViewModel>();
            model.Filter = filterInput.Adapt<FinanceYearFilterViewModel>();
            return View(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateFinanceYearViewModel();
            return PartialView("_createModal", model);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateFinanceYearViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var data = model.Adapt<CreateFinanceYearDTO>();
            try
            {
                await _financeYearService.CreateAsync(data);
            }
            catch (CopySameYearException ex)
            {
                model.AddError(ViewMessages.CopyDestYearHasData);
                return Json(model);
            }

            return RedirectToAction("index");
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _financeYearService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }
            var model = entity.Adapt<UpdateFinanceYearViewModel>();
            model.SetOrganizationStatusFilterSource(
                (await _financeYearService.GetDropDownStatusAsync())
                .Adapt<IEnumerable<DropDownItemViewModel>>());
            return PartialView("_editModal", model);
        }
        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateFinanceYearViewModel model)
        {

            model.CheckArgumentIsNull(nameof(model));
            if (!ModelState.IsValid)
            {
                model.AddError("خطاهای داده ای را بررسی نمایید.");
                return Json(model);
            }
            var data = model.Adapt<UpdateFinanceYearDTO>();
            try
            {
                await _financeYearService.UpdateAsync(data);
            }
            catch (CopySameYearException ex)
            {
                model.AddError(ViewMessages.CopySameYear);
                return Json(model);
            }
            catch (CopyDestYearHasDataException ex)
            {
                model.AddError(ViewMessages.CopyDestYearHasData);
                return Json(model);
            }

            return RedirectToAction("index");
        }
        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _financeYearService.SoftDeleteAsync(id);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    hasError = true,
                    message = "خطایی به وجود آمده است ."
                });
            }

            return Json(new
            {
                hasError = false,
                message = "حذف رکورد با موفقیت انجام شد."
            });
        }
    }
}
