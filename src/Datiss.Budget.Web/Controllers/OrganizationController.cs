using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Web.Controllers
{
    public class OrganizationController : Controller
    {

        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            var parentList = await _organizationService.GetParentsAsync();
            var model = new AddOrganizationViewModel
            {
                ParentList = parentList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                })
            };

            return View(model);
        }

    }
}
