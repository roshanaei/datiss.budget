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

    [Route("[controller]")]
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

            var myreport = await _reportEngine.GenerateReportAsync(model);

             await myreport.ExportDocumentAsync(
                 Stimulsoft.Report.StiExportFormat.Pdf, @"E:\Projects\Datiss\datiss.budget\src\Datiss.Budget.Web\wwwroot\iman.pdf");

            return View();
        }

        [HttpPost("report1")]
        public async Task<IActionResult> Report1(int id) {

            return PartialView("_report1");
        }


        [Route("[action]")]
        public IActionResult ViewerEvent() {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        [HttpPost("GetReport/{id}")]
        public async Task<IActionResult> GetReport(int id = 1) {
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

            var myreport = await _reportEngine.GenerateReportAsync(model);

            return await StiNetCoreViewer.GetReportResultAsync(this, myreport);
        } 
    }
}
