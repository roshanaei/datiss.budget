using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class IncomeCurrentCofficientImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string UserTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int UserTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string UsageLayerDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int UsageLayerId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public decimal Fee { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public decimal FeeWs { get; set; }

    }
}
