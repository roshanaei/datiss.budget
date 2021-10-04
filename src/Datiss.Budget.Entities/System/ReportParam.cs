using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities
{
    public class ReportParam : IAuditableEntity
    {

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public ReportParamType ParamType { get; set; }

        public int ReportId { get; set; }

        #endregion

        #region Navigations

        public Report Report { get; set; }

        #endregion
    }
}
