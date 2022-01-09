using Datiss.Budget.Enum;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class NHCoImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public ActivityType ActivityType { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int P1Capacity { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int FixCostCo { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int P1CostCo { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int P2CostCo { get; set; }
    }
}
