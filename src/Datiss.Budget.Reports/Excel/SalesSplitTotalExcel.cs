using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class SalesSplitTotalExcel
    {
        private const string _sheetName = "SalesSplitTotal";

        public static XLWorkbook ExportExcel(this IEnumerable<SalesSplitTotalDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "تعداد انشعاب آب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(1, 5).Value = "آحاد انشعاب آب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(1, 6).Value = "تعداد انشعاب فاضلاب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(1, 7).Value = "آحاد انشعاب فاضلاب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(1, 8).Value = "تعداد انشعاب آب شش ماهه دوم سال ماقبل";
            sheet.Cell(1, 9).Value = "آحاد انشعاب آب شش ماهه دوم سال ماقبل";
            sheet.Cell(1, 10).Value = "تعداد انشعاب فاضلاب شش ماهه دوم سال ماقبل";
            sheet.Cell(1, 11).Value = "آحاد انشعاب فاضلاب شش ماهه دوم سال ماقبل";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.WNumber;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.WUnit;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.WsNumber;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.WsUnit;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.WNumber_2;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.WUnit_2;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.WsNumber_2;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.WsUnit_2;
                sheet.Cell(row, 11).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 11);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
        public static XLWorkbook GetImportTemplate(this IEnumerable<SalesSplitTotalDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 12).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";
            sheet.Cell(2, 5).Value = "تعداد انشعاب آب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(2, 6).Value = "آحاد انشعاب آب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(2, 7).Value = "تعداد انشعاب فاضلاب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(2, 8).Value = "آحاد انشعاب فاضلاب پایان شش ماهه اول سال ماقبل";
            sheet.Cell(2, 9).Value = "تعداد انشعاب آب شش ماهه دوم سال ماقبل";
            sheet.Cell(2, 10).Value = "آحاد انشعاب آب شش ماهه دوم سال ماقبل";
            sheet.Cell(2, 11).Value = "تعداد انشعاب فاضلاب شش ماهه دوم سال ماقبل";
            sheet.Cell(2, 12).Value = "آحاد انشعاب فاضلاب شش ماهه دوم سال ماقبل";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.UserTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 12);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(6).Style.NumberFormat.Format = "#,##0";
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(7).Style.NumberFormat.Format = "#,##0";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(8).Style.NumberFormat.Format = "#,##0";
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(9).Style.NumberFormat.Format = "#,##0";
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = "#,##0";
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(11).Style.NumberFormat.Format = "#,##0";
            range.Column(11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(12).Style.NumberFormat.Format = "#,##0";
            range.Column(12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
