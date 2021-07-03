using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Contracts;


namespace Datiss.Budget.Web.Controllers
{
    public class FinanceYearController : Controller
    {
        private readonly IFinanceYearService _financeYearService;

        public FinanceYearController(IFinanceYearService financeYearService){

            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public  async Task<IActionResult> New()
        {

            var model = new AddFinanceYearViewModel();
            return View(model);
        }
     
    }
}
