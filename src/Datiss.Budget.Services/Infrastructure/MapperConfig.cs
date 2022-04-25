using System.Linq;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Excel.Models;
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
            TypeAdapterConfig<CostForcastConstructionW, CostForcastConstructionWDTO>
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

            TypeAdapterConfig<CostForcastTransferW, CostForcastTransferWDTO>
                  .NewConfig()
                  .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                  .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                  .Map(d => d.TransferTypeDisplay, s => s.TransferType != null ? s.TransferType.Title : null)
                  .Map(d => d.DigTypeDisplay, s => s.DigType != null ? s.DigType.Title : null)
                  .Map(d => d.TubeTypeDisplay, s => s.TubeType != null ? s.TubeType.Title : null)
                  .Map(d => d.DiameterPipeTypeDisplay, s => s.DiameterType != null ? s.DiameterType.Title : null)
                  .Map(d => d.CreditTypeDisplay, s => s.Credit != null ? s.Credit.Title : null)
                  .Map(d => d.ExtensionTypeDisplay, s => s.Extension != null ? s.Extension.Title : null)
                  .Map(d => d.SuggestedBudgetTopicTypeDisplay, s => s.SuggestedBudgetTopic != null ? s.SuggestedBudgetTopic.Title : null);

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

            TypeAdapterConfig<CostCurrentRawMaterial, CostCurrentRawMaterialDTO>
                .NewConfig()
                .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                .Map(d => d.RawMaterialTypeDisplay, s => s.RawMaterial != null ? s.RawMaterial.Title : null);

            TypeAdapterConfig<CostCurrentPersonel, CostCurrentPersonelDTO>
                .NewConfig()
                .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                .Map(d => d.CostCenterTypeDisplay, s => s.CostCenter != null ? s.CostCenter.Title : null)
                .Map(d => d.GradeTypeDisplay, s => s.Grade != null ? s.Grade.Title : null)
                .Map(d => d.ContractTypeDisplay, s => s.Contract != null ? s.Contract.Title : null)
                .Map(d => d.JobDepartmentTypeDisplay, s => s.JobDepartment != null ? s.JobDepartment.Title : null)
                .Map(d => d.JobStatusDetailTypeDisplay, s => s.JobStatusDetail != null ? s.JobStatusDetail.Title : null)
                .Map(d => d.JobStatusTypeDisplay, s => s.JobStatus != null ? s.JobStatus.Title : null);

            TypeAdapterConfig<CostForcastTransferWs, CostForcastTransferWsDTO>
                  .NewConfig()
                  .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                  .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                  .Map(d => d.TransferTypeDisplay, s => s.TransferType != null ? s.TransferType.Title : null)
                  .Map(d => d.DigTypeDisplay, s => s.DigType != null ? s.DigType.Title : null)
                  .Map(d => d.MethodTypeDisplay, s => s.MethodType != null ? s.MethodType.Title : null)
                  .Map(d => d.TubeTypeDisplay, s => s.TubeType != null ? s.TubeType.Title : null)
                  .Map(d => d.DiameterPipeTypeDisplay, s => s.DiameterType != null ? s.DiameterType.Title : null)
                  .Map(d => d.CreditTypeDisplay, s => s.Credit != null ? s.Credit.Title : null)
                  .Map(d => d.ExtensionTypeDisplay, s => s.Extension != null ? s.Extension.Title : null)
                  .Map(d => d.SuggestedBudgetTopicTypeDisplay, s => s.SuggestedBudgetTopic != null ? s.SuggestedBudgetTopic.Title : null);

            TypeAdapterConfig<CostForcastBuy, CostForcastBuyDTO>
                  .NewConfig()
                  .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                  .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                  .Map(d => d.LocationDisplay, s => s.Location != null ? s.Location.Title : null)
                  .Map(d => d.BuyDepartmentDisplay, s => s.Department != null ? s.Department.Title : null)
                  .Map(d => d.CostCenterTypeDisplay, s => s.CostCenter != null ? s.CostCenter.Title : null)
                  .Map(d => d.AssetTypeDisplay, s => s.Asset != null ? s.Asset.Title : null)
                  .Map(d => d.AssetDetailTypeDisplay, s => s.AssetDetail != null ? s.AssetDetail.Title : null)
                  .Map(d => d.CreditTypeDisplay, s => s.Credit != null ? s.Credit.Title : null)
                  .Map(d => d.MeasurementTypeDisplay, s => s.Measurement != null ? s.Measurement.Title : null);

            TypeAdapterConfig<CostCurrentPersonelImportModel, CostCurrentPersonel>
                .NewConfig()
                .Map(d => d.GenderId, s => s.GenderVal == 0 ? false : true);

            TypeAdapterConfig<CostCurrentReport, CostCurrentReportDTO>
                  .NewConfig()
                  .Map(d => d.OrganizationDisplay, s => s.Organization != null ? s.Organization.Title : null)
                  .Map(d => d.Year, s => s.FinanceYear != null ? s.FinanceYear.Year : 0)
                  .Map(d => d.SectionTypeDisplay, s => s.SectionType != null ? s.SectionType.Title : null)
                  .Map(d => d.UnitTypeDisplay, s => s.UnitType != null ? s.UnitType.Title : null)
                  .Map(d => d.UnitDetailTypeDisplay, s => s.UnitDetailType != null ? s.UnitDetailType.Title : null)
                  .Map(d => d.CostCenterTypeDisplay, s => s.CostCenterType != null ? s.CostCenterType.Title : null);
        }
    }
}
