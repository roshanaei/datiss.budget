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
    public static class ConsumeForcastWsExcel
    {
        private const string _sheetName = "ConsumeForcastWs";

        public static XLWorkbook ExportExcel(this IEnumerable<ConsumeForcastWsDTO> items)
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
            for (int i = 0; i < items.Count(); i++)
            {
                var item = items.ElementAt(i);
                var row = i + 2;
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeTitle;
                sheet.Cell(row, 4).Value = item.UsageLayerTitle;
                sheet.Cell(row, 5).Value = item.CountUser;
                sheet.Cell(row, 5).DataType = XLDataType.Number;
                sheet.Cell(row, 6).Value = item.UnitUser;
                sheet.Cell(row, 6).DataType = XLDataType.Number;
                sheet.Cell(row, 7).Value = item.ConsumeUser;
                sheet.Cell(row, 7).DataType = XLDataType.Number;
                sheet.Cell(row, 8).Value = item.AvgConsumeUser;
                sheet.Cell(row, 8).DataType = XLDataType.Number;
                sheet.Cell(row, 9).Value = item.ConsumeUserForcast;
                sheet.Cell(row, 9).DataType = XLDataType.Number;
            }

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<ConsumeForcastWsDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 5).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "قطر انشعاب";
            sheet.Cell(2, 4).Value = "کد قطر انشعاب";
            sheet.Cell(2, 5).Value = "تعرفه نصب انشعاب";

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

            var range = sheet.Range(2, 1, row - 1, 5);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(4).Style.NumberFormat.Format = "#,##0";
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(6).Style.NumberFormat.Format = "#,##0";
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
