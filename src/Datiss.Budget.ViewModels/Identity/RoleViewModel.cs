using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Datiss.Budget.ViewModels.Identity
{

    public class PrevRoleViewModel
    {
        [HiddenInput]
        public string Id { set; get; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام نقش")]
        public string Name { set; get; }
    }

    public class RolesIndexViewModel
    {

        public RolesIndexViewModel() {
            Items = new List<RoleViewModel>();
            ClaimTypeSource = new List<AppClaimTypeViewModel>();
        }

        public RolesIndexViewModel(IEnumerable<RoleViewModel> roles) {
            Items = roles;
            ClaimTypeSource = new List<AppClaimTypeViewModel>();
        }

        public RolesIndexViewModel(
            IEnumerable<RoleViewModel> roles, 
            IEnumerable<AppClaimTypeViewModel> claimTypes) {

            Items = roles;
            ClaimTypeSource = claimTypes;
        }

        public IEnumerable<RoleViewModel> Items { get; set; }

        public int TotalCount => Items.Count();

        public IEnumerable<AppClaimTypeViewModel> ClaimTypeSource { get; set; }
    }

    public class RoleViewModel {

        public RoleViewModel() 
        {
            Claims = new List<RoleClaimViewModel>();
        }

        public int Id { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public EntityStatus Status { get; set; }
        public bool Enabled => Status == EntityStatus.Enabled;
        public IEnumerable<RoleClaimViewModel> Claims { get; set; }
    }

    public class AppClaimTypeViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public EntityStatus Status { get; set; }
    }

    public class RoleClaimViewModel 
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }


    public class CreateRoleViewModel : BaseViewModel {

        public CreateRoleViewModel() {
            ClaimTypeSource = new List<AppClaimTypeViewModel>();
            SelectedClaims = new Dictionary<string, string>();
        }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(255,ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "Required")]
        [MaxLength(255, ErrorMessageResourceType = typeof(ViewModelString), ErrorMessageResourceName = "MaxLen")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<AppClaimTypeViewModel> ClaimTypeSource { get; set; }

        public Dictionary<string, string> SelectedClaims { get; set; }
    }

    public class UpdateRoleViewModel : CreateRoleViewModel {
        public int Id { get; set; }
    }

}