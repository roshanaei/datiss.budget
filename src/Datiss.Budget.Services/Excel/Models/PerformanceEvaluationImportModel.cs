using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class PerformanceEvaluationImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string TableFieldDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int TableFieldId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public decimal Operation { get; set; }

    }
}
