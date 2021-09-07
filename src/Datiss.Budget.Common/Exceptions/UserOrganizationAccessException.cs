namespace Datiss.Budget.Common.Exceptions
{
    public class UserOrganizationAccessException : BaseAppException
    {
        public UserOrganizationAccessException(int excelRowIndex)
            => ExcelRowIndex = excelRowIndex;

        public UserOrganizationAccessException(): base() { }

        public int ExcelRowIndex { get; private set; }
    }
}
