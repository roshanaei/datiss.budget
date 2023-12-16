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
        public long Cost { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public decimal AvgConsumeUser { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public int ConsumptionUser { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public long Income { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public long SubscriptionIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public long SeasonalIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public long TIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "P")]
        public long Note3Price { get; set; }

        [Column(MappingDirections.Both, Letter = "Q")]
        public long Note3Income { get; set; }

        [Column(MappingDirections.Both, Letter = "R")]
        public int Diff_ConsWsVolume { get; set; }

        [Column(MappingDirections.Both, Letter = "S")]
        public long Note2Income { get; set; }

        [Column(MappingDirections.Both, Letter = "T")]
        public int WasteVolume { get; set; }
    }
}
