using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities
{
    public class FinanceYear : IAuditableEntity
    {
        public FinanceYear()
        {
            WaterInstallFees = new HashSet<WaterInstallFee>();
            IncomeCurrentCofficients = new HashSet<IncomeCurrentCofficient>();
            CostCurrentReports = new HashSet<CostCurrentReport>();
            BudgetSourceReports = new HashSet<BudgetSourceReport>();
            CostForcastFinance = new HashSet<CostForcastFinance>();
            CostCurrentWaterSourcePrices = new HashSet<CostCurrentWaterSourcePrice>();
            CostCurrentPrescriptionBaseInfo = new HashSet<CostCurrentPrescriptionBaseInfo>();
        }

        #region Properties

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Year { get; set; }

        public EntityStatus Status { get; set; }

        #endregion

        #region Navigation

        public ICollection<WaterInstallFee> WaterInstallFees { get; set; }

        public ICollection<WasteInstallFee> WasteInstallFees { get; set; }

        public ICollection<WaterSalesSplit> WaterSalesSplits { get; set; }

        public ICollection<BranchFeeAmount> BranchFeeAmounts { get; set; }

        public ICollection<WasteSalesSplit> WasteSalesSplits { get; set; }

        public ICollection<AverageContractedCapacityNHUses> AverageContractedCapacityNHUses { get; set; }

        public ICollection<UserTypeAverageCapacityForcast> UserTypeAverageCapacityForcasts { get; set; }

        public ICollection<UserTypeAverageCapacityCurrent> UserTypeAverageCapacityCurrents { get; set; }

        public ICollection<IncomeForcast> IncomeForcasts { get; set; }

        public ICollection<IncomeForcastWs> IncomeForcastWs { get; set; }

        public ICollection<WWsFee> WWsFee { get; set; }

        public ICollection<PerformanceEvaluation> PerformanceEvaluation { get; set; }

        public ICollection<SalesSplitTotal> SalesSplitTotal { get; set; }

        public ICollection<FeeCity> FeeCity { get; set; }

        public ICollection<Subscription> Subscription { get; set; }

        public ICollection<IncomeCurrentWH> IncomeCurrentWH { get; set; }

        public ICollection<IncomeCurrentWNH> IncomeCurrentWNH { get; set; }

        public ICollection<IncomeCurrentWsH> IncomeCurrentWsH { get; set; }

        public ICollection<IncomeCurrentWsNH> IncomeCurrentWsNH { get; set; }

        public ICollection<ConsumeForcast> ConsumeForcast { get; set; }

        public ICollection<ConsumeForcastWs> ConsumeForcastWs { get; set; }

        public ICollection<IncomeForcastOther> IncomeForcastOthers { get; set; }

        public ICollection<IncomeCurrentNOperational> IncomeCurrentNOperationals { get; set; }

        public ICollection<IncomeCurrentOperational> IncomeCurrentOperationals { get; set; }

        public ICollection<BranchingRateIncrease> BranchingRateIncrease { get; set; }

        public ICollection<NHCo> NHCo { get; set; }

        public ICollection<Cofficient> Cofficients { get; set; }

        public ICollection<IncomeCurrentCofficient> IncomeCurrentCofficients { get; set; }

        public ICollection<IncomeCurrentReport> CurrentIncomeReports { get; set; }

        public ICollection<CostCurrentInstalation> CostCurrentInstalations { get; set; }

        public ICollection<CostCurrentPMDep> CostCurrentPMDeps { get; set; }

        public ICollection<CostCurrentElectricity> CostCurrentElectricity { get; set; }

        public ICollection<CostCurrentConsumable> CostCurrentConsumable { get; set; }

        public ICollection<CostCurrentEPayment> CostCurrentEPayment { get; set; }

        public ICollection<CostCurrentBankFee> CostCurrentBankFee { get; set; }

        public ICollection<CostCurrentContractual> CostCurrentContractual { get; set; }

        public ICollection<CostCurrentOther> CostCurrentOther { get; set; }

        public ICollection<CostCurrentSharingSetad> CostCurrentSharingSetad { get; set; }

        public ICollection<CostCurrentNO> CostCurrentNO { get; set; }

        public ICollection<CostCurrentFinancing> CostCurrentFinancing { get; set; }

        public ICollection<CostCurrentWaterSource> CostCurrentWaterSource { get; set; }

        public ICollection<CostForcastConstructionW> CostForcastConstructionW { get; set; }

        public ICollection<CostForcastConstructionWs> CostForcastConstructionWs { get; set; }

        public ICollection<CostForcastTransferW> CostForcastTransferW { get; set; }

        public ICollection<CostForcastTransferWs> CostForcastTransferWs { get; set; }

        public ICollection<CostCurrentPersonel> CostCurrentPersonel { get; set; }

        public ICollection<CostCurrentRawMaterial> CostCurrentRawMaterial { get; set; }

        public ICollection<CostForcastBuy> CostForcastBuy { get; set; }

        public ICollection<CostCurrentReport> CostCurrentReports { get; set; }

        public ICollection<BudgetSourceReport> BudgetSourceReports { get; set; }

        public ICollection<CostForcastFinance> CostForcastFinance { get; set; }

        public ICollection<CostCurrentWaterSourcePrice> CostCurrentWaterSourcePrices { get; set; }

        public ICollection<CostCurrentPrescriptionBaseInfo> CostCurrentPrescriptionBaseInfo { get; set; }

        public ICollection<CostForcastPipingW> CostForcastPipingW { get; set; }

        public ICollection<CostForcastPipingWs> CostForcastPipingWs { get; set; }

        public ICollection<CostForcastBuyDescription> CostForcastBuyDescription { get; set; }

        public ICollection<CostForcastConsumptionReport> CostForcastConsumptionReport { get; set; }

        #endregion
    }
}