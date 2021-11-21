using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;
using Datiss.Budget.Resources;

namespace Datiss.Budget.ViewModels
{

    public static class EnumDisplayProvider {

        public static string ToDisplay(this EntityStatus status)
            => status switch {
                EntityStatus.Deleted => EnumText.EntityStatus_Deleted,
                EntityStatus.Disbaled => EnumText.EntityStatus_Disabled,
                EntityStatus.Enabled => EnumText.EntityStatus_Enabled,
                _=> EnumText.Unknown
            };


    }
}
