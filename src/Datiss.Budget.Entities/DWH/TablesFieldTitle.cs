using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Enum;


namespace Datiss.Budget.Entities.DWH
{
    public class TablesFiledTitle : IAuditableEntity
    {
        public TablesFiledTitle() { }

        #region Properties
        public int Id { get; set; }

        public string Title { get; set; }

        public EntityStatus Status { get; set; }

        public TablesName TableName { get; set; }

        public SectionName SectionName { get; set; }


        public int DisplayOrder { get; set; }

        #endregion

    }
}