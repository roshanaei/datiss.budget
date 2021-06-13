using System.Collections.Generic;
using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.ViewModels.Identity
{
    public class TodayBirthDaysViewModel
    {
        public List<User> Users { set; get; }

        public AgeStatViewModel AgeStat { set; get; }
    }
}