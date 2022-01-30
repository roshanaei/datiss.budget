using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface ITablesFieldTitleService
    {
        Task<TablesFiledTitle> GetByIdAsync(int id);
        Task<IEnumerable<DropDownItem>> GetByTableSectionNameAsync(TablesName tablesName, SectionName sectionName = SectionName.A);
    }
}
