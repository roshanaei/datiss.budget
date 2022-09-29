using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{
    public static class TotalBudgetWReportExcel
    {
        private const string _sheetName = "TotalBudgetWReport";

        public static XLWorkbook ExportExcel(this IEnumerable<TotalBudgetWReportDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "شرح";
            sheet.Cell(1, 4).Value = "واحد";

            sheet.Cell(1, 5).Value = "پیش بینی سال بودجه";
            sheet.Cell(1, 6).Value = "عملکرد سال پایه";
            sheet.Cell(1, 7).Value = "عملکرد سال ماقبل";
            sheet.Cell(1, 8).Value = "مصوب سال ماقبل";
            sheet.Cell(1, 9).Value = "درصد رشد پیش بینی به عملکرد";
            sheet.Cell(1, 10).Value = "درصد رشد پیش بینی به بودجه";

            var totalcount = items.Count();

            int row = 2;
            for (int i = 0; i < totalcount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.SectionTypeDisplay;
                sheet.Cell(row, 4).Value = item.UnitTypeDisplay;
                sheet.Cell(row, 5).Value = item.ForcastY;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 6).Value = item.FunctionalBasicYear;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 7).Value = item.FunctionalYear_1;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 8).Value = item.ApproveYear_1;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 9).Value = item.ForcastFunctionalPercent;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 10).Value = item.ForcastBudgetPercent;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 10);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleLight16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<TotalBudgetWReportDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 6).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "شرح";
            sheet.Cell(2, 4).Value = "کد شرح";
            sheet.Cell(2, 5).Value = $"عملکرد سال{year - 2}";
            sheet.Cell(2, 6).Value = $"عملکرد سال {year - 1}";


            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.SectionTypeDisplay;
                sheet.Cell(row, 4).Value = item.SectionTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 6);
            range.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
