using System.Collections.Generic;

namespace Datiss.Budget.ViewModels
{
    public class PagedViewModel<T> where T : class
    {
        public PagedViewModel() {
            Items = new List<T>();
        }

        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; } = 10;
        public int PreviousPage => PageNumber - 1;
        public int PageNumber { get; set; } = 1;
        public int NextPage => PageNumber + 1;
        public int TotalCount { get; set; }
        public int PagesCount { get; set; }
    }
}
