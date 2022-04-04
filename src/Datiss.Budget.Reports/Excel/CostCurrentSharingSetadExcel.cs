using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentSharingSetadExcel
    {
        private const string _sheetName = "CostCurrentSharingSetad";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentSharingSetadDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 2).Value = "احاد مشترکین آب";
            sheet.Cell(1, 3).Value = "درآمد جاری";
            sheet.Cell(1, 4).Value = "ضریب تسهیم درآمد جاری";
            sheet.Cell(1, 5).Value = "درصد سهام شهرداری";
            sheet.Cell(1, 6).Value = "درآمد سرمایه ای";
            sheet.Cell(1, 7).Value = "ضریب تسهیم درآمد سرمایه ای";
            sheet.Cell(1, 8).Value = "آحاد مشترک فاضلاب";
            sheet.Cell(1, 9).Value = "درآمد جاری فاضلاب";
            sheet.Cell(1, 10).Value = "ضریب تسهیم درآمد جاری فاضلاب";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.WUnit;
                sheet.Cell(row, 3).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 4).Value = item.IncomeCurrentW;
                sheet.Cell(row, 4).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.IncomeCurrentWSharingCoff;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.SPSHahrdari;
                sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.IncomeForcast;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.IncomeForcastsharing;
                sheet.Cell(row, 8).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.WsUnit;
                sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.IncomeCurrentWs;
                sheet.Cell(row, 10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.IncomeCurrentWsSharingCoff;
                sheet.Cell(row, 11).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 11);

            var table = range.CreateTable($"{_sheetName}_Table");

            table.Theme = XLTableTheme.TableStyleMedium16;

            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentSharingSetadDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 11).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "احاد مشترکین آب";
            sheet.Cell(2, 4).Value = "درآمد جاری";
            sheet.Cell(2, 5).Value = "ضریب تسهیم درآمد جاری";
            sheet.Cell(2, 6).Value = "درصد سهام شهرداری";
            sheet.Cell(2, 7).Value = "درآمد سرمایه ای";
            sheet.Cell(2, 8).Value = "ضریب تسهیم درآمد سرمایه ای";
            sheet.Cell(2, 9).Value = "آحاد مشترک فاضلاب";
            sheet.Cell(2, 10).Value = "درآمد جاری فاضلاب";
            sheet.Cell(2, 11).Value = "ضریب تسهیم درآمد جاری فاضلاب";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                row++;
            }

            var range = sheet.Range(2, 1, row - 1, 11);

            range.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(3).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(4).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 8).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(row, 11).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
