using Datiss.Budget.Common;
using Datiss.Budget.Services;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Excel;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Reports;
using Datiss.Budget.Reports.Contracts;
using Datiss.Budget.Services.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    //IOC : Add All services in this file 
    public static class AddDatissBudgetServicesExtensions
    {

        public static IServiceCollection AddDatissBudgetServices(this IServiceCollection services)
        {
            services.AddSingleton<IExcelService, ExcelService>();

            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IDateService, DateService>();

            MapperConfig.Config();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IConstantService, ConstantService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IFinanceYearService, FinanceYearService>();
            services.AddScoped<IWaterInstallFeeService, WaterInstallFeeService>();
            services.AddScoped<IWasteInstallFeeService, WasteInstallFeeService>();
            services.AddScoped<IWaterSalesSplitService, WaterSalesSplitService>();
            services.AddScoped<IWasteSalesSplitService, WasteSalesSplitService>();
            services.AddScoped<IBranchFeeAmountService, BranchFeeAmountService>();
            services.AddScoped<IConsumeForcastService, ConsumeForcastService>();
            services.AddScoped<IConsumeForcastWsService, ConsumeForcastWsService>();
            services.AddScoped<IIncomeForcastService, IncomeForcastService>();
            services.AddScoped<IIncomeForcastWsService, IncomeForcastWsService>();
            services.AddScoped<INHCoService, NHCoService>();
            services.AddScoped<IBranchingRateIncreaseService, BranchingRateIncreaseService>();
            services.AddScoped<IUserTypeAverageCapacityForcastService, UserTypeAverageCapacityForcastService>();
            services.AddScoped<IUserTypeAverageCapacityCurrentService, UserTypeAverageCapacityCurrentService>();
            services.AddScoped<IIncomeForcastOtherService, IncomeForcastOtherService>();
            services.AddScoped<ISalesSplitTotalService, SalesSplitTotalService>();
            services.AddScoped<IFeeCityService, FeeCityService>();
            services.AddScoped<IIncomeCurrentWHService, IncomeCurrentWHService>();
            services.AddScoped<IIncomeCurrentWsHService, IncomeCurrentWsHService>();
            services.AddScoped<IIncomeCurrentWNHService, IncomeCurrentWNHService>();
            services.AddScoped<IIncomeCurrentWsNHService, IncomeCurrentWsNHService>();
            services.AddScoped<IPerformanceEvaluationService, PerformanceEvaluationService>();
            services.AddScoped<ICofficientService, CofficientService>();
            services.AddScoped<IIncomeCurrentCofficientService, IncomeCurrentCofficientService>();
            services.AddScoped<ITablesFieldTitleService, TablesFieldTitleService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IWWsFeeService, WWsFeeService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPerformanceEvaluationService, PerformanceEvaluationService>();
            services.AddScoped<IIncomeCurrentReportService, IncomeCurrentReportService>();
            services.AddScoped<IIncomeCurrentOperationalService, IncomeCurrentOperationalService>();
            services.AddScoped<IReportEngine, ReportEngine>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAppClaimTypeService, AppClaimTypeService>();
            services.AddScoped<IIncomeCurrentNOperationalService, IncomeCurrentNOperationalService>();
            services.AddScoped<ICostCurrentPMDepService, CostCurrentPMDepService>();
            services.AddScoped<ICostCurrentInstallationService, CostCurrentInstallationService>();
            services.AddScoped<ICostCurrentElectricityService, CostCurrentElectricityService>();
            services.AddScoped<ICostCurrentConsumableService, CostCurrentConsumableService>();
            services.AddScoped<ICostCurrentBankFeeService, CostCurrentBankFeeService>();
            services.AddScoped<ICostCurrentOtherService, CostCurrentOtherService>();
            services.AddScoped<ICostCurrentEPaymentService, CostCurrentEPaymentService>();
            services.AddScoped<ICostCurrentContractualService, CostCurrentContractualService>();
            services.AddScoped<ICostCurrentNOService, CostCurrentNOService>();
            services.AddScoped<ICostCurrentSharingSetadService, CostCurrentSharingSetadService>();
            services.AddScoped<ICostCurrentFinancingService, CostCurrentFinancingService>();
            services.AddScoped<ICostCurrentWaterSourceService, CostCurrentWaterSourceService>();
            services.AddScoped<ICostForcastConstructionWService, CostForcastConstructionWService>();
            services.AddScoped<ICostForcastTransferWService, CostForcastTransferWService>();
            services.AddScoped<ICostForcastTransferWsService, CostForcastTransferWsService>();
            services.AddScoped<ICostForcastConstructionWsService, CostForcastConstructionWsService>();
            services.AddScoped<ICostForcastBuyService, CostForcastBuyService>();
            services.AddScoped<ICostCurrentRawMaterialService, CostCurrentRawMaterialService>();
            services.AddScoped<ICostCurrentPersonelService, CostCurrentPersonelService>();
            services.AddScoped<ICostCurrentReportService, CostCurrentReportService>();
            services.AddScoped<IBudgetSourceReportService, BudgetSourceReportService>();

          
            return services;
        }
    }
}
