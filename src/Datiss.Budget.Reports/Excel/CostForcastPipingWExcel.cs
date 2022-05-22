using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

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
            var range = sheet.Range(1, 1, row - 1, 14);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this CostForcastPipingWImportViewModel model, int year)
        {
            if (!model.Items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);
            sheet.RightToLeft = true;
            //
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "جنس لوله";
            sheet.Cell(1, 3).Value = "قطر لوله";
            sheet.Cell(1, 4).Value = "نوع کندمان";
            sheet.Cell(1, 5).Value = "هزینه هر متر خرید لوله";
            sheet.Cell(1, 6).Value = "هزینه هر متر اجرا";
            sheet.Cell(1, 7).Value = "جمع کل هزینه ها (هزار ریال)";


            sheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 11).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 12).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 13).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 14).Style.Fill.BackgroundColor = XLColor.Cream;
            int row = 2;
            foreach (var item in model.DigTypeSource)
            {
                sheet.Cell(row, 1).Value = item.Title;
                sheet.Cell(row, 1).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 2).Value = item.Id;
                sheet.Cell(row, 2).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.TubeTypeSource)
            {
                sheet.Cell(row, 3).Value = item.Title;
                sheet.Cell(row, 3).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 4).Value = item.Id;
                sheet.Cell(row, 4).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.DiameterPipeTypeSource)
            {
                sheet.Cell(row, 5).Value = item.Title;
                sheet.Cell(row, 5).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 6).Value = item.Id;
                sheet.Cell(row, 6).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            sheet.Range(1, 1, 8, 6);

            sheet.Cell(9, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            //sheet.Range(23, 1, 24, 15).Merge();

            sheet.Cell(10, 1).Value = "کد جنس لوله";
            sheet.Cell(10, 2).Value = "کد قطر لوله";
            sheet.Cell(10, 3).Value = "کد کندمان";
            sheet.Cell(10, 4).Value = "هزینه هر متر خرید لوله";
            sheet.Cell(10, 5).Value = "هزینه هر متر اجرا";
            sheet.Cell(10, 6).Value = "جمع کل هزینه ها (هزار ریال)";




            var totalCount = model.Items.Count();
            row = 11;
           
            var range = sheet.Range(20, 1, row - 1, 14);
            //range.Column(4).Style.NumberFormat.Format = "#,##0";
            //range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            //range.Column(5).Style.NumberFormat.Format = "#,##0";
            //range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
