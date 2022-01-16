using Ganss.Excel;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Excel
{
    public class IncomeCurrentOperationalImportModel
    {

        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public ActivityType ActivityType { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public string ICOTypeTitle { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int ICOTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int CountH { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int PriceH { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int CountNH { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int PriceNH { get; set; }
}
}
