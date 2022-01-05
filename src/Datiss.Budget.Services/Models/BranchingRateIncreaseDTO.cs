using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateBranchingRateIncreaseDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeTitle { get; set; }
        public int WaterRateIncrease { get; set; }
        public int WasteRateIncrease { get; set; }
        public int WastePersentIncrease { get; set; }
        public int FixAmountBusiness { get; set; }
        public int CapacityFixAmount { get; set; }
        public int WaterInstallRateIncrease { get; set; }
        public int WsInstalIncrease { get; set; }
        public int WaterFixNote2 { get; set; }
        public int WasteFixNote2 { get; set; }
    }

    public class UpdateBranchingRateIncreaseDTO : CreateBranchingRateIncreaseDTO
    {
        public int Id { get; set; }
    }

    public class BranchingRateIncreaseDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int WaterRateIncrease { get; set; }
        public int WasteRateIncrease { get; set; }
        public int WastePersentIncrease { get; set; }
        public int FixAmountBusiness { get; set; }
        public int CapacityFixAmount { get; set; }
        public int WaterInstallRateIncrease { get; set; }
        public int WsInstalIncrease { get; set; }
        public int WaterFixNote2 { get; set; }
        public int WasteFixNote2 { get; set; }
    }
}
