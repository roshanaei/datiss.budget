using System.Collections.Generic;

namespace Datiss.Budget.ViewModels
{
    public class PagedViewModel<T> where T : class
    {
        public PagedViewModel() {
            Items = new List<T>();
        }

        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; }
        public int PreviousPage => PageNumber - 1;
        public int PageNumber { get; set; }
        public int NextPage => PageNumber + 1;
        public int TotalCount { get; set; }
        public int PagesCount { get; set; }
        public int PageLimit { get; set; } = 5;
        public int StartPage { get; set; } = 1;
        public int Lastpage => PagesCount;
        public int TotalLimit => PagesCount % PageLimit == 0
                               ? PagesCount / PageLimit
                               : (PagesCount / PageLimit) + 1;
        public int LimitNumber => PageNumber % PageLimit == 0
                               ? PageNumber / PageLimit
                               : (PageNumber / PageLimit) + 1;
    }
}
