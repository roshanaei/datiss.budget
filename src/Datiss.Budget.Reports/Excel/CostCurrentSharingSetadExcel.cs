using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentSharingSetadExcel
    {
        private const string _sheetName = "CostCurrentSharingSetad";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentSharingSetadDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 5).Value = "ضریب تسهیم درآمد جاری";
            sheet.Cell(1, 8).Value = "ضریب تسهیم درآمد سرمایه ای";
            sheet.Cell(1, 11).Value = "ضریب تسهیم درآمد جاری فاضلاب";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.IncomeCurrentWSharingCoff;
                sheet.Cell(row, 3).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 4).Value = item.IncomeCurrentWsSharingCoff;
                sheet.Cell(row, 4).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.IncomeForcastsharing;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 5);

            var table = range.CreateTable($"{_sheetName}_Table");

            table.Theme = XLTableTheme.TableStyleMedium16;

            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentSharingSetadDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 11).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "ضریب تسهیم هزینه اداری";
            sheet.Cell(2, 4).Value = "ضریب تسهیم هزینه فاضلاب";
            sheet.Cell(2, 5).Value = "ضریب تسهیم سایر هزینه ها";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                row++;
            }

            var range = sheet.Range(2, 1, row - 1, 11);

            range.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(3).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(4).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Column(5).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            sheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
