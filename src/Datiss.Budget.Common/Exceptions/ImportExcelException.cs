namespace Datiss.Budget.Common.Exceptions
{
    public class ImportExcelFileFormatInvalidException : BaseAppException
    { }

    public class ImportExcelFileSizeInvalidException : BaseAppException
    { }

    public class ImportExcelFileException : BaseAppException
    {
        public int ExcelRowIndex { get; private set; }

        public ImportExcelFileException(): base() { }

        public ImportExcelFileException(int excelRowIndex)
            => ExcelRowIndex = excelRowIndex;
    }
}
