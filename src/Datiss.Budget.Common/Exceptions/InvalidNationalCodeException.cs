namespace Datiss.Budget.Common.Exceptions
{
    public class InvalidNationalCodeException : BaseAppException
    {

        public InvalidNationalCodeException(string nationalCode)
            : base($"NationalCode format is invalid : {nationalCode}.") 
        {
            NationalCode = nationalCode;
        }

        public string NationalCode { get; set; }
    }
}
