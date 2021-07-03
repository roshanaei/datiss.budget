using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class PagedResult<T> where T: class
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int TotalCount { get; set; }
        public int PagesCount => TotalCount / PageSize;
    }
}
