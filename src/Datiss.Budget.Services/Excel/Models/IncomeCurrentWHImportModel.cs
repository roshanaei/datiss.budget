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
        [Column(0, MappingDirections.Both, Letter = "A")]
        public int YearId { get; set; }
        [Column(1, MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }
        [Column(2, MappingDirections.Both, Letter = "C")]
        public int UserTypeId { get; set; }
        [Column(3, MappingDirections.Both, Letter = "D")]
        public int UsageLayerId { get; set; }
        [Column(4, MappingDirections.Both, Letter = "E")]
        public int NumberUser { get; set; }
        [Column(5, MappingDirections.Both, Letter = "F")]
        public int UnitUser { get; set; }
        [Column(6, MappingDirections.Both, Letter = "G")]
        public decimal AvgConsumeUser { get; set; }
        [Column(7, MappingDirections.Both, Letter = "H")]
        public int ConsumptionUser { get; set; }
        [Column(8, MappingDirections.Both, Letter = "I")]
        public int Cost { get; set; }
        [Column(9, MappingDirections.Both, Letter = "J")]
        public int Note3Price { get; set; }
        [Column(10, MappingDirections.Both, Letter = "K")]
        public int Income { get; set; }
        [Column(11, MappingDirections.Both, Letter = "L")]
        public int Note3Income { get; set; }
        [Column(12, MappingDirections.Both, Letter = "M")]
        public int SubscriptionIncome { get; set; }
        [Column(13, MappingDirections.Both, Letter = "N")]
        public int SeasonalIncome { get; set; }
        [Column(14, MappingDirections.Both, Letter = "O")]
        public int TIncome { get; set; }
        [Column(15, MappingDirections.Both, Letter = "P")]
        public int Diff_ConsWsVolume { get; set; }
        [Column(16, MappingDirections.Both, Letter = "Q")]
        public int Note2Income { get; set; }
        [Column(17, MappingDirections.Both, Letter = "R")]
        public int WasteVolume { get; set; }
        [Column(18, MappingDirections.Both, Letter = "S")]
        public int Note7Price { get; set; }
        [Column(19, MappingDirections.Both, Letter = "T")]
        public int Note7Income { get; set; }
    }
}
