using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{

    public static class CostForcastBuyDescriptionExcel
    {
        private const string _sheetName = "CostForcastBuyDescription";

        public static XLWorkbook ExportExcel(this IEnumerable<CostForcastBuyDescriptionDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "عنوان دارائی";
            sheet.Cell(1, 3).Value = "شرح خرید دارئی";
            sheet.Cell(1, 4).Value = "واحد اندازه گیری";
            sheet.Cell(1, 5).Value = "قیمت واحد(هزار ریال)";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.AssetTypeDisplay;
                sheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                sheet.Cell(row, 3).Value = item.AssetDetailTypeDisplay;
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 4).Value = item.MeasurementTypeDisplay;
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                sheet.Cell(row, 5).Value = item.UnitPrice;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 5);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this CostForcastBuyDescriptionImportViewModel model, int year)
        {
            if (!model.Items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);
            sheet.RightToLeft = true;
            //
            sheet.Cell(1, 9).Value = "واحد اندازه گیری";
            sheet.Cell(1, 10).Value = "کد واحد اندازه گیری";

            sheet.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.Cream;
            
            int row = 2;
           
            foreach (var item in model.MeasurementTypeSource)
            {
                sheet.Cell(row, 9).Value = item.Title;
                sheet.Cell(row, 9).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 10).Value = item.Id;
                sheet.Cell(row, 10).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
           
            sheet.Range(1, 1, 1, 16);

            sheet.Cell(25, 1).Value = "ورود اطلاعات برای سال مالی : " + year;

            row = 1;

            sheet.Cell(row, 1).Value = "کد عنوان دارائی";
            sheet.Cell(row, 2).Value = "کد شرح خرید دارئی";
            sheet.Cell(row, 3).Value = "کد واحد اندازه گیری";
            sheet.Cell(row, 4).Value = "قیمت واحد(هزار ریال)";

            var totalCount = model.Items.Count();
            row = 1;


            var range = sheet.Range(1, 1, row - 1, 6);

            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}