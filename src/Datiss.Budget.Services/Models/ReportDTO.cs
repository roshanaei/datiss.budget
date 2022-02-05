using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public byte[] FileData { get; set; }
        public string FilePath { get; set; }
    }

    public class CreateReportData
    {
        public CreateReportData() 
            => Parameters = new List<CreateReportParamDTO>();
        
        public void AddParam(CreateReportParamDTO p)
            => Parameters.Add(p);

        public void AddParam(string name, string title, ReportParamType paramType) 
            => AddParam(new CreateReportParamDTO
                {
                    Name = name,
                    Title = title,
                    ParamType = paramType
                });
        
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] FileData { get; set; }
        public IList<CreateReportParamDTO> Parameters { get; set; }
    }

    public class UpdateReportData : CreateReportData
    {
        public int Id { get; set; }
        public EntityStatus Status { get; set; }
    }

    public class ReportFilterDTO : FilterInputDTO
    {
    }

}
