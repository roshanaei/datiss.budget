using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class IncomeForcastWsExcel
    {
        private const string _sheetName = "IncomeForcastWs";

        public static XLWorkbook ExportExcel(this IEnumerable<IncomeForcastWsDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "نوع کاربری";
            sheet.Cell(1, 4).Value = "تعداد انشعاب";
            sheet.Cell(1, 5).Value = "آحاد انشعاب";
            sheet.Cell(1, 6).Value = "درآمد هزینه لوله گذاری فاضلاب";
            sheet.Cell(1, 7).Value = "درآمد حق انشعاب فاضلاب";
            sheet.Cell(1, 8).Value = "درآمد تبصره 3 ماده واحده فاضلاب";
            sheet.Cell(1, 9).Value = "درآمد ماده 11 فاضلاب";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.NumberUser;
                sheet.Cell(row, 5).Value = item.UnitUser;
                sheet.Cell(row, 6).Value = item.WasteInstallIncome;
                sheet.Cell(row, 7).Value = item.WasteBranchIncome;
                sheet.Cell(row, 8).Value = item.WasteNote3Income;
                sheet.Cell(row, 9).Value = item.WsNote11Income;
                sheet.Cell(row, 9).DataType = XLDataType.Number;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 9);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<IncomeForcastWsDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 10).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";
            sheet.Cell(2, 5).Value = "تعداد انشعاب";
            sheet.Cell(2, 6).Value = "آحاد انشعاب";
            sheet.Cell(2, 7).Value = "درآمد هزینه لوله گذاری فاضلاب";
            sheet.Cell(2, 8).Value = "درآمد حق انشعاب فاضلاب";
            sheet.Cell(2, 9).Value = "درآمد تبصره 3 ماده واحده فاضلاب";
            sheet.Cell(2, 10).Value = "درآمد ماده 11 فاضلاب";

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

            var range = sheet.Range(2, 1, row - 1, 10);
            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
