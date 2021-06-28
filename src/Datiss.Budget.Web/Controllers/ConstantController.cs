using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Controllers
{
    public class ConstantController : Controller
    {

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New() {

        }
    }
}
