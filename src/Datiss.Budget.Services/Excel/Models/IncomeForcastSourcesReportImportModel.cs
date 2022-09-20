using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeForcastSourcesReportImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }
        [Column(MappingDirections.Both, Letter = "C")]
        public string SourceDescriptionTitle { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int SourceDescriptionId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long FunctionalBasicYear { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long FunctionalLastYear { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long ApproveYear_1 { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long PercentBudget { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long ForcastY { get; set; }
    }
}
