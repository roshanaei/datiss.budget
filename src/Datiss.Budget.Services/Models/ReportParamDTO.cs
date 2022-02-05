using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{
    public class ReportParamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ReportParamType ParamType { get; set; }
        public int ReportId { get; set; }
        public string ConstantKey { get; set; }
        public string ReportName { get; set; }
        public string ReportTitle { get; set; }
    }

    public class CreateReportParamDTO
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public ReportParamType ParamType { get; set; }
    }

    public class EditReportParamDTO : CreateReportParamDTO 
    { 
        public int Id { get; set; }
    }

}
