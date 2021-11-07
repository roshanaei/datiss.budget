namespace Datiss.Budget.ViewModels
{
    public abstract class FilterViewModel
    {
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }


}
