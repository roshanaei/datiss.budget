using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeForcastImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int DWaterTypeId { get; set; }

        [Column(3, MappingDirections.Both, Letter = "D")]
        public int NumberUser { get; set; }
        [Column(4, MappingDirections.Both, Letter = "E")]
        public int UnitUser { get; set; }
        [Column(5, MappingDirections.Both, Letter = "F")]
        public int WaterBranchIncome { get; set; }
        [Column(6, MappingDirections.Both, Letter = "G")]
        public int WaterInstllIncome { get; set; }
        [Column(7, MappingDirections.Both, Letter = "H")]
        public int WaterNote2Income { get; set; }
        [Column(8, MappingDirections.Both, Letter = "I")]
        public int WaterNote3Income { get; set; }
        [Column(9, MappingDirections.Both, Letter = "J")]
        public int WNote11Income { get; set; }
    }
}
