using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;
using Datiss.Budget.Extensions;
using DNTPersianUtils.Core;
using Datiss.Budget.ViewModels;

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
        public string UserName { get; set; }
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

        public CreateUserViewModel() {
            PositionSource = new List<SelectListItem>();
            OrganizationSource = new List<SelectListItem>();
        }

        public CreateUserViewModel(
            IEnumerable<DropDownItemViewModel> positions, 
            IEnumerable<DropDownItemViewModel> organizations) {
            SetPositionSource(positions);
            SetOrganizationSource(organizations);
        }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(450, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(450, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string LastName { get; set; }

        [MaxLength(256, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(30, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(30, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "PasswordDoesNotMatch")]
        public string RetypePassword { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string PhoneNumber { get; set; }

        public int? PositionId { get; set; }
        public IEnumerable<SelectListItem> PositionSource { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(10, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string NationalCode { get; set; }

        public int? OrganizationId { get; set; }
        public IEnumerable<SelectListItem> OrganizationSource { get; set; }

        public void SetPositionSource(IEnumerable<DropDownItemViewModel> source)
            => PositionSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();

        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source)
            => OrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList().AddEmptySelectListItem();

    }

    public class UpdateUserViewModel : BaseViewModel
    {

        public UpdateUserViewModel() {
            PositionSource = new List<SelectListItem>();
            OrganizationSource = new List<SelectListItem>();
        }

        public UpdateUserViewModel(
            IEnumerable<DropDownItemViewModel> positions,
            IEnumerable<DropDownItemViewModel> organizations){
            SetOrganizationSource(organizations);
            SetPositionSource(positions);
        }

        public int Id { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(256, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(450, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(450, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string LastName { get; set; }

        [MaxLength(256, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Email { get; set; }

        [MaxLength(30, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string PhoneNumber { get; set; }

        public int? PositionId { get; set; }
        public IEnumerable<SelectListItem> PositionSource { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(10, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string NationalCode { get; set; }

        public int? OrganizationId { get; set; }
        public IEnumerable<SelectListItem> OrganizationSource { get; set; }

        public void SetPositionSource(IEnumerable<DropDownItemViewModel> source, int? selectedPositionId = null)
            => PositionSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString(),
                Selected = x.Id == selectedPositionId
            }).ToList();

        public void SetOrganizationSource(IEnumerable<DropDownItemViewModel> source, int? selectedOrganizationId = null)
            => OrganizationSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString(),
                Selected = x.Id == selectedOrganizationId
            }).ToList();

    }

    public class UserFilterViewModel : FilterViewModel
    {
        public string UserName { get; set; }
        public string NationalCode { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public int? OrganizationId { get; set; }
        public int? PositionId { get; set; }
        public EntityStatus? Status { get; set; }

        public IEnumerable<SelectListItem> StatusSource
            => EnumSelectListProvider.GetEntityStatusItems(Status)
                .ToList()
                .AddEmptySelectListItem();

        public IEnumerable<SelectListItem> OrganizationSource { get; set; }

        public IEnumerable<SelectListItem> PositionSource { get; set; }

    }
}
