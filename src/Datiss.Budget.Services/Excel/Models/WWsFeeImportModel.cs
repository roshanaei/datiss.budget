using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class WWsFeeImportModel
    {
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }

        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(2, MappingDirections.Both, Letter = "C")]
        public int UserTypeId { get; set; }
        [Column(3, MappingDirections.Both, Letter = "D")]
        public bool ActivityType { get; set; }
        [Column(4, MappingDirections.Both, Letter = "E")]
        public int UsageLayerId { get; set; }
        [Column(5, MappingDirections.Both, Letter = "F")]
        public int P1Fee { get; set; }
        [Column(6, MappingDirections.Both, Letter = "G")]
        public int P2Fee { get; set; }
        [Column(7, MappingDirections.Both, Letter = "H")]
        public int P1Note3 { get; set; }
        [Column(8, MappingDirections.Both, Letter = "I")]
        public int P2Note3 { get; set; }
        [Column(9, MappingDirections.Both, Letter = "J")]
        public int P1Note7 { get; set; }
        [Column(10, MappingDirections.Both, Letter = "K")]
        public int P2Note7 { get; set; }
    }
}
