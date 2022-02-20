using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class PagerViewModel
    {
        public PagerViewModel(int startPage = 1, int pageLimit = 5)
        {
            StartPage = startPage;
            PageLimit = pageLimit;
        }

        public int PageSize { get; set; }
        public int PreviousPage => PageNumber - 1;
        public int PageNumber { get; set; }
        public int NextPage => PageNumber + 1;
        public int TotalCount { get; set; }
        public int PagesCount { get; set; }
        public int PageLimit { get; set; }
        public int StartPage { get; set; }
        public int LastPage => PagesCount;
        public int TotalLimit => PagesCount % PageLimit == 0
                               ? PagesCount / PageLimit
                               : (PagesCount / PageLimit) + 1;
        public int LimitNumber => PageNumber % PageLimit == 0
                               ? PageNumber / PageLimit
                               : (PageNumber / PageLimit) + 1;
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
