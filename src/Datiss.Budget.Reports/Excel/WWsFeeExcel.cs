using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class WWsFeeExcel
    {
        private const string _sheetName = "WWsFee";

        public static XLWorkbook ExportExcel(this IEnumerable<WWsFeeDTO> items) 
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "حوزه فعالیت";
            sheet.Cell(1, 4).Value = "کاربری";
            sheet.Cell(1, 5).Value = "طبقه مصرف";
            sheet.Cell(1, 6).Value = "پارامتر اول تعرفه بهای خدمات ";
            sheet.Cell(1, 7).Value = "پارامتر دوم تعرفه بهای خدمات ";
            sheet.Cell(1, 8).Value = "پارامتر اول خدمات تبصره 3 ";
            sheet.Cell(1, 9).Value = "پارامتر دوم خدمات تبصره 3 ";
            sheet.Cell(1, 10).Value = "پارامتر اول خدمات تبصره 7 ";
            sheet.Cell(1, 11).Value = "پارامتر دوم خدمات تبصره 7 ";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.ActivityTypeDisplay;
                sheet.Cell(row, 3).DataType = XLDataType.Text;
                sheet.Cell(row, 4).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).DataType = XLDataType.Text;
                sheet.Cell(row, 5).Value = item.UsageLayerDisplay;
                sheet.Cell(row, 5).DataType = XLDataType.Text;
                sheet.Cell(row, 6).Value = item.P1Fee;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.P2Fee;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.P1Note3;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.P1Note7;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.P2Note3;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.P2Note7;
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

    }
}
