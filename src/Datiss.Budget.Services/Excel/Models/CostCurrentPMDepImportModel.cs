using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentPMDepImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string CostCenterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string CCPMDepTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int CCPMDepTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long FinancePMCost { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public decimal RFinancePMCost_D { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long FinanceDepCost { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public decimal RFinanceDepCost_D { get; set; }
    }
}
