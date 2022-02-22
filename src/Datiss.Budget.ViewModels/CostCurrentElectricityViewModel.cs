using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateCostCurrentElectricityViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public int ElectricityAmount { get; set; }

        public long ElectricityCost { get; set; }

        public IEnumerable<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType);
    }

    public class UpdateCostCurrentElectricityViewModel : CreateCostCurrentElectricityViewModel
    {
        public int Id { get; set; }
    }
    public class CostCurrentElectricityViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int ElectricityAmount { get; set; }

        public string ElectricityAmountDisplay => ElectricityAmount.ToString("N0");

        public long ElectricityCost { get; set; }

        public string ElectricityCostDisplay => ElectricityCost.ToString("N0");
    }

    public class CostCurrentElectricityFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType).ToList();
    }

}
