using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{

    public static class CostForcastTransferWExcel
    {
        private const string _sheetName = "CostForcastTransferW";

        public static XLWorkbook ExportExcel(this IEnumerable<CostForcastTransferWDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "اصلاح/توسعه شبکه توزیع/خط انتقال";
            sheet.Cell(1, 4).Value = "آدرس";
            sheet.Cell(1, 5).Value = "محل تامین اعتبار";
            sheet.Cell(1, 6).Value = "نوع کندمان";
            sheet.Cell(1, 7).Value = "جنس لوله";
            sheet.Cell(1, 8).Value = "قطر لوله";
            sheet.Cell(1, 9).Value = "طول اجرا";
            sheet.Cell(1, 10).Value = "هزینه هر متر خرید لوله";
            sheet.Cell(1, 11).Value = "هزینه هر متر اجرا";
            sheet.Cell(1, 12).Value = "جمع کل هزینه ها (هزار ریال)";
            sheet.Cell(1, 13).Value = "این پروژه در سال بودجه به بهره برداری رسیده؟";
            sheet.Cell(1, 14).Value = "سرفصل کلی در بودجه پیشنهادی";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.TransferTypeDisplay;
                sheet.Cell(row, 4).Value = item.Location;
                sheet.Cell(row, 5).Value = item.CreditTypeDisplay;
                sheet.Cell(row, 6).Value = item.DigTypeDisplay;
                sheet.Cell(row, 7).Value = item.DigTypeDisplay;
                sheet.Cell(row, 8).Value = item.DiameterPipeTypeDisplay;
                sheet.Cell(row, 9).Value = item.Lenth;
                sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.PipeCost;
                sheet.Cell(row, 10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.RunCost;
                sheet.Cell(row, 11).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 12).Value = item.TotalCost;
                sheet.Cell(row, 12).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 13).Value = item.ExtensionTypeDisplay;
                sheet.Cell(row, 14).Value = item.SuggestedBudgetTopicTypeDisplay;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 14);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this CostForcastTransferWImportViewModel model, int year)
        {
            if (!model.Items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);
            sheet.RightToLeft = true;
            //
            sheet.Cell(1, 1).Value = "اصلاح/توسعه شبکه توزیع/خط انتقال";
            sheet.Cell(1, 2).Value = "کد اصلاح ...";
            sheet.Cell(1, 3).Value = "محل تامین اعتبار";
            sheet.Cell(1, 4).Value = "کد محل تامین اعتبار";
            sheet.Cell(1, 5).Value = "نوع کندمان";
            sheet.Cell(1, 6).Value = "کد کندمان";
            sheet.Cell(1, 7).Value = "جنس لوله";
            sheet.Cell(1, 8).Value = "کد جنس لوله";
            sheet.Cell(1, 9).Value = "قطر لوله";
            sheet.Cell(1, 10).Value = "کد قطر لوله";
            sheet.Cell(1, 11).Value = "این پروژه در سال بودجه به بهره برداری رسیده؟";
            sheet.Cell(1, 12).Value = "کد بهره برداری";
            sheet.Cell(1, 13).Value = "سر فصل کلی در بودجه ";
            sheet.Cell(1, 14).Value = "کد سر فصل کلی در بودجه ";
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
            foreach (var item in model.TransferTypeSource)
            {
                sheet.Cell(row, 1).Value = item.Title;
                sheet.Cell(row, 1).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 2).Value = item.Id;
                sheet.Cell(row, 2).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.CreditTypeSource)
            {
                sheet.Cell(row, 3).Value = item.Title;
                sheet.Cell(row, 3).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 4).Value = item.Id;
                sheet.Cell(row, 4).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.DigTypeSource)
            {
                sheet.Cell(row, 5).Value = item.Title;
                sheet.Cell(row, 5).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 6).Value = item.Id;
                sheet.Cell(row, 6).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.DigTypeSource)
            {
                sheet.Cell(row, 7).Value = item.Title;
                sheet.Cell(row, 7).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 8).Value = item.Id;
                sheet.Cell(row, 8).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.DiameterPipeTypeSource)
            {
                sheet.Cell(row, 9).Value = item.Title;
                sheet.Cell(row, 9).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 10).Value = item.Id;
                sheet.Cell(row, 10).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.ExtensionTypeSource)
            {
                sheet.Cell(row, 11).Value = item.Title;
                sheet.Cell(row, 11).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 12).Value = item.Id;
                sheet.Cell(row, 12).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.SuggestedBudgetTopicTypeSource)
            {
                sheet.Cell(row, 13).Value = item.Title;
                sheet.Cell(row, 13).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 14).Value = item.Id;
                sheet.Cell(row, 14).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            sheet.Range(1, 1, 23, 14);

            sheet.Cell(24, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            //sheet.Range(23, 1, 24, 15).Merge();

            sheet.Cell(25, 1).Value = "عنوان سازمان";
            sheet.Cell(25, 2).Value = "کد سازمان";
            sheet.Cell(25, 3).Value = "کد اصلاح/توسعه شبکه ...";
            sheet.Cell(25, 4).Value = "آدرس";
            sheet.Cell(25, 5).Value = "کد محل تامین اعتبار";
            sheet.Cell(25, 6).Value = "کد کندمان";
            sheet.Cell(25, 7).Value = "کد جنس لوله";
            sheet.Cell(25, 8).Value = "کد قطر لوله";
            sheet.Cell(25, 9).Value = "طول اجرا";
            sheet.Cell(25, 10).Value = "هزینه هر متر خرید لوله";
            sheet.Cell(25, 11).Value = "هزینه هر متر اجرا";
            sheet.Cell(25, 12).Value = "جمع کل هزینه ها (هزار ریال)";
            sheet.Cell(25, 13).Value = "کد بهره برداری";
            sheet.Cell(25, 14).Value = "کد سر فصل کلی در بودجه";

            var totalCount = model.Items.Count();
            row = 26;
            for (int i = 0; i < totalCount; i++)
            {
                var item = model.Items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(25, 1, row - 1, 14);
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
