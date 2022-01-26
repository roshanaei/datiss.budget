using ClosedXML.Excel;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class CofficientExcel
    {
        private const string _sheetName = "Cofficient";

        public static XLWorkbook ExportExcel(this IEnumerable<CofficientDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "عنوان ضریب";
            sheet.Cell(1, 4).Value = "ضریب";


            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.CofficientTypeTitle;
                sheet.Cell(row, 4).Value = item.Fee;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 4);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
        public static XLWorkbook GetImportTemplate(this IEnumerable<CofficientDTO> items, int year, CofficientsGroup group)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year + "و ضرایب افزایش " + group.ToDisplay();
            sheet.Range(1, 1, 1, 5).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان ضریب";
            sheet.Cell(2, 4).Value = "کد ضریب";
            sheet.Cell(2, 5).Value = "ضریب";


            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.CofficientTypeTitle;
                sheet.Cell(row, 4).Value = item.CofficientTypeId;
                sheet.Cell(row, 5).Value = item.Fee;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 5);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(5).Style.NumberFormat.Format = "#,##0.00";
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
