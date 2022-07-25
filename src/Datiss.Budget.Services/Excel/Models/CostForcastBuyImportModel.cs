using Ganss.Excel;

namespace Datiss.Budget.Services.Excel
{
    public class CostForcastBuyImportModel
    {
        [Column(MappingDirections.Both, Letter = "A")]
        public string OrganizationDisplay { get; set; }

        [Column(MappingDirections.Both, Letter = "B")]
        public int OrganizationId { get; set; }

        [Column(MappingDirections.Both, Letter = "C")]
        public string BuyDescription { get; set; }

        [Column(MappingDirections.Both, Letter = "D")]
        public int LocationId { get; set; }

        [Column(MappingDirections.Both, Letter = "E")]
        public int BuyDepartmentId { get; set; }

        [Column(MappingDirections.Both, Letter = "F")]
        public int CostCenterTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "G")]
        public int AssetTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "H")]
        public int AssetDetailTypeId { get; set; }

        [Column(MappingDirections.Both, Letter = "I")]
        public int Amount { get; set; }

        [Column(MappingDirections.Both, Letter = "J")]
        public int CreditTypeId { get; set; }

    }
}