using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{

    public static class CostForcastBuyExcel
    {
        private const string _sheetName = "CostForcastBuy";

        public static XLWorkbook ExportExcel(this IEnumerable<CostForcastBuyDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "توضیحات خرید دارائی";
            sheet.Cell(1, 4).Value = "سازمان مکان استقرار";
            sheet.Cell(1, 5).Value = "واحد سازمانی مربوطه";
            sheet.Cell(1, 6).Value = "مرکز هزینه";
            sheet.Cell(1, 7).Value = "عنوان دارائی";
            sheet.Cell(1, 8).Value = "شرح خرید دارئی";
            sheet.Cell(1, 9).Value = "تعداد/مقدار";
            sheet.Cell(1, 10).Value = "واحد اندازه گیری";
            sheet.Cell(1, 11).Value = "قیمت واحد(هزار ریال)";
            sheet.Cell(1, 12).Value = "محل تامین اعتبار";
            sheet.Cell(1, 13).Value = "هزینه پیشنهادی خرید دارایی در سال بودجه";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.BuyDescription;
                sheet.Cell(row, 4).Value = item.LocationDisplay;
                sheet.Cell(row, 5).Value = item.BuyDepartmentDisplay;
                sheet.Cell(row, 6).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 7).Value = item.AssetTypeDisplay;
                sheet.Cell(row, 8).Value = item.AssetDetailTypeDisplay;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 9).Value = item.Amount;
                sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.MeasurementTypeDisplay;
                sheet.Cell(row, 11).Value = item.UnitPrice;
                sheet.Cell(row, 11).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 12).Value = item.CreditTypeDisplay;
                sheet.Cell(row, 13).Value = item.ProposedCost;
                sheet.Cell(row, 13).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 13);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this CostForcastBuyImportViewModel model, int year)
        {
            if (!model.Items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);
            sheet.RightToLeft = true;
            //
            sheet.Cell(1, 3).Value = "سازمان مکان استقرار";
            sheet.Cell(1, 4).Value = "کد سازمان";
            sheet.Cell(1, 5).Value = "واحد سازمانی مربوطه";
            sheet.Cell(1, 6).Value = "کد واحد سازمانی";
            sheet.Cell(1, 7).Value = "مرکز هزینه";
            sheet.Cell(1, 8).Value = "کد مرکز هزینه";
            sheet.Cell(1, 9).Value = "محل تامین اعتبار";
            sheet.Cell(1, 10).Value = "کد محل تامین";
            sheet.Cell(1, 11).Value = "عنوان دارائی";
            sheet.Cell(1, 12).Value = "کد عنوان دارائی";
            sheet.Cell(1, 13).Value = "شرح خرید دارئی";
            sheet.Cell(1, 14).Value = "کد شرح خرید دارئی";
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
            foreach (var item in model.LocationTypeSource)
            {
                sheet.Cell(row, 3).Value = item.Title;
                sheet.Cell(row, 3).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 4).Value = item.Id;
                sheet.Cell(row, 4).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.BuyDepartmentTypeSource)
            {
                sheet.Cell(row, 5).Value = item.Title;
                sheet.Cell(row, 5).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 6).Value = item.Id;
                sheet.Cell(row, 6).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.CostCenterTypeSource)
            {
                sheet.Cell(row, 7).Value = item.Title;
                sheet.Cell(row, 7).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 8).Value = item.Id;
                sheet.Cell(row, 8).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }

            //row = 2;
            //foreach (var item in model.MeasurementTypeSource)
            //{
            //    sheet.Cell(row, 9).Value = item.Title;
            //    sheet.Cell(row, 9).Style.Fill.SetBackgroundColor(XLColor.White);
            //    sheet.Cell(row, 10).Value = item.Id;
            //    sheet.Cell(row, 10).Style.Fill.SetBackgroundColor(XLColor.White);
            //    row++;
            //}
            row = 2;
            foreach (var item in model.CreditTypeSource)
            {
                sheet.Cell(row, 9).Value = item.Title;
                sheet.Cell(row, 9).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 10).Value = item.Id;
                sheet.Cell(row, 10).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.AssetTypeSource)
            {
                sheet.Cell(row, 11).Value = item.Title;
                sheet.Cell(row, 11).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 12).Value = item.Id;
                sheet.Cell(row, 12).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.AssetDetailTypeSource)
            {
                sheet.Cell(row, 13).Value = item.Title;
                sheet.Cell(row, 13).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 14).Value = item.Id;
                sheet.Cell(row, 14).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            sheet.Range(1, 1, 24, 14);

            sheet.Cell(25, 1).Value = "ورود اطلاعات برای سال مالی : " + year;

            row = 26;

            sheet.Cell(row, 1).Value = "عنوان سازمان";
            sheet.Cell(row, 2).Value = "کد سازمان";
            sheet.Cell(row, 3).Value = "توضیحات خرید دارائی";
            sheet.Cell(row, 4).Value = "کد سازمان مکان استقرار";
            sheet.Cell(row, 5).Value = "کد واحد سازمانی مربوطه";
            sheet.Cell(row, 6).Value = "کد مرکز هزینه";
            sheet.Cell(row, 7).Value = "کد عنوان دارائی";
            sheet.Cell(row, 8).Value = "کد شرح خرید دارئی";
            sheet.Cell(row, 9).Value = "تعداد/مقدار";
            sheet.Cell(row, 10).Value = "کد محل تامین اعتبار";
            sheet.Cell(row, 11).Value = "هزینه پیشنهادی خرید دارایی در سال بودجه";

            var totalCount = model.Items.Count();
            row = 27;
            for (int i = 0; i < totalCount; i++)
            {
                var item = model.Items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(26, 1, row - 1, 11);

            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}