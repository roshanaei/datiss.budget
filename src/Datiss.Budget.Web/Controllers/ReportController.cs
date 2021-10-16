using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datiss.Budget.Reports.Contracts;
using Datiss.Budget.Reports;
using Stimulsoft.Report.Mvc;

namespace Datiss.Budget.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class ReportController : Controller
    {

        private readonly IReportEngine _reportEngine;


        public ReportController(
            IReportEngine reportEngine) {
            _reportEngine = reportEngine 
                ?? throw new ArgumentNullException(nameof(reportEngine));
        }


        public IActionResult Index() {
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Report(int id) {
            var model = new ReportViewData {
                Id = id,
                Params = new List<ReportParamViewData> {
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Year,
                        Name = "YearId",
                        Value = 1
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Organization,
                        Name = "OrganizationId",
                        Value = 5
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Constant,
                        Name = "UserTypeId",
                        Value = 3
                    }
                }
            };

            //var myreport = await _reportEngine.GenerateReportAsync(model);

            //myreport.Render(true);

            //await myreport.ExportDocumentAsync(
            //     Stimulsoft.Report.StiExportFormat.Pdf, @"E:\Projects\Datiss\datiss.budget\src\Datiss.Budget.Web\wwwroot\iman.pdf");

            return View(model);
        }

        [HttpPost("report1")]
        public async Task<IActionResult> Report1(int id) {

            return PartialView("_report1");
        }

        public IActionResult GetReport() {
            var model = new ReportViewData {
                Id = 1,
                Params = new List<ReportParamViewData> {
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Year,
                        Name = "YearId",
                        Value = 1
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Organization,
                        Name = "OrganizationId",
                        Value = 5
                    },
                    new ReportParamViewData {
                        ParamType = Enum.ReportParamType.Constant,
                        Name = "UserTypeId",
                        Value = 3
                    }
                }
            };

            //var myreport = await _reportEngine.GenerateReportAsync(model);

            var myreport = _reportEngine.GenerateReportAsync(model).Result;

            myreport.Render(true);

            //return await StiNetCoreViewer.GetReportResultAsync(this, myreport);

            return StiNetCoreViewer.GetReportResult(this, myreport);
        }


        public IActionResult ViewerEvent() {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        
    }
}
