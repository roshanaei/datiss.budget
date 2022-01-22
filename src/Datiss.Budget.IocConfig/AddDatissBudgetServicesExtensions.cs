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

        public static IServiceCollection AddDatissBudgetServices(this IServiceCollection services) {
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
            services.AddScoped<IUserTypeAverageCapacityService, UserTypeAverageCapacityService>();
            services.AddScoped<IIncomeForcastOtherService, IncomeForcastOtherService>();
            services.AddScoped<ISalesSplitTotalService, SalesSplitTotalService>();
            services.AddScoped<IFeeCityService, FeeCityService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPerformanceEvaluationService, PerformanceEvaluationService>();
            services.AddScoped<IReportEngine, ReportEngine>();

            return services;
        }
    }
}
