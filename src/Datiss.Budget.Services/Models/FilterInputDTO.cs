using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{

    public abstract class FilterInputDTO
    {
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int StartIndex => (PageNumber * PageSize) - PageSize;
    }

    public enum InstallFeeFilterMode
    {
        Exact = 0,
        LessThan = 1,
        GreaterThan = 2
    }


    public class OrganizationFilterDTO : FilterInputDTO
    {
        public int? ParentId { get; set; }
        public OrganizationType? Type { get; set; }
        public bool? SewageStatus { get; set; }
        public EntityStatus? Status { get; set; }
    }
    public class ConstantFilterDTO : FilterInputDTO
    {
        public int? ParentId { get; set; }
        public string ConstantKey { get; set; }
    }
    public class FinanceYearFilterDTO : FilterInputDTO
    {

    }
    public class WaterInstallFeeFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DWaterTypeId { get; set; }
        public int? WInstallFee { get; set; }
        public InstallFeeFilterMode FeeMode { get; set; }
    }

    public class WasteInstallFeeFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
    }

    public class WaterSalesSplitFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? WPipeDiameterId { get; set; }
        public int? NumberSales { get; set; }
        public int? UnitSales { get; set; }
        public InstallFeeFilterMode NumberMode { get; set; }
        public InstallFeeFilterMode UnitMode { get; set; }
    }

    public class BranchFeeAmountFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public decimal? UrbanAdjustmentFactor { get; set; }

        public decimal? WasteRateInWater { get; set; }

        public int? WaterBranchingPerHousing { get; set; }

        public int? TubingCost { get; set; }

        public int? WaterPartnershipAmountDomestic { get; set; }

        public int? WaterPartnershipAmountNDomestic { get; set; }

        public int? WastePartnershipAmountDomestic { get; set; }

        public int? WastePartnershipAmountNDomestic { get; set; }

        public int? FixCostNote11H { get; set; }

        public int? FixCostNote11NH { get; set; }

        public int? FixCostNote11HWs { get; set; }

        public int? FixCostNote11NHWs { get; set; }

        public int? WsTubingCost { get; set; }
    }

    public class WasteSalesSplitFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? WsPipeDiameterId { get; set; }
        public int? NumberSales { get; set; }
        public int? UnitSales { get; set; }
        public InstallFeeFilterMode NumberMode { get; set; }
        public InstallFeeFilterMode UnitMode { get; set; }
    }
    public class UserTypeAverageCapacityFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }

    public class IncomeForcastFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }

    public class IncomeForcastWsFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class WWsFeeFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? UsageLayerId { get; set; }
        public ActivityType? ActivityType { get; set; }
    }
    public class FeeCityFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
    }
    public class SubscriptionFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class IncomeCurrentWHFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? UsageLayerId { get; set; }
    }
    public class IncomeCurrentWNHFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class PerformanceEvaluationFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
