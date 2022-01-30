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
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string ActivityTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int ActivityType { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string UserTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int UserTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public string UsageLayerDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int UsageLayerId { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int P1Fee { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int P2Fee { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int P1Note3 { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public int P2Note3 { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public int P1Note7 { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public int P2Note7 { get; set; }
    }
}
