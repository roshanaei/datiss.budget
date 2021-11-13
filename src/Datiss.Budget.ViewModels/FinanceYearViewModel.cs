using System;

namespace Datiss.Budget.ViewModels
{
    public class CreateFinanceYearViewModel
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Year { get; set; }

        public bool Enabled { get; set; }
    }

    public class UpdateFinanceYearViewModel : CreateFinanceYearViewModel
    {
        public int Id { get; set; }
    }
}
