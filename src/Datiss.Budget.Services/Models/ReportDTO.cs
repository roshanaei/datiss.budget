using System;
using System.Collections.Generic;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{
    public class ReportData : IEquatable<ReportData>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public int CategoryTypeId { get; set; }
        public string CategoryTypeDisplay { get; set; }
        public byte[] FileData { get; set; }
        public string FilePath { get; set; }
        public IList<ReportParamDTO> Params { get; set; }

        public bool Equals(ReportData other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ReportData);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CreateReportData
    {
        public CreateReportData() 
            => Params = new List<CreateReportParamDTO>();
        
        public void AddParam(CreateReportParamDTO p)
            => Params.Add(p);

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
        public int CategoryTypeId { get; set; }
        public byte[] FileData { get; set; }
        public IList<CreateReportParamDTO> Params { get; set; }
    }

    public class UpdateReportData : CreateReportData
    {
        public int Id { get; set; }
        public EntityStatus Status { get; set; }
    }

    public class ReportFilterDTO : FilterInputDTO
    {
        public string ReportTitle { get; set; }

        public int? CategoryId { get; set; }

        public EntityStatus? Status { get; set; }
        public int UserId { get; set; }
    }

    public class ReportRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

}
