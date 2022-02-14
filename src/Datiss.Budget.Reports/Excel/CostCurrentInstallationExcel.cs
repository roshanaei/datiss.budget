using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentInstallationExcel
    {
        private const string _sheetName = "CostCurrentInstallation";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentInstalationDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "بخش";
            sheet.Cell(1, 4).Value = "عنوان";
            sheet.Cell(1, 5).Value = "تعداد انشعاب طی دوره";
            sheet.Cell(1, 6).Value = "بهاء واحد";
            sheet.Cell(1, 7).Value = "جمع کل هزینه";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.CCInstalationTypeTitle;
                sheet.Cell(row, 4).Value = item.ActivityType;
                sheet.Cell(row, 5).Value = item.NumberUser;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.Cost;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.Income;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 6);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
