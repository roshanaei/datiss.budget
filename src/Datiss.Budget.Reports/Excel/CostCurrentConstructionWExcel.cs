using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{

    public static class CostCurrentConstructionWExcel
    {
        private const string _sheetName = "CostCurrentConstructionW";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentConstructionWDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "شرح پروژه های عمرانی";
            sheet.Cell(1, 4).Value = "عنوان هزینه سرمایه ای";
            sheet.Cell(1, 5).Value = "مرکز هزینه";
            sheet.Cell(1, 6).Value = "حوزه بهره بردار در ستاد";
            sheet.Cell(1, 7).Value = "درصد پیشرفت فیزیکی";
            sheet.Cell(1, 8).Value = "هزینه انجام شده (هزار ریال)";
            sheet.Cell(1, 9).Value = "واحد";
            sheet.Cell(1, 10).Value = "قیمت واحد(هزار ریال)";
            sheet.Cell(1, 11).Value = "مقدار";
            sheet.Cell(1, 12).Value = "(هزار ریال)کل هزینه اجرایی پروژه";
            sheet.Cell(1, 13).Value = "محل تامین اعتبار";
            sheet.Cell(1, 14).Value = "این پروژه در سال بودجه به بهره برداری رسیده؟";
            sheet.Cell(1, 15).Value = "سرفصل کلی در بودجه پیشنهادی";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.ProjectDescription;
                sheet.Cell(row, 4).Value = item.WaterInvestorsDisplay;
                sheet.Cell(row, 5).Value = item.CostCenterDisplay;
                sheet.Cell(row, 6).Value = item.ExploitationAreaDisplay;
                sheet.Cell(row, 7).Value = item.ProgressPercent;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.CostDone;
                sheet.Cell(row, 8).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.MeasurementDisplay;
                sheet.Cell(row, 10).Value = item.UnitPrice;
                sheet.Cell(row, 10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.Amount;
                sheet.Cell(row, 11).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 12).Value = item.TotalCost;
                sheet.Cell(row, 12).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 13).Value = item.CreditDisplay;
                sheet.Cell(row, 14).Value = item.ExtensionDisplay;
                sheet.Cell(row, 15).Value = item.SuggestedBudgetTopicDisplay;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 15);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this CostCurrentConstructionWImportViewModel model, int year)
        {
            if (!model.Items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);
            sheet.RightToLeft = true;
            //
            sheet.Cell(1, 1).Value = "عنوان هزینه سرمایه ای";
            sheet.Cell(1, 2).Value = "کد هزینه سرمایه ای";
            sheet.Cell(1, 3).Value = "مرکز هزینه";
            sheet.Cell(1, 4).Value = "کد مرکز هزینه";
            sheet.Cell(1, 5).Value = "حوزه بهره بردار در ستاد";
            sheet.Cell(1, 6).Value = "کد حوزه بهره بردار در ستاد";
            sheet.Cell(1, 7).Value = "کد واحد";
            sheet.Cell(1, 8).Value = "محل تامین اعتبار";
            sheet.Cell(1, 9).Value = "کد محل تامین اعتبار";
            sheet.Cell(1, 10).Value = "این پروژه در سال بودجه به بهره برداری رسیده؟";
            sheet.Cell(1, 11).Value = "کد بهره برداری";
            sheet.Cell(1, 12).Value = "سر فصل کلی در بودجه ";
            sheet.Cell(1, 13).Value = "کد سر فصل کلی در بودجه ";
            int row = 2;
            foreach (var item in model.WaterInvestorsTypeSource)
            {
                sheet.Cell(row,1).Value = item.Title;
                sheet.Cell(row,2).Value = item.Id;
                row++;
            }
            //sheet.Range(1, 1, 22, 13);
            //
            //sheet.Cell(23, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            //sheet.Range(23, 1, 24, 15).Merge();

            //sheet.Cell(24, 1).Value = "عنوان سازمان";
            //sheet.Cell(24, 2).Value = "کد سازمان";
            //sheet.Cell(24, 3).Value = "شرح پروژه های عمرانی";
            //sheet.Cell(24, 4).Value = "عنوان هزینه سرمایه ای";
            //sheet.Cell(24, 5).Value = "مرکز هزینه";
            //sheet.Cell(24, 6).Value = "حوزه بهره بردار در ستاد";
            //sheet.Cell(24, 7).Value = "درصد پیشرفت فیزیکی";
            //sheet.Cell(24, 8).Value = "هزینه انجام شده (هزار ریال)";
            //sheet.Cell(24, 9).Value = "واحد";
            //sheet.Cell(24, 10).Value = "قیمت واحد(هزار ریال)";
            //sheet.Cell(24, 11).Value = "مقدار";
            //sheet.Cell(24, 12).Value = "(هزار ریال)کل هزینه اجرایی پروژه";
            //sheet.Cell(24, 13).Value = "محل تامین اعتبار";
            //sheet.Cell(24, 14).Value = "این پروژه در سال بودجه به بهره برداری رسیده؟";
            //sheet.Cell(24, 15).Value = "سر فصل کلی در بودجه پیشنهادی";

            //var totalCount = model.Items.Count();
            //row = 25;
            //for (int i = 0; i < totalCount; i++)
            //{
            //    var item = model.Items.ElementAt(i);
            //    sheet.Cell(row, 1).Value = item.OrganizationDisplay;
            //    sheet.Cell(row, 2).Value = item.OrganizationId;
            //    row++; //for keeping index in table records
            //}

            var range = sheet.Range(1, 1, row-1 , 13);
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
