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
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Resources;
using Datiss.Budget.Enum;
using DNTPersianUtils.Core;

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

        public FinanceYearController(IFinanceYearService financeYearService)
        {

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
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }
            var data = model.Adapt<CreateFinanceYearDTO>();

            var result = await _financeYearService.CreateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result);
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _financeYearService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }
            var model = new UpdateFinanceYearViewModel
            {
                Year = entity.Year,
                Title = entity.Title,
                StartPersianDate = entity.StartDate.ToShortPersianDateString(),
                EndPrsianDate = entity.EndDate.ToShortPersianDateString(),
                Enable = entity.Status == EntityStatus.Enabled ? true : false
            };

            return PartialView("_editModal", model);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(UpdateFinanceYearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddError(ViewMessages.InvalidData);
                return Json(model);
            }

            var data = model.Adapt<UpdateFinanceYearDTO>();
            var result = await _financeYearService.UpdateAsync(data);

            if (!result.IsValid)
            {
                model.AddError(result.Message);
                return Json(model);
            }

            return Json(result);
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
                    message = "حتما باید سال مالی داشته باشید ."
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
