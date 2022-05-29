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
    public class UserTypeAverageCapacityForcastFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
    }
    public class UserTypeAverageCapacityCurrentFilterDTO : FilterInputDTO
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
        public TablesName? TableName { get; set; }
        public SectionName? SectionName { get; set; }
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
        public CofficientsGroup? GroupName { get; set; }
    }
    public class IncomeCurrentCofficientFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserTypeId { get; set; }
        public int? UsageLayerId { get; set; }
    }
    public class IncomeCurrentReportFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? NumberYear { get; set; }
        public int? OrganizationId { get; set; }
        public int? SectionTypeId { get; set; }
        public int? UnitTypeId { get; set; }
        public ActivityType? Activity { get; set; }
    }

    public class CostCurrentPMDepFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public RecordType? RecordType { get; set; }
        public int? CCPMDepTypeId { get; set; }
        public int? CostCenterTypeId { get; set; }
    }

    public class CostCurrentInstalationFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }
        public int? CCInstalationTypeId { get; set; }
    }

    public class CostCurrentElectricityFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }

    }

    public class CostCurrentConsumableFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }
        public int? ConsumableTypeId { get; set; }

    }

    public class CostCurrentBankFeeFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CostCenterTypeId { get; set; }
    }

    public class CostCurrentNOFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public int? CostCurrentNoTypeId { get; set; }
    }

    public class CostCurrentEPaymentFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
    }
    public class CostCurrentContractualFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CostCenterTypeId { get; set; }
        public bool? ExtensionId { get; set; }
    }

    public class CostCurrentOtherFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CostCenterTypeId { get; set; }
        public int? CCOtherCostsTypeId { get; set; }
    }


    public class CostCurrentSharingSetadFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

    }

    public class CostCurrentFinancingFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? FinancialCostTypeId { get; set; }
    }

    public class CostCurrentWaterSourceFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? WaterSourceTypeId { get; set; }
    }

    public class CostForcastConstructionWFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

    }

    public class CostForcastTransferWFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

    }

    public class CostForcastConstructionWsFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

    }

    public class CostCurrentRawMaterialFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }
        public int? RawMaterialTypeId { get; set; }
    }


    public class CostCurrentPersonelFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public RecordType? RecordType { get; set; }
    }

    public class CostForcastTransferWsFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
    }

    public class CostCurrentReportFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? NumberYear { get; set; }
        public int? OrganizationId { get; set; }
    }

    public class BudgetSourceReportFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? NumberYear { get; set; }
        public int? SectionTypeId { get; set; }
    }

    public class CostForcastBuyFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? LocationId { get; set; }
    }

    public class CostForcastFinanceFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public int? CostCenterTypeId { get; set; }
        public int? FinanceSubjectTypeId { get; set; }
    }

    public class DefaultFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
    }

    public class CostCurrentPrescriptionBaseInfoFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }
    }

    public class CostForcastPipingWFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }

    }

    public class CostForcastPipingWsFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }

    }

    public class CostForcastBuyDescriptionFilterDTO : FilterInputDTO
    {
        public int? YearId { get; set; }

    }
}
