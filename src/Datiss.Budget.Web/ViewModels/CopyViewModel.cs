using System;
using System.Linq;
using System.Collections.Generic;
using Datiss.Budget.Services.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Web.ViewModels {

    public class CopyViewModel : BaseViewModel {

        public int SourceYearId { get; set; }

        public int SourceOrgId { get; set; }

        public int TargetYearId { get; set; }

        public IEnumerable<SelectListItem> YearSource { get; protected set; }

        public IEnumerable<SelectListItem> OrganizationSource { get; protected set; }

        public void SetYearSource(IEnumerable<DropDownItem> source)
            => YearSource = source.Select(_ => new SelectListItem {
                Text = _.Title,
                Value = _.Id.ToString(),
                Selected = _.Selected
            });

        public void SetOrganizationSource(IEnumerable<DropDownItem> source)
            => OrganizationSource = source.Select(_ => new SelectListItem {
                Text = _.Title,
                Value = _.Id.ToString(),
                Selected = _.Selected
            });
    }
}
