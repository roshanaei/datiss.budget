using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeCurrentWNHImportModel
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
        public decimal AvgConsumeUser { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int ConsumptionUser { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public decimal ExcessConsumption { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long Cost { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public long Income { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public long ExcessIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public long SeasonalIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public long Note3Price { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public long Note3Income { get; set; }

        [Column(MappingDirections.Both, Letter = "P")]
        public long SubscriptionIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "Q")]
        public decimal TotalIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "R")]
        public long Note2Income { get; set; }

        [Column(MappingDirections.Both, Letter = "S")]
        public int WasteVolume { get; set; }

        [Column(MappingDirections.Both, Letter = "T")]
        public int Diff_ConsWsVolume { get; set; }

    }
}
