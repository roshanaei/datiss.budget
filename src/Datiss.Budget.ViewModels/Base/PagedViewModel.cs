using System.Collections.Generic;

namespace Datiss.Budget.ViewModels
{
    public class PagedViewModel<T> where T : class
    {
        public PagedViewModel() {
            Items = new List<T>();
        }

        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; } = 1;
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public int PagesCount => TotalCount % PageSize == 0
                               ? TotalCount / PageSize
                               : (TotalCount / PageSize) + 1;

        //public int PageLimit { get; set; }
        //public int StartPage { get; set; }
        //public int Lastpage { get; set; }
        //public int TotalLimit { get; set; }
        //public int LimitNumber { get; set; }
    }
}
