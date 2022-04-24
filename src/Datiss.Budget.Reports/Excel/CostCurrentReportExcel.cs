using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentReportExcel
    {
        private const string _sheetName = "CostCurrentReport";

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentReportDTO> items, int year)
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
            sheet.Cell(2, 3).Value = "مرکز هزینه";
            sheet.Cell(2, 4).Value = "کد مرکز هزینه";
            sheet.Cell(2, 5).Value = "شرح";
            sheet.Cell(2, 6).Value = "کد شرح";
            sheet.Cell(2, 7).Value = "واحد";
            sheet.Cell(2, 8).Value = "کد واحد";
            sheet.Cell(2, 9).Value = "عنوان";
            sheet.Cell(2, 10).Value = "کد عنوان";
            sheet.Cell(2, 11).Value = $"عملکرد سال{year - 2}";
            sheet.Cell(2, 12).Value = $"عملکرد سال {year - 1}";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 4).Value = item.CostCenterTypeId;
                sheet.Cell(row, 5).Value = item.SectionTypeDisplay;
                sheet.Cell(row, 6).Value = item.SectionTypeId;
                sheet.Cell(row, 7).Value = item.UnitTypeDisplay;
                sheet.Cell(row, 8).Value = item.UnitTypeId;
                sheet.Cell(row, 9).Value = item.UnitDetailTypeDisplay;
                sheet.Cell(row, 10).Value = item.UnitDetailTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 12);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(9).Style.NumberFormat.Format = "#,##0";
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = "#,##0";
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
