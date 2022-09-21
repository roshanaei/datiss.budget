using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentProfitLossReportImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string SectionTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int SectionTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public long FunctionalBasicYear { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public long FunctionalYear_1 { get; set; }
    }
}
