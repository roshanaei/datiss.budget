using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{

    public abstract class FilterInputDTO
    {
        public string Search { get; set; }
        public string Columns { get; set; }
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
        public int? OrganizationId { get; set; }
        public OrganizationType? Type { get; set; }
        public bool? SewageStatus { get; set; }
        public EntityStatus? Status { get; set; }
    }
    public class ConstantFilterDTO : FilterInputDTO
    {
        public int? ParentId { get; set; }
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
        public int? DWasteTypeId { get; set; }
        public int? WsInstallFee { get; set; }
        public InstallFeeFilterMode FeeMode { get; set; }
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
        public int? SubW { get; set; }
        public int? SubWs { get; set; }

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
        public TablesName? tableNames { get; set; }
    }
    public class ConsumeForcastFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? UsageLayerId { get; set; }
    }
    public class ConsumeForcastWsFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? UsageLayerId { get; set; }
    }
    public class IncomeCurrentNOperationalFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? NOICType { get; set; }
    }
    public class IncomeCurrentOperationalFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? Type { get; set; }
        public int? ICOTypeId { get; set; }
    }
    public class IncomeCurrentWsHFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? UsageLayerId { get; set; }
    }
    public class IncomeCurrentWsNHFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class IncomeForcastOtherFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? Type { get; set; }
        public int? OIFTypeId { get; set; }
    }
    public class SalesSplitTotalFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class NHCoFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }

    }
    public class BranchingRateIncreaseFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class CofficientFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CofficientTypeId { get; set; }
    }
}
