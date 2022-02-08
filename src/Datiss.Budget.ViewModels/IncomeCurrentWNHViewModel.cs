using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeCurrentWNHViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public int NumberUser { get; set; }

        public int UnitUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public int ConsumptionUser { get; set; }

        public decimal Capacity { get; set; }

        public long Cost { get; set; }

        public long Income { get; set; }

        public long ExcessIncome { get; set; }

        public long SeasonalIncome { get; set; }

        public long Note3Price { get; set; }

        public long Note3Income { get; set; }

        public long SubscriptionIncome { get; set; }

        public long TotalIncome { get; set; }

        public int Diff_ConsWsVolume { get; set; }

        public long Note2Income { get; set; }

        public int WasteVolume { get; set; }

        public IEnumerable<SelectListItem> UserTypeSource { get; set; }

    }

    public class UpdateIncomeCurrentWNHViewModel : CreateIncomeCurrentWNHViewModel
    {
        public int Id { get; set; }

    }

    public class IncomeCurrentWNHViewModel
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int NumberUser { get; set; }
        public string NumberUserDisplay => NumberUser.ToString("N0");
        public int UnitUser { get; set; }
        public string UnitUserDisplay => UnitUser.ToString("N0");
        public decimal AvgConsumeUser { get; set; }
        public string AvgConsumeUserDisplay => AvgConsumeUser.ToString("N2");
        public int ConsumptionUser { get; set; }
        public string ConsumptionUserDisplay => ConsumptionUser.ToString("N0");
        public decimal Capacity { get; set; }
        public string CapacityDisplay => Capacity.ToString("N2");
        public long Cost { get; set; }
        public string CostDisplay => Cost.ToString("N0");
        public long Income { get; set; }
        public string IncomeDisplay => Income.ToString("N0");
        public long ExcessIncome { get; set; }
        public string ExcessIncomeDisplay => ExcessIncome.ToString("N0");
        public long SeasonalIncome { get; set; }
        public string SeasonalIncomeDisplay => SeasonalIncome.ToString("N0");
        public long Note3Price { get; set; }
        public string Note3PriceDisplay => Note3Price.ToString("N0");
        public long Note3Income { get; set; }
        public string Note3IncomeDisplay => Note3Income.ToString("N0");
        public long SubscriptionIncome { get; set; }
        public string SubscriptionIncomeDisplay => SubscriptionIncome.ToString("N0");
        public long TotalIncome { get; set; }
        public string TotalIncomeDisplay => TotalIncome.ToString("N0");
        public int Diff_ConsWsVolume { get; set; }
        public string Diff_ConsWsVolumeDisplay => Diff_ConsWsVolume.ToString("N0");
        public long Note2Income { get; set; }
        public string Note2IncomeDisplay => Note2Income.ToString("N0");
        public int WasteVolume { get; set; }
        public string WasteVolumeDisplay => WasteVolume.ToString("N0");
    }

    public class IncomeCurrentWNHFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }
        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class IncomeCurrentWNHIndexViewModel : PagedViewModel<IncomeCurrentWNHViewModel>
    {

        public IncomeCurrentWNHIndexViewModel()
        {
            Filter = new IncomeCurrentWNHFilterViewModel();
        }

        public IncomeCurrentWNHFilterViewModel Filter { get; set; }
        public IList<SelectListItem> YearSource { get; set; }
        public IList<SelectListItem> OrganizationSource { get; set; }
        public IList<SelectListItem> InputOrganizationSource { get; set; }
        public IList<SelectListItem> UserTypeSource { get; set; }

        public void SetYearSource(IEnumerable<DropDownItemViewModel> source)
            => YearSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => OrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetInputOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => InputOrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetUserTypeSource(IEnumerable<DropDownItemViewModel> source)
            => UserTypeSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetFinanceYearFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedYearId = null)
            => Filter.YearSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedYearId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

    }
}
