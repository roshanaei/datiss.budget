using Datiss.Budget.Enum;
using Datiss.Budget.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class AddFinanceYearViewModel : BaseViewModel
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Year { get; set; }

        public bool Enabled { get; set; }
    }

    public class UpdateFinanceYearViewModel : AddFinanceYearViewModel
    {
        public int Id { get; set; }
    }

    public class FinanceYearViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Year { get; set; }

        public EntityStatus Status { get; set; }
    }
}
