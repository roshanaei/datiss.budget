namespace Datiss.Budget.ViewModels
{
    public abstract class FilterViewModel
    {
        /// <summary>
        /// Search phrase use to filter output result
        /// </summary>
        public string Search { get; set; }
        
        /// <summary>
        /// Determines which column should order by in query.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// If set to true, data must order by descending on database
        /// </summary>
        public bool OrderDesc { get; set; }

        /// <summary>
        /// Indices of clumns visible in DataTable comma-seperated
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// PageSize of DataTable, defauls is 10
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// PageNumber of data to fetch from database.
        /// </summary>
        public int PageNumber { get; set; } = 1;
    }

}
