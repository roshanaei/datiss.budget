using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Common.GuardToolkit;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ConstantController : Controller
    {
        public const string Name = "Constant";
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Index = nameof(Index);
        //public const string ACTION_Edit = nameof(Edit);
        //public const string ACTION_Delete = nameof(Delete);

        private readonly ILogger<ConstantController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConstantService _constantService;
        private readonly ISecurityTrimmingService _securityTrimmingService;


        public ConstantController(
            ILogger<ConstantController> logger,
            IWebHostEnvironment environment,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }


        private void showMessage(string type, string message)
        {
            ViewData["type"] = type;
            ViewData["message"] = message;
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            var parentSource = (await _constantService.GetParentsAsync())
                    .Adapt<List<DropDownItemViewModel>>();
            int firstParentId = parentSource.FirstOrDefault().Id;


            var filterInput = new ConstantFilterDTO
            {
                OrderBy = "displayorder",
                PageNumber = page,
                ParentId = firstParentId
            };

            var result = await _constantService.GetListAsync(filterInput);
            var model = result.Adapt<ConstantIndexViewModel>();

            model.SetParentSource(parentSource);

            model.SetParentFilterSource(parentSource, firstParentId);

            model.Filter.ParentId = filterInput.ParentId;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConstantIndexViewModel model, int page = 1)
        {
            var filterInput = model.Filter.Adapt<ConstantFilterDTO>();

            var result = await _constantService.GetListAsync(filterInput);
            model = result.Adapt<ConstantIndexViewModel>();
            model.Filter = filterInput.Adapt<ConstatntFilterViewModel>();

            var parentSource = (await _constantService.GetParentsAsync())
                    .Adapt<IEnumerable<DropDownItemViewModel>>();

            model.SetParentSource(parentSource);
            model.SetParentFilterSource(parentSource);

            return View(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create() 
        {
            var parentList = await _constantService.GetParentsAsync();
            var model = new CreateConstantViewModel {
                ParentList = parentList.Select(x=> new SelectListItem { 
                    Value = x.Id.ToString(),
                    Text = x.Title
                })
            };

            return PartialView("_CreateModal",model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateConstantViewModel model)
        {
            model.CheckArgumentIsNull(nameof(model));
            var data = model.Adapt<CreateConstantDTO>();
            //try
            //{
            //    await _constantService.CreateAsync(data);
            //}
            //catch (CopySameYearException ex)
            //{
            //    model.AddError(ViewMessages.CopyDestYearHasData);
            //    return Json(model);
            //}

            return RedirectToAction("index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _constantService.GetByIdAsync(id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }
            var model = entity.Adapt<UpdateConstantViewModel>();
            //model.set(
            //    (await _constantService.ge())
            //    .Adapt<IEnumerable<DropDownItemViewModel>>());
            return PartialView("_editModal", model);
        }

    }
}
