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
    [Route("[controller]/[action]")]
    public class FinanceYearController : Controller
    {
        private readonly IFinanceYearService _financeYearService;

        public FinanceYearController(IFinanceYearService financeYearService){

            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("create")]
        public  async Task<IActionResult> Create()
        {

            var model = new AddFinanceYearViewModel();
            return View(model);
        }
     
    }
}
