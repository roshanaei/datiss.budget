namespace Datiss.Budget.Common.Exceptions
{

    public class RequiredFieldException : BaseAppException
    {

        public RequiredFieldException(string fieldName) 
            : base(message : $"Required field : {fieldName} exception.") 
        {
            FieldName = fieldName;
        }

        public string FieldName { get; set; }
    }
}
