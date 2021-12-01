using Datiss.Budget.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.ViewModels
{
    public class CreateFinanceYearViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }
        public EntityStatus Status { get; set; }
        public string StatusDisplay => Status.ToDisplay();
        public IList<SelectListItem> OrganizationStatusSource { get; set; }
        public void SetOrganizationStatusFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectOrgStatusId = null)
        {
            OrganizationStatusSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectOrgStatusId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }
    }

    public class UpdateFinanceYearViewModel : CreateFinanceYearViewModel
    {
        public int Id { get; set; }
    }
    public class FinanceYearViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }
        public EntityStatus Status { get; set; }
        public string StatusDisplay => Status.ToDisplay();
    }
    public class FinanceYearFilterViewModel : FilterViewModel
    {
        public EntityStatus? Status { get; set; }
        public IList<SelectListItem> OrganizationStatusSource { get; set; }
    }
    public class FinanceYearIndexViewModel : PagedViewModel<FinanceYearViewModel>
    {
        public FinanceYearIndexViewModel()
        {
            Filter = new FinanceYearFilterViewModel();
        }

        public FinanceYearFilterViewModel Filter { get; set; }

        public void SetOrganizationStatusFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectOrgStatusId = null)
        {
            Filter.OrganizationStatusSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectOrgStatusId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();
        }

    }
}
