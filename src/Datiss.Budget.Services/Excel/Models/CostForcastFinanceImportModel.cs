using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastFinanceImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string CostCenterTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public string FinanceSubjectTypeDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int FinanceSubjectTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public long RemainingAssets { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public long AssetsCreated6_1 { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public long AssetsCreated6_2 { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public long ForcastAssets_D { get; set; }

        [Column(MappingDirections.Both, Letter = "K")]
        public long TotalAssetsCreated_D { get; set; }

    }
}
