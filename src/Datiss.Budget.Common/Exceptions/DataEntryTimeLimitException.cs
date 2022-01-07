namespace Datiss.Budget.Common.Exceptions
{
    public class DataEntryTimeLimitException : BaseAppException
    {

        public DataEntryTimeLimitException() : base() { }

        public DataEntryTimeLimitException(string message) : base(message) { }

        public DataEntryTimeLimitException(int? organizationId, int? yearId, string message) 
            : base(message) 
        {
            OrganizationId = organizationId;
            YearId = yearId;
        }

        public int? OrganizationId { get; set; }

        public int? YearId { get; set; }
    }
}
