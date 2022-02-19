using Datiss.Budget.Enum;
using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class CostCurrentInstallationImportModel
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
        public string CCInstalationTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int CCInstalationTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int NumberUser { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int Cost { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long Income { get; set; }

    }
}
