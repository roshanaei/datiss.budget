using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class ConsumeForcastImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int UserTypeId { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public int UsageLayerId { get; set; }

        [Column(4, MappingDirections.Both, Letter = "E")]
        public decimal CountUser { get; set; }

        [Column(5, MappingDirections.Both, Letter = "F")]
        public decimal UnitUser { get; set; }

        [Column(6, MappingDirections.Both, Letter = "G")]
        public decimal ConsumeUser { get; set; }

        [Column(7, MappingDirections.Both, Letter = "H")]
        public decimal AvgConsumeUser { get; set; }
    }
}
