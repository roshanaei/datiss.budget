using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentOtherCofficientExcel
    {
        private const string _sheetName = "CostCurrentOtherCofficient";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentOtherCofficientDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "مرکز هزینه";
            sheet.Cell(1, 3).Value = "عنوان سایر هزینه";
            sheet.Cell(1, 4).Value = "ضریب";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 3).Value = item.CCOtherCostsTypeDisplay;
                sheet.Cell(row, 4).Value = item.Fee;
                sheet.Cell(row, 4).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 4);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentOtherCofficientDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 5).Merge();


            sheet.Cell(2, 1).Value = "عنوان مرکز هزینه";
            sheet.Cell(2, 2).Value = "کد مرکز هزینه";
            sheet.Cell(2, 3).Value = "عنوان سایر هزینه";
            sheet.Cell(2, 4).Value = "کد سایر هزینه";
            sheet.Cell(2, 5).Value = "ضریب";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 2).Value = item.CostCenterTypeId;
                sheet.Cell(row, 3).Value = item.CCOtherCostsTypeDisplay;
                sheet.Cell(row, 4).Value = item.CCOtherCostsTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 5);
            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
 
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
