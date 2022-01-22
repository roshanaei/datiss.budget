using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeForcastWsImportModel
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
        public int NumberUser { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int UnitUser { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int WasteInstallIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int WasteBranchIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int WasteNote3Income { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int WsNote11Income { get; set; }
    }
}
