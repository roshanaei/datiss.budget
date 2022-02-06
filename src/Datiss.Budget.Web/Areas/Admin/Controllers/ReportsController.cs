using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class ReportsController : Controller
    {

        public const string Name = "Reports";
        public const string ACTION_Index = nameof(Index);

        private readonly IReportService _reportService;

        public ReportsController(
            IReportService reportService) {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

        [HttpGet("{page?}")]
        public IActionResult Index(int page = 1) {
            
            return View();
        }
    }
}
