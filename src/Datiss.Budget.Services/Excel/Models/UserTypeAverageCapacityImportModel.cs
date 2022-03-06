using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class UserTypeAverageCapacityImportModel
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
        public decimal AverageCapacityW { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public decimal AverageCapacityWs { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public decimal AverageCapacityWIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public decimal AverageCapacityWsIncome { get; set; }
    }
}
