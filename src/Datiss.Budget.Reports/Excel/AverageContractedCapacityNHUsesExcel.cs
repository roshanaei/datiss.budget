using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class AverageContractedCapacityNHUsesExcel
    {
        private const string _sheetName = "AverageContractedCapacityNHUses";

        public static XLWorkbook ExportExcel(this IEnumerable<AverageContractedCapacityNHUsesDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "متوسط ظرفیت قراردادی آب";
            sheet.Cell(1, 5).Value = "متوسط ظرفیت قراردادی فاضلاب";
            sheet.Cell(1, 6).Value = "درآمد سرمایه ای متوسط ظرفیت قراردادی آب";
            sheet.Cell(1, 7).Value = "درآمد سرمایه ای متوسط ظرفیت قراردادی فاضلاب";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;

                sheet.Cell(row, 4).Value = item.AverageCapacity;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 4).Value = item.AverageCapacityWs;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 5).Value = item.AverageCapacityIncome;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 6).Value = item.AverageCapacityWsIncome;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 15);

            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
