using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports
{
    public class ReportViewData
    {
        public ReportViewData() {
            Params = new List<ReportParamViewData>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

        public IEnumerable<DropDownItem> FinanceYearSource { get; set; }

        public int? SelectedYearId { get; set; }

        public IEnumerable<DropDownItem> OrganizationSource { get; set; }

        public int? SelectedOrganizationId { get; set; }

        public Dictionary<string, IEnumerable<DropDownItem>> ConstantSource { get; set; }

        public Dictionary<string, int?> SelectedConstants { get; set; }

        public IEnumerable<ReportParamViewData> Params { get; set; }
    }


    public class ReportParamViewData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public ReportParamType ParamType { get; set; }

        public int ReportId { get; set; }

        public string ConstantKey { get; set; }

        public object Value { get; set; }
    }

}
