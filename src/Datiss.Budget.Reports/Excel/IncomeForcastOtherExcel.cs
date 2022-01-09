using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class IncomeForcastOtherExcel
    {
        private const string _sheetName = "IncomeForcastOther";

        public static XLWorkbook ExportExcel(this IEnumerable<IncomeForcastOtherDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "عنوان";
            sheet.Cell(1, 4).Value = "فعالیت";
            sheet.Cell(1, 5).Value = "تعداد";
            sheet.Cell(1, 6).Value = "درآمد";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.OIFTypeDisplay;
                sheet.Cell(row, 4).Value = item.ActivityId;
                sheet.Cell(row, 5).Value = item.OIFCount;
                sheet.Cell(row, 6).Value = item.OIFPrice;
                sheet.Cell(row, 6).DataType = XLDataType.Number;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 6);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<IncomeForcastOtherDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 7).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان درآمدهای سرمایه ای";
            sheet.Cell(2, 4).Value = "کد درآمدهای سرمایه ای";
            sheet.Cell(2, 5).Value = "فعالیت";
            sheet.Cell(2, 6).Value = "تعداد";
            sheet.Cell(2, 7).Value = "درآمد";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.OIFTypeDisplay;
                sheet.Cell(row, 4).Value = item.OIFTypeId;
                sheet.Cell(row, 5).Value = item.ActivityId;
                sheet.Cell(row, 6).Value = item.OIFCount;
                sheet.Cell(row, 7).Value = item.OIFPrice;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 7);
            range.Column(6).Style.NumberFormat.Format = "#,##0";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
