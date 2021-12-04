using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class WasteSalesSplitExcel
    {
        private const string _sheetName = "WasteSalesSplit";

        public static XLWorkbook ExportExcel(this IEnumerable<WasteSalesSplitDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "قطر لوله فاضلاب";
            sheet.Cell(1, 5).Value = "تعداد انشعاب";
            sheet.Cell(1, 6).Value = "آحاد انشعاب";

            for (int i = 0; i < items.Count(); i++)
            {
                var item = items.ElementAt(i);
                var row = i + 2;
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.WspipeDiameterDisplay;
                sheet.Cell(row, 5).Value = item.NumberSales;
                sheet.Cell(row, 6).Value = item.UnitSales;
                sheet.Cell(row, 6).DataType = XLDataType.Number;
            }

            return workbook;
        }
    }
}
