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
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int UserTypeId { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public decimal AverageCapacityW { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public decimal AverageCapacityWIncome { get; set; }

        [Column(5, MappingDirections.Both, Letter = "F")]
        public decimal AverageCapacityWs { get; set; }

        [Column(6, MappingDirections.Both, Letter = "G")]
        public decimal AverageCapacityWsIncome { get; set; }
    }
}
