using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels.Tools
{
    public class JqueryDataTable
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public long RecordsTotal { get; set; }
        public long RecordsFiltered { get; set; }

        public List<JqueryDataTableColumn> Columns { get; set; }
        public JqueryDataTableSearch Search { get; set; }
        public List<JqueryDataTableOrder> Order { get; set; }

        public JqueryDataTable()
        {
            Columns = new List<JqueryDataTableColumn>();
            Search = new JqueryDataTableSearch();
            Order = new List<JqueryDataTableOrder>();
        }
    }

    public class JqueryDataTableColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public JqueryDataTableSearch Search { get; set; }

        public JqueryDataTableColumn()
        {
            Search = new JqueryDataTableSearch();
        }
    }

    public class JqueryDataTableSearch
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }

    public class JqueryDataTableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }
}
