using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class SalesSplitTotalImportModel
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
        public int WNumber { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int WUnit { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int WsNumber { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int WsUnit { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public decimal WNumber_2 { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public decimal WUnit_2 { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public decimal WsNumber_2 { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public decimal WsUnit_2 { get; set; }
    }
}
