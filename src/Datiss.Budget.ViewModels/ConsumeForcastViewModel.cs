using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.ViewModels;
using System.Globalization;

namespace Datiss.Budget.ViewModels
{
    public class CreateConsumeForcastViewModel : BaseViewModel
    {
        public int YearId { get; set; }
        public string YearDisplay { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public int UsageLayerId { get; set; }

        public decimal CountUser { get; set; }

        public decimal UnitUser { get; set; }

        public decimal ConsumeUser { get; set; }

        public decimal AvgConsumeUser { get; set; }

        public IEnumerable<SelectListItem> UserTypeSource { get; set; }

        public string UserTypeTitle
        {
            get {
                if (UserTypeSource == null || !UserTypeSource.Any())
                    return string.Empty;

                return UserTypeSource.FirstOrDefault(x => x.Value.ToString() == UserTypeId.ToString()).Text;
            }
        }

        public IEnumerable<SelectListItem> UsageLayerSource { get; set; }

        public string UsageLayerTitle
        {
            get
            {
                if (UsageLayerSource == null || !UsageLayerSource.Any())
                    return string.Empty;

                return UsageLayerSource.FirstOrDefault(x => x.Value.ToString() == UsageLayerId.ToString()).Text;
            }
        }
    }

    public class UpdateConsumeForcastViewModel : CreateConsumeForcastViewModel
    {
        public int Id { get; set; }

        public decimal ConsumeUserForcast { get; set; }
    }

    public class ConsumeForcastViewModel
    {
        public int Id { get; set; }

        public int YearId { get; set; }

        public int Year { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationDisplay { get; set; }

        public int UserTypeId { get; set; }

        public string UserTypeTitle { get; set; }

        public int UsageLayerId { get; set; }

        public string UsageLayerTitle { get; set; }

        public decimal CountUser { get; set; }

        public string CountUserDisplay => CountUser.ToString("N2");

        public decimal UnitUser { get; set; }

        public string UnitUserDisplay => UnitUser.ToString("N2");

        public decimal ConsumeUser { get; set; }

        public string ConsumeUserDisplay => ConsumeUser.ToString("N2");

        public decimal AvgConsumeUser { get; set; }

        public string AvgConsumeUserDisplay => AvgConsumeUser.ToString("N2");

        public decimal ConsumeUserForcast { get; set; }

        public string ConsumeUserForcastDisplay => ConsumeUserForcast.ToString("N2");
    }

    public class ConsumeForcastFilterViewModel : FilterViewModel
    {
        public int? YearId { get; set; }

        public int? OrganizationId { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }
    }

    public class ConsumeForcastIndexViewModel : PagedViewModel<ConsumeForcastViewModel>
    {
        public ConsumeForcastIndexViewModel()
        {
            Filter = new ConsumeForcastFilterViewModel();
        }

        public ConsumeForcastFilterViewModel Filter { get; set; }

        public IList<SelectListItem> YearSource { get; set; }

        public IList<SelectListItem> OrganizationSource { get; set; }

        public IList<SelectListItem> InputOrganizationSource { get; set; }

        public IList<SelectListItem> UserTypeSource { get; set; }

        public string UserTypeSourceIdArray
        {
            get
            {
                string result = "";
                foreach(var item in UserTypeSource)
                {
                    result += $"{item.Value},";
                }
                return result.TrimEnd(',');
            }
        }

        public IList<SelectListItem> UsageLayerSource { get; set; }

        public IFormFile ExcelFile { get; set; }

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
            => UserTypeSource = source.Select(x => new SelectListItem {
                   Text = x.Title,
                   Value = x.Id.ToString()
               }).ToList();

        public void SetUsageLayerSource(IEnumerable<DropDownItemViewModel> source)
             => UsageLayerSource = source.Select(x => new SelectListItem
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


