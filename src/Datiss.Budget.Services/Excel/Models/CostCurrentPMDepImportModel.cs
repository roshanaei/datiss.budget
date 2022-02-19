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
        public string ActivityTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int ActivityType { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string CostCenterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "f")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public string CCPMDepTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int CCPMDepTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long CostCenter { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long FinancePMCost { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public decimal RFinancePMCost_D { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public long FinanceDepCost { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public decimal RFinanceDepCost_D { get; set; }
    }
}
