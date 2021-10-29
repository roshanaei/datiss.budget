using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FinanceYearController : Controller
    {
        private readonly IFinanceYearService _financeYearService;

        public FinanceYearController(IFinanceYearService financeYearService){

            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));

        }

        [HttpGet("{page?}")]
        public  async Task<IActionResult>Index(int page = 1)
        {
            var model = new FinanceYearIndexViewModel();
            return View(model);
        }


        [HttpPost("{page?}")]
        public async Task<IActionResult> Index(FinanceYearIndexViewModel viewModel,int page = 1)
        {
            if (Request.Form["btnCreate"].Count() > 0)
            {
                return RedirectToAction("Create");
            }

            return RedirectToAction("Index");
        }

        [HttpGet("create")]
        public  async Task<IActionResult> Create()
        {

            var model = new AddFinanceYearViewModel();
            return View(model);
        }
     
    }
}
