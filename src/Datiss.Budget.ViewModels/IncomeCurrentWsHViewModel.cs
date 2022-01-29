using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeCurrentWsHViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        [Required(ErrorMessage = "*")]
        public int NumberUser { get; set; }

        [Required(ErrorMessage = "*")]
        public int UnitUser { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal AvgConsumeUser { get; set; }

        [Required(ErrorMessage = "*")]
        public int ConsumptionUser { get; set; }

        [Required(ErrorMessage = "*")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "*")]
        public int Income { get; set; }

        [Required(ErrorMessage = "*")]
        public int SubscriptionIncome { get; set; }

        [Required(ErrorMessage = "*")]
        public int Note3Price { get; set; }

        [Required(ErrorMessage = "*")]
        public int Note3Income { get; set; }

        [Required(ErrorMessage = "*")]
        public int SeasonalIncome { get; set; }

        [Required(ErrorMessage = "*")]
        public int TIncome { get; set; }

        [Required(ErrorMessage = "*")]
        public int Note7Price { get; set; }

        [Required(ErrorMessage = "*")]
        public int Note7Income { get; set; }
    }

    public class UpdateIncomeCurrentWsHViewModel : CreateIncomeCurrentWsHViewModel
    {
        public int Id { get; set; }
    }

    public class IncomeCurrentWsHViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeDisplay { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerDisplay { get; set; }

        public int NumberUser { get; set; }

        public string NumberUserDisplay => NumberUser.ToString("N0");

        public int UnitUser { get; set; }

        public string UnitUserDisplay => UnitUser.ToString("N0");

        public decimal AvgConsumeUser { get; set; }

        public string AvgConsumeUserDisplay => AvgConsumeUser.ToString("N2");

        public int ConsumptionUser { get; set; }

        public string ConsumptionUserDisplay => ConsumptionUser.ToString("N0");

        public int Cost { get; set; }

        public string CostDisplay => Cost.ToString("N0");

        public int Income { get; set; }

        public string IncomeDisplay => Income.ToString("N0");

        public int SubscriptionIncome { get; set; }

        public string SubscriptionIncomeDisplay => SubscriptionIncome.ToString("N0");

        public int Note3Price { get; set; }

        public string Note3PriceDisplay => Note3Price.ToString("N0");

        public int Note3Income { get; set; }

        public string Note3IncomeDisplay => Note3Income.ToString("N0");

        public int SeasonalIncome { get; set; }

        public string SeasonalIncomeDisplay => SeasonalIncome.ToString("N0");

        public int TIncome { get; set; }

        public string TIncomeDisplay => TIncome.ToString("N0");

        public int Note7Price { get; set; }

        public string Note7PriceDisplay => Note7Price.ToString("N0");

        public int Note7Income { get; set; }

        public string Note7IncomeDisplay => Note7Income.ToString("N0");
    }
    public class IncomeCurrentWsHFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class IncomeCurrentWsHIndexViewModel : PagedViewModel<IncomeCurrentWsHViewModel>
    {
        public IncomeCurrentWsHIndexViewModel()
        {
            Filter = new IncomeCurrentWsHFilterViewModel();
        }

        public IncomeCurrentWsHFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> UserTypeSource { get; set; }

        public IList<SelectListItem> UsageLayerTypeSource { get; set; }

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

        public void SetUsageLayerTypeSource(IEnumerable<DropDownItemViewModel> source)
            => UsageLayerTypeSource = source.Select(x => new SelectListItem
            {
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

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
    }

}
