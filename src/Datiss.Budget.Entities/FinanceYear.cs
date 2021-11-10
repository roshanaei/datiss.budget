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

        public ICollection<SalesSplitFunction> SalesSplitFunctions { get; set; }

        public ICollection<UserTypeAverageCapacity> UserTypeAverageCapacities { get; set; }

        public ICollection<IncomeForcast> IncomeForcasts { get; set; }

        public ICollection<IncomeForcastWs> IncomeForcastWs { get; set; }

        public ICollection<WWsFee> WWsFees { get; set; }


        #endregion
    }
}