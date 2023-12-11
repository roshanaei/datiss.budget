using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Excel.Models
{
    public class IncomeCurrentWsNHImportModel
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
        public decimal ExcessConsumption { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int ConsumptionUser { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long Cost { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public long Income { get; set; }

        [Column(MappingDirections.Both, Letter = "L")]
        public long SubscriptionIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "M")]
        public long ExcessIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "N")]
        public long SeasonalIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "O")]
        public long Note3Price { get; set; }

        [Column(MappingDirections.Both, Letter = "P")]
        public long TotalIncome { get; set; }

        [Column(MappingDirections.Both, Letter = "Q")]
        public long Note3Income { get; set; }

    }
}
