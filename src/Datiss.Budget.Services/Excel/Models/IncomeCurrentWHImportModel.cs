using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeCurrentWHImportModel
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
        public string UsageLayerDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int UsageLayerId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int NumberUser { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int UnitUser { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int Cost { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public decimal AvgConsumeUser { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int ConsumptionUser { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public int Income { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public int SubscriptionIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public int SeasonalIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public int TIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "P")]
        public int Note3Price { get; set; }

        [Column(MappingDirections.Both, Letter = "Q")]
        public int Note3Income { get; set; }

        [Column(MappingDirections.Both, Letter = "R")]
        public int Diff_ConsWsVolume { get; set; }

        [Column(MappingDirections.Both, Letter = "S")]
        public int Note2Income { get; set; }

        [Column(MappingDirections.Both, Letter = "T")]
        public int WasteVolume { get; set; }
    }
}
