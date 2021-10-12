using Datiss.Budget.Entities.AuditableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Entities
{
    public class Report : IAuditableEntity
    {

        public Report() {
            Params = new HashSet<ReportParam>();
        }

        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public EntityStatus Status { get; set; }

        public byte[] FileData { get; set; }

        public string FilePath { get; set; }

        #endregion

        #region Navigations

        public ICollection<ReportParam> Params { get; set; }

        #endregion
    }
}
