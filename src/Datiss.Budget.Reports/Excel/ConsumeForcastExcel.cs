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
    }
}
