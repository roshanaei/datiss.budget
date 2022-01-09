using Datiss.Budget.Enum;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeForcastOtherImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string OIFTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int OIFTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public ActivityType ActivityId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int OIFCount { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int OIFPrice { get; set; }
    }
}
