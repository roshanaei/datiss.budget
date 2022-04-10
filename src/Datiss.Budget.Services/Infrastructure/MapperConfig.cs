using System.Linq;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Services.Infrastructure
{
    public static class MapperConfig
    {
        /// <summary>
        /// Config default maps for mapster. You should call this method in DI before using project's services.
        /// </summary>
        public static void Config() 
        {
            TypeAdapterConfig<User, UserResultDTO>
                .NewConfig()
                .Map(d => d.OrganizationTitle, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.PositionTitle, s => s.Position != null ? s.Position.Title : null)
                .Map(d => d.SelectedRoles, s => s.Roles != null && s.Roles.Any() 
                                                ? s.Roles.Select(_ => _.RoleId).ToList() 
                                                : null);
            TypeAdapterConfig<CostCurrentConstructionW, CostCurrentConstructionWDTO>
                .NewConfig()
                .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                .Map(d => d.WaterInvestorsDisplay, s => s.WaterInvestors != null ? s.WaterInvestors.Title : null)
                .Map(d => d.CostCenterDisplay, s => s.CostCenter != null ? s.CostCenter.Title : null)
                .Map(d => d.ExploitationAreaDisplay, s => s.ExploitationArea != null ? s.ExploitationArea.Title : null)
                .Map(d => d.MeasurementDisplay, s => s.Measurement != null ? s.Measurement.Title : null)
                .Map(d => d.CreditDisplay, s => s.Credit != null ? s.Credit.Title : null)
                .Map(d => d.ExtensionDisplay, s => s.Extension != null ? s.Extension.Title : null)
                .Map(d => d.SuggestedBudgetTopicDisplay, s => s.SuggestedBudgetTopic != null ? s.SuggestedBudgetTopic.Title : null);

            TypeAdapterConfig<CostForcastConstructionWs, CostForcastConstructionWsDTO>
                .NewConfig()
                .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                .Map(d => d.WasteInvestorsDisplay, s => s.WasteInvestors != null ? s.WasteInvestors.Title : null)
                .Map(d => d.CostCenterDisplay, s => s.CostCenter != null ? s.CostCenter.Title : null)
                .Map(d => d.ExploitationAreaDisplay, s => s.ExploitationArea != null ? s.ExploitationArea.Title : null)
                .Map(d => d.MeasurementDisplay, s => s.Measurement != null ? s.Measurement.Title : null)
                .Map(d => d.CreditDisplay, s => s.Credit != null ? s.Credit.Title : null)
                .Map(d => d.ExtensionDisplay, s => s.Extension != null ? s.Extension.Title : null)
                .Map(d => d.SuggestedBudgetTopicDisplay, s => s.SuggestedBudgetTopic != null ? s.SuggestedBudgetTopic.Title : null);


        }
    }
}
