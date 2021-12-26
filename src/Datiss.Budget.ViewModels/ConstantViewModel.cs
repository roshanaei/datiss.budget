using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Enum;


namespace Datiss.Budget.ViewModels
{
    public class CreateConstantViewModel
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<SelectListItem> ParentList { get; set; }
    }

    public class UpdateConstantViewModel: CreateConstantViewModel
    {
        public int Id { get; set; }
    }


    public class ConstantViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string ConstantKey { get; set; }

        public int DisplayOrder { get; set; }

        public EntityStatus Status { get; set; }
    }


    public class ConstatntFilterViewModel: FilterViewModel
    {
        public int? ParentId { get; set; }

        public string ConstantKey { get; set; }

        public IList<SelectListItem> ParentSource { get; set; }
    }

    public class ConstantIndexViewModel : PagedViewModel<ConstantViewModel>
    {
        public ConstantIndexViewModel(){
            Filter = new ConstatntFilterViewModel();
            Items = new List<ConstantViewModel>();
        }

        public ConstatntFilterViewModel Filter { get; set; }


        public IList<SelectListItem> ParentSource { get; set; }

        public void SetParentSource(IEnumerable<DropDownItemViewModel> source)
            => ParentSource = source.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();

        public void SetParentFilterSource(IEnumerable<DropDownItemViewModel> source, int? selectParentId = null)
            => Filter.ParentSource = source.Select(x => new SelectListItem {
                Selected = x.Id == selectParentId,
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList();
    }
}
