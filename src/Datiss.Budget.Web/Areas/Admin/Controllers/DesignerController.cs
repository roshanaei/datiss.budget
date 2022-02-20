using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Identity;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class DesignerController : Controller
    {

        private readonly IWebHostEnvironment _host;

        public DesignerController(IWebHostEnvironment host) {
            _host = host ?? throw new ArgumentNullException(nameof(host));

            var stimulLicenseKey = Path.Combine(_host.WebRootPath, "reporting\\license.key");
            StiLicense.LoadFromFile(stimulLicenseKey);
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [Route("[action]")]
        public async Task<IActionResult> GetReport() {
            StiReport report = new StiReport();

            return await StiNetCoreDesigner.GetReportResultAsync(this, report);
        }

        [Route("[action]")]
        public IActionResult DesignerEvent() {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }

    }

}
