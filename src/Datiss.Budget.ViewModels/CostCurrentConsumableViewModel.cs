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
    public class CreateCostCurrentConsumableViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int ConsumableTypeId { get; set; }

        public int ConsumableAmount { get; set; }

        public long ConsumableCost { get; set; }

        public IEnumerable<SelectListItem> ConsumableTypeSource { get; set; }

        public string ConsumableTypeDisplay
        {
            get
            {
                if (ConsumableTypeSource == null || !ConsumableTypeSource.Any())
                    return string.Empty;

                return ConsumableTypeSource.FirstOrDefault(x => x.Value.ToString() == ConsumableTypeId.ToString()).Text;
            }
        }

    }

    public class UpdateCostCurrentConsumableViewModel : CreateCostCurrentConsumableViewModel
    {
        public int Id { get; set; }
    }
    public class CostCurrentConsumableViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public ActivityType ActivityType { get; set; }

        public string ActivityTypeDisplay => ActivityType.ToDisplay();

        public int ConsumableTypeId { get; set; }

        public string ConsumableTypeDisplay { get; set; }

        public int ConsumableAmount { get; set; }

        public string ConsumableAmountDisaplay => ConsumableAmount.ToString("N0");

        public long ConsumableCost { get; set; }

        public string ConsumableCostDisaplay => ConsumableCost.ToString("N0");

    }

    public class CostCurrentConsumableFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }
        public ActivityType? ActivityType { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> ActivityTypeSource => EnumSelectListProvider.GetActivityTypeItems(ActivityType).ToList();
    }
}
