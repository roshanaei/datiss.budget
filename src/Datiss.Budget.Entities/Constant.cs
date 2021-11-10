using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities
{
    public class Constant : IAuditableEntity
    {
        public Constant()
        {
            Childrens = new HashSet<Constant>();
            WaterInstallFees = new HashSet<WaterInstallFee>();
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

        public ICollection<SalesSplitFunction> SalesSplitFunctions { get; set; }

        public ICollection<UserTypeAverageCapacity> UserTypeAverageCapacities { get; set; }

        public ICollection<IncomeForcast> IncomeForcasts { get; set; }

        public ICollection<IncomeForcastWs> IncomeForcastWs { get; set; }

        public ICollection<WWsFee> WWsFees { get; set; }

        #endregion
    }
}