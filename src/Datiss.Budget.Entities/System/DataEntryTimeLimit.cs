using System;
using Datiss.Budget.Entities.AuditableEntity;

namespace Datiss.Budget.Entities 
{

    public class DataEntryTimeLimit : IAuditableEntity 
    {

        public DataEntryTimeLimit() 
        { 
        
        }

        #region Properties

        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public int? YearId { get; set; }

        public int? RoleId { get; set; }

        public int? UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string Description { get; set; }

        #endregion

        #region Navigations

        public Organization Organization { get; set; }

        public FinanceYear Year { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }

        #endregion
    }
}
