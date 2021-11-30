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

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ConstantController : Controller
    {

        private readonly IConstantService _constantService;

        public ConstantController(IConstantService constantService) {
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));

        }

        [HttpGet("{page}")]
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

            model.Filter.ParentId = filterInput.ParentId;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> New() 
        {
            var parentList = await _constantService.GetParentsAsync();
            var model = new CreateConstantViewModel {
                ParentList = parentList.Select(x=> new SelectListItem { 
                    Value = x.Id.ToString(),
                    Text = x.Title
                })
            };

            return View(model);
        }

    }
}
