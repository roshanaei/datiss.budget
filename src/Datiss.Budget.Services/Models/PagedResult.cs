using System.Collections.Generic;

namespace Datiss.Budget.Services.Models
{
    public class PagedResult<T> where T: class
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int TotalCount { get; set; }
        public int PagesCount => TotalCount % PageSize == 0
                               ? TotalCount / PageSize
                               : (TotalCount / PageSize) + 1;
        public int PageLimit { get; set; } = 5;


    }
}
