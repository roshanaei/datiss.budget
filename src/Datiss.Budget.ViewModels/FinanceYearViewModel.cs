using Datiss.Budget.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.ViewModels
{
    public class AddFinanceYearViewModel
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Year { get; set; }

        public Entitystatus Status { get; set; }
    }

    public class UpdateFinanceYearViewModel : AddFinanceYearViewModel
    {
        public int Id { get; set; }
    }
}
