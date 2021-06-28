using System;
using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities.Identity
{
    public class AppLogItem : IAuditableEntity
    {
        public AppLogItem() { }

        #region properties
        public int Id { set; get; }

        public DateTime? CreatedDateTime { get; set; }

        public int EventId { get; set; }

        public string Url { get; set; }

        public string LogLevel { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string StateJson { get; set; }
        #endregion
    }
}