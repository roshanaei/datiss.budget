using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Enum;
using Datiss.Budget.Extensions;
using DNTPersianUtils.Core;

namespace Datiss.Budget.ViewModels.Identity
{

    public class UsersIndexViewModel : PagedViewModel<UserViewModel>
    {

        public UsersIndexViewModel() {
            Items = new List<UserViewModel>();
            Filter = new UserFilterViewModel();
        }

        public UserFilterViewModel Filter { get; set; }

        public void SetOrganizationFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrgId = null)
            => Filter.OrganizationSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedOrgId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetPositionFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectedPositionId = null)
            => Filter.PositionSource = source.Select(x => new SelectListItem
            {
                Selected = x.Id == selectedPositionId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? PositionId { get; set; }
        public string PositionTitle { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatedDateTimeDisplay => CreatedDateTime?.ToPersianDateTextify();
        public DateTime? LastVisitDateTime { get; set; }
        public string LastVisitDateTimeDisplay => LastVisitDateTime?.ToPersianDateTextify();
        public string NationalCode { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationTitle { get; set; }
        public EntityStatus Status { get; set; }
        public string StatusDisplay => Status.ToDisplay();
    }


    public class CreateUserViewModel : BaseViewModel
    {

    }

    public class UpdateUserViewModel
    {

    }

    public class UserFilterViewModel : FilterViewModel
    {
        public string Username { get; set; }
        public string NationalCode { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public int? OrganizationId { get; set; }
        public int? PositionId { get; set; }
        public EntityStatus? Status { get; set; }

        public IEnumerable<SelectListItem> StatusSource
            => EnumSelectListProvider.GetEntityStatusItems(Status);

        public IEnumerable<SelectListItem> OrganizationSource { get; set; }

        public IEnumerable<SelectListItem> PositionSource { get; set; }
    }
}
