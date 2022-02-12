using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeCurrentOperationalImportModel
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
        public string ICOTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int ICOTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int CountH { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int PriceH { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int CostH { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int CountNH { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int PriceNH { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public int CostNH { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public int TotalCount { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public int TotalCost { get; set; }
    }
}
