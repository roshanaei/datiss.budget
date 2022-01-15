using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class ConsumeForcastWsImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int UserTypeId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "D")]
        public int UserTypeDisplay { get; set; }

        [Column(3, MappingDirections.Both, Letter = "E")]
        public int UsageLayerId { get; set; }

        [Column(3, MappingDirections.Both, Letter = "F")]
        public int UsageLayerDisplay { get; set; }

        [Column(4, MappingDirections.Both, Letter = "G")]
        public decimal CountUser { get; set; }

        [Column(5, MappingDirections.Both, Letter = "H")]
        public decimal UnitUser { get; set; }

        [Column(6, MappingDirections.Both, Letter = "I")]
        public decimal ConsumeUser { get; set; }

        [Column(7, MappingDirections.Both, Letter = "J")]
        public decimal AvgConsumeUser { get; set; }
    }
}
