using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Web.Helpers;
using Datiss.Budget.Resources;
using Datiss.Budget.Web.ViewModels;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ConstantController : Controller
    {
        public const string Name = "WaterInstallFee";
        public const string ACTION_Create = nameof(New);
        public const string ACTION_Index = nameof(Index);
        //public const string ACTION_Edit = nameof(Edit);
        //public const string ACTION_Delete = nameof(Delete);

        private readonly IWebHostEnvironment _env;
        private readonly IConstantService _constantService;

        public ConstantController(
            IWebHostEnvironment environment,
            IConstantService constantService)
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));

        }

        private void showMessage(string type, string message) {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            
            var model = new ConstantIndexViewModel();
            var parentId = await _constantService.GetParentsAsync();
            int minParent = parentId.Min(x => x.Id);

            model.SetConstantParentSource(parentId, minParent);

            var filterInput = new ConstantFilter
            {
                OrderBy = "displayOrder",
                PageNumber = page,
                ParentId = minParent
            };

            var result = await _constantService.GetListAsync(filterInput);

            model.Model = result;
            model.Filter.ParentId = filterInput.ParentId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConstantIndexViewModel viewModel,int page = 1)
        {
            if (Request.Form["btnFilter"].Count() > 0)
            {
                var filterInput = viewModel.Filter.Adapt<ConstantFilter>();

                var result = await _constantService.GetListAsync(filterInput);

                viewModel.SetConstantParentSource(await _constantService.GetParentsAsync());

                viewModel.Model = result;

                return View(viewModel);
            }

            if (Request.Form["btnCreate"].Count() > 0)
            {
                int parentId = int.Parse(Request.Form["Filter.ParentId"].ToString());

                return RedirectToAction("Create", new
                {
                    parentId = parentId
                });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> New() 
        {
            var parentList = await _constantService.GetParentsAsync();
            var model = new AddConstantViewModel {
                ParentList = parentList.Select(x=> new SelectListItem { 
                    Value = x.Id.ToString(),
                    Text = x.Title
                })
            };

            return View(model);
        }

    }
}
