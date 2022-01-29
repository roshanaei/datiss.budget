namespace Datiss.Budget.Common.Exceptions
{
    public class UserNationalCodeAlreadyExistException : BaseAppException
    {

        public UserNationalCodeAlreadyExistException(string nationalCode)
            : base(message: $"User with NationalCode : '{nationalCode}' already existed.") 
        {
            NationalCode = nationalCode;
        }

        public string NationalCode { get; set; }

    }
}
