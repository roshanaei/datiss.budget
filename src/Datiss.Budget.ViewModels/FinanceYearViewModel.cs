using Datiss.Budget.Enum;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class CreateFinanceYearViewModel : BaseViewModel
    {
        public int Year { get; set; }
        //set required
        public string StartPersianDate { get; set; }
        public DateTime StartDate => (DateTime)StartPersianDate.ToGregorianDateTime();
        public string EndPrsianDate { get; set; }
        public DateTime EndDate => (DateTime)EndPrsianDate.ToGregorianDateTime();
    }

    public class UpdateFinanceYearViewModel : CreateFinanceYearViewModel
    {
        public int Id { get; set; }
        public bool Enable { get; set; }
    }
    public class FinanceYearViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public EntityStatus Status { get; set; }
        public string StatusDisplay => Status.ToDisplay();
    }
    public class FinanceYearFilterViewModel : FilterViewModel
    {

    }
    public class FinanceYearIndexViewModel : PagedViewModel<FinanceYearViewModel>
    {
        public FinanceYearIndexViewModel()
        {
            Filter = new FinanceYearFilterViewModel();
        }

        public string StartPersianDate { get; set; }
        public FinanceYearFilterViewModel Filter { get; set; }

    }
}
