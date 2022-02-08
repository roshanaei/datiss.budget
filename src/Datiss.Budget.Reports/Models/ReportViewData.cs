using Datiss.Budget.Enum;
using System.Collections.Generic;

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

        public byte[] TemplateFileData { get; set; }

        public string Description { get; set; }

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
