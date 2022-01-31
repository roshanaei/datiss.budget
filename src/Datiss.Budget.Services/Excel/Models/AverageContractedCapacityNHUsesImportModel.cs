using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class AverageContractedCapacityNHUsesImportModel
    {
        [Column(1, MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "C")]
        public string UserTypeDisplay { get; set; }

        [Column(1, MappingDirections.Both, Letter = "D")]
        public int UserTypeId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "E")]
        public decimal AverageCapacity { get; set; }

        [Column(1, MappingDirections.Both, Letter = "F")]
        public decimal AverageCapacityWs { get; set; }

        [Column(1, MappingDirections.Both, Letter = "G")]
        public decimal AverageCapacityIncome { get; set; }

        [Column(1, MappingDirections.Both, Letter = "H")]
        public decimal AverageCapacityWsIncome { get; set; }
    }
}
