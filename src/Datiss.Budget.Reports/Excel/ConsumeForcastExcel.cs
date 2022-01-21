using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Reports.Excel
{
    public static class ConsumeForcastExcel
    {
        private const string _sheetName = "ConsumeForcast";

        public static XLWorkbook ExportExcel(this IEnumerable<ConsumeForcastDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "طبقه مصرف";
            sheet.Cell(1, 5).Value = "تعداد";
            sheet.Cell(1, 6).Value = "آحاد";
            sheet.Cell(1, 7).Value = "متوسط ظرفیت قراردادی";
            sheet.Cell(1, 8).Value = "میانگین مصرف"; 
            sheet.Cell(1, 9).Value = "پیش بینی ظرفیت قراردادی";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeTitle;
                sheet.Cell(row, 4).Value = item.UsageLayerTitle;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 5).Value = item.CountUser;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.UnitUser;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.ConsumeUser;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.AvgConsumeUser;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.ConsumeUserForcast;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 9);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<ConsumeForcastDTO> items, int year)
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
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";
            sheet.Cell(2, 5).Value = "طبقه مصرف";
            sheet.Cell(2, 6).Value = "کد طبقه مصرف";
            sheet.Cell(2, 7).Value = "تعداد";
            sheet.Cell(2, 8).Value = "آحاد";
            sheet.Cell(2, 9).Value = "متوسط ظرفیت قراردادی";
            sheet.Cell(2, 10).Value = "میانگین مصرف";
            sheet.Cell(2, 11).Value = "پیش بینی ظرفیت قراردادی";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.UserTypeTitle;
                sheet.Cell(row, 4).Value = item.UserTypeId;
                sheet.Cell(row, 5).Value = item.UsageLayerTitle;
                sheet.Cell(row, 6).Value = item.UsageLayerId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 11);
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(7).Style.NumberFormat.Format = "#,##0.00";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(8).Style.NumberFormat.Format = "#,##0.00";
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(9).Style.NumberFormat.Format = "#,##0.00";
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = "#,##0.00";
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(11).Style.NumberFormat.Format = "#,##0.00";
            range.Column(11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
