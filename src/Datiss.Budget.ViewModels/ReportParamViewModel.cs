using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class ReportParamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ReportParamType ParamType { get; set; }
        public string ParamTypeDisplay => ParamType.ToDisplay();
        public int ReportId { get; set; }
        public string ConstantKey { get; set; }
        public string ReportName { get; set; }
        public string ReportTitle { get; set; }
    }

    public class CreateReportParamTypeViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public ReportParamType ParamType { get; set; }
    }

}
