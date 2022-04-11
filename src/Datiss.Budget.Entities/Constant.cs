using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities
{
    public class Constant : IAuditableEntity
    {
        public Constant()
        {
            Childrens = new HashSet<Constant>();
            WaterInstallFees = new HashSet<WaterInstallFee>();
            UserPositions = new HashSet<User>();
            WasteInstallFees = new HashSet<WasteInstallFee>();
            WaterPipeDiameterSalessplit = new HashSet<WaterSalesSplit>();
            UserTypeIncomeCurrentCofficients = new HashSet<IncomeCurrentCofficient>();
            UsageLayerIncomeCurrentCofficients = new HashSet<IncomeCurrentCofficient>();
        }

        #region Properties

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public EntityStatus Status { get; set; }

        public int DisplayOrder { get; set; }

        #endregion

        #region Navigations
        public Constant Parent { get; set; }
        public ICollection<Constant> Childrens { get; set; }

        public ICollection<WaterInstallFee> WaterInstallFees { get; set; }

        public ICollection<WasteInstallFee> WasteInstallFees { get; set; }

        public ICollection<WaterSalesSplit> WaterPipeDiameterSalessplit { get; set; }
        public ICollection<WaterSalesSplit> UserTypeWaterSalesSplit { get; set; }

        public ICollection<WasteSalesSplit> UserTypeWasteSalesSplit { get; set; }
        public ICollection<WasteSalesSplit> WastepipeDiameterSalesSplit { get; set; }

        public ICollection<AverageContractedCapacityNHUses> AverageContractedCapacityNHUses { get; set; }

        public ICollection<UserTypeAverageCapacityForcast> UserTypeAverageCapacityForcasts { get; set; }

        public ICollection<UserTypeAverageCapacityCurrent> UserTypeAverageCapacityCurrents { get; set; }

        public ICollection<IncomeForcast> IncomeForcasts { get; set; }

        public ICollection<IncomeForcastWs> IncomeForcastWs { get; set; }

        public ICollection<WWsFee> WWsFee { get; set; }

        public ICollection<WWsFee> UsageLayerWWsFee { get; set; }

        public ICollection<SalesSplitTotal> SalesSplitTotal { get; set; }

        public ICollection<Subscription> Subscription { get; set; }

        public ICollection<IncomeCurrentWH> IncomeCurrentWH { get; set; }

        public ICollection<IncomeCurrentWH> UsageLayerIncomeCurrentWH { get; set; }


        public ICollection<IncomeCurrentWNH> IncomeCurrentWNH { get; set; }

        public ICollection<IncomeCurrentWsH> IncomeCurrentWsH { get; set; }

        public ICollection<IncomeCurrentWsH> UsageLayerIncomeCurrentWsH { get; set; }

        public ICollection<IncomeCurrentWsNH> IncomeCurrentWsNH { get; set; }

        public ICollection<ConsumeForcast> ConsumeForcast { get; set; }

        public ICollection<ConsumeForcast> UsageLayerConsumeForcast { get; set; }

        public ICollection<IncomeCurrentCofficient> UserTypeIncomeCurrentCofficients { get; set; }

        public ICollection<IncomeCurrentCofficient> UsageLayerIncomeCurrentCofficients { get; set; }

        public ICollection<ConsumeForcastWs> ConsumeForcastWs { get; set; }

        public ICollection<ConsumeForcastWs> UsageLayerConsumeForcastWs { get; set; }

        public ICollection<IncomeForcastOther> IncomeForcastOthers { get; set; }

        public ICollection<IncomeCurrentNOperational> IncomeCurrentNOperationals { get; set; }

        public ICollection<IncomeCurrentOperational> IncomeCurrentOperationals { get; set; }

        public ICollection<User> UserPositions { get; set; }

        public ICollection<BranchingRateIncrease> BranchingRateIncrease { get;set;}

        public ICollection<Cofficient> Cofficients { get;set;}

        public ICollection<IncomeCurrentReport> CurrentIncomeReports { get; set; }
        
        public ICollection<IncomeCurrentReport> UnitTypeCurrentIncomeReports { get; set; }

        public ICollection<CostCurrentInstalation> CostCurrentInstalations { get; set; }

        public ICollection<CostCurrentPMDep> CostCurrentPMDeps { get; set; }

        public ICollection<CostCurrentPMDep> CostCenterCostCurrentPMDeps { get; set; }

        public ICollection<CostCurrentConsumable> CostCurrentConsumable { get; set; }

        public ICollection<CostCurrentBankFee> CostCurrentBankFee { get; set; }

        public ICollection<CostCurrentContractual> CostCurrentContractual { get; set; }

        public ICollection<CostCurrentOther> CostCenterCostCurrentOther { get; set; }

        public ICollection<CostCurrentOther> CCOtherCostsCostCurrentOther { get; set; }

        public ICollection<CostCurrentNO> CostCurrentNO { get; set; }

        public ICollection<CostCurrentFinancing> CostCurrentFinancing { get; set; }

        public ICollection<CostCurrentWaterSource> CostCurrentWaterSource { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWInvestors { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWCostCenters { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWExploitationArea { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWMeasurement { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWCredit { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWExtension { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionWSuggestedBudgetTopic { get; set; }

         public ICollection<CostForcastConstructionWs> CostForcastConstructionWsInvestors { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWsCostCenters { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWsExploitationArea { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWsMeasurement { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWsCredit { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWsExtension { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWsSuggestedBudgetTopic { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferWTransfer { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferWCreadit { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferWDig { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferWTube { get; set; }
        
        public ICollection<CostForcastTransferW> CostForcastTransferWDiameterPipe { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferWExtension { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferWSuggestedBudgetTopic { get; set; }
        
         public ICollection<CostForcastTransferWs> CostForcastTransferWsTransfer { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsCreadit { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsDig { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsMethod { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsTube { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsDiameterPipe { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsExtension { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWsSuggestedBudgetTopic { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonelCostCenter { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonelGrade { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonelContract { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonelJobDepartment { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonelJobStatus { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonelJobStatusDetail { get; set; }


        #endregion
    }
}