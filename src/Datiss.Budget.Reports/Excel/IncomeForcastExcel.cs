using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class IncomeForcastExcel
    {
        private const string _sheetName = "IncomeForcast";

        public static XLWorkbook ExportExcel(this IEnumerable<IncomeForcastDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "تعداد انشعاب";
            sheet.Cell(1, 5).Value = "آحاد انشعاب";
            sheet.Cell(1, 6).Value = "درآمد نصب انشعاب آب";
            sheet.Cell(1, 7).Value = "درآمد حق انشعاب آب";
            sheet.Cell(1, 8).Value = "درآمد تبصره 2 ماده واحده آب";
            sheet.Cell(1, 9).Value = "درآمد تبصره 3 ماده واحده آب";
            sheet.Cell(1, 10).Value = "درآمد ماده 11 آب";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.NumberUser;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.UnitUser;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.WaterInstllIncome;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.WaterBranchIncome;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.WaterNote2Income;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.WaterNote3Income;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.WNote11Income;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 10);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<IncomeForcastDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 4).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";


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

            var range = sheet.Range(2, 1, row - 1, 4);
            range.Column(4).Style.NumberFormat.Format = "#,##0";
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other

            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
