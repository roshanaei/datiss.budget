using Datiss.Budget.Enum;
using System;

namespace Datiss.Budget.Services.Models
{
    public class CreateFinanceYearDTO
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Year { get; set; }

        public bool Enabled { get; set; }
    }

    public class UpdateFinanceYearDTO : CreateFinanceYearDTO
    { 
        public int Id { get; set; }
    }
    public class FinanceYearDTO : FilterInputDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Year { get; set; }
        public EntityStatus Status { get; set;}
    }
}
