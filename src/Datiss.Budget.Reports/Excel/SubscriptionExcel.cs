using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class SubscriptionExcel
    {
        private const string _sheetName = "Subscription";

        public static XLWorkbook ExportExcel(this IEnumerable<SubscriptionDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "کاربری";
            sheet.Cell(1, 3).Value = "آبونمان آب";
            sheet.Cell(1, 4).Value = "آبونمان فاضلاب";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.UserTypeDisplay;
                sheet.Cell(row, 2).DataType = XLDataType.Text;
                sheet.Cell(row, 3).Value = item.SubW;
                sheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 4).Value = item.SubWs;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 4);

            var table = range.CreateTable($"{_sheetName}_Table");
            
            table.Theme = XLTableTheme.TableStyleMedium16;
            
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<SubscriptionDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 4).Merge();

            sheet.Cell(2, 1).Value = "عنوان کاربری";
            sheet.Cell(2, 2).Value = "کد کاربری";
            sheet.Cell(2, 3).Value = "آبونمان آب";
            sheet.Cell(2, 4).Value = "آبونمان فاضلاب";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.UserTypeDisplay;
                sheet.Cell(row, 2).Value = item.UserTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 4);
            range.Column(3).Style.NumberFormat.Format = "#,##0";
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(4).Style.NumberFormat.Format = "#,##0";
            range.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
