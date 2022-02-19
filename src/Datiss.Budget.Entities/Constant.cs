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
            //TODO : Add initializers for the rest of the collections
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

        public ICollection<UserTypeAverageCapacity> UserTypeAverageCapacities { get; set; }

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


        #endregion
    }
}