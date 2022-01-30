using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class CreateIncomeCurrentWsNHViewModel : BaseViewModel
    {
        public int YearId { get; set; }

        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int NumberUser { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int UnitUser { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public decimal AvgConsumeUser { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public decimal Capacity { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int ConsumptionUser { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Income { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int SubscriptionIncome { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int ExcessIncome { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int SeasonalIncome { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Note3Price { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Note3Income { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int TotalIncome { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Note7Price { get; set; }

        [Required(ErrorMessage = "")]
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Note7Income { get; set; }

        public IEnumerable<SelectListItem> UserTypeSource { get; set; }

        public string UserTypeTitle
        {
            get
            {
                if (UserTypeSource == null || !UserTypeSource.Any())
                    return string.Empty;

                return UserTypeSource.FirstOrDefault(x => x.Value.ToString() == UserTypeId.ToString()).Text;
            }
        }
    }
    public class IncomeCurrentWsNHViewModel
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

        public decimal Capacity { get; set; }

        public string CapacityDisplay => Capacity.ToString("N2");

        public int ConsumptionUser { get; set; }

        public string ConsumptionUserDisplay => ConsumptionUser.ToString("N0");
        
        public int Cost { get; set; }

        public string CostDisplay => Cost.ToString("N0");

        public int Income { get; set; }

        public string IncomeDisplay => Income.ToString("N0");

        public int SubscriptionIncome { get; set; }

        public string SubscriptionIncomeDisplay => SubscriptionIncome.ToString("N0");

        public int ExcessIncome { get; set; }

        public string ExcessIncomeDisplay => ExcessIncome.ToString("N0");

        public int SeasonalIncome { get; set; }

        public string SeasonalIncomeDisplay => SeasonalIncome.ToString("N0");

        public int Note3Price { get; set; }

        public string Note3PriceDisplay => Note3Price.ToString("N0");
       
        public int Note3Income { get; set; }

        public string Note3IncomeDisplay => Note3Income.ToString("N0");

        public int TotalIncome { get; set; }

        public string TotalIncomeDisplay => TotalIncome.ToString("N0");

        public int Note7Price { get; set; }

        public string Note7PriceDisplay => Note7Price.ToString("N0");

        public int Note7Income { get; set; }

        public string Note7IncomeDisplay => Note7Income.ToString("N0");
    }
}
