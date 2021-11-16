using System.Collections.Generic;
using Datiss.Budget.Entities.AuditableEntity;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;


namespace Datiss.Budget.Entities.DWH
{
    public class TablesFiledTitle : IAuditableEntity
    {
        public TablesFiledTitle() 
        {
            Childrens = new HashSet<TablesFiledTitle>();
        }

        #region Properties
        public int Id { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public EntityStatus Status { get; set; }

        public TablesName TableName { get; set; }

        public SectionName SectionName { get; set; }

        public int DisplayOrder { get; set; }

        #endregion

        #region Navigations
        public TablesFiledTitle Parent { get; set; }

        public ICollection<TablesFiledTitle> Childrens { get; set; }

        public ICollection<PerformanceEvaluation> PerformanceEvaluation { get; set; }

        #endregion
    }
}