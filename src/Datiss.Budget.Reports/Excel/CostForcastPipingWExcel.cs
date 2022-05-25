using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Reports.Excel
{

    public static class CostForcastPipingWExcel
    {
        private const string _sheetName = "CostForcastPipingW";

        public static XLWorkbook ExportExcel(this IEnumerable<CostForcastPipingWDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "جنس لوله";
            sheet.Cell(1, 3).Value = "قطر لوله";
            sheet.Cell(1, 4).Value = "نوع کندمان";
            sheet.Cell(1, 5).Value = "هزینه هر متر خرید لوله";
            sheet.Cell(1, 6).Value = "هزینه هر متر اجرا";
            sheet.Cell(1, 7).Value = "جمع کل هزینه ها (هزار ریال)";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.TubeTypeDisplay;
                sheet.Cell(row, 3).Value = item.DiameterPipeTypeDisplay;
                sheet.Cell(row, 4).Value = item.DigTypeDisplay;
                sheet.Cell(row, 5).Value = item.TubeBuyCost;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.RunCost;
                sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.TotalCost;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 7);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostForcastPipingWDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);


            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 8).Merge();
            //
            sheet.Cell(2, 1).Value = "جنس لوله";
            sheet.Cell(2, 2).Value = "کد جنس لوله";

            sheet.Cell(2, 3).Value = "قطر لوله";
            sheet.Cell(2, 4).Value = "کد قطر لوله";

            sheet.Cell(2, 5).Value = "نوع کندمان";
            sheet.Cell(2, 6).Value = "کد نوع کندمان";


            sheet.Cell(2, 7).Value = "هزینه هر متر خرید لوله";
            sheet.Cell(2, 8).Value = "هزینه هر متر اجرا";


            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.TubeTypeDisplay;
                sheet.Cell(row, 2).Value = item.TubeTypeId;
                sheet.Cell(row, 3).Value = item.DiameterPipeTypeDisplay;
                sheet.Cell(row, 4).Value = item.DiameterPipeTypeId;
                sheet.Cell(row, 5).Value = item.DigTypeDisplay;
                sheet.Cell(row, 6).Value = item.DigTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 8);
            range.Column(7).Style.NumberFormat.Format = "#,##0";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(8).Style.NumberFormat.Format = "#,##0";
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
