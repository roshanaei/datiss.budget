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
    public static class WaterInstallFeeExcel
    {
        private const string _sheetName = "WaterInstallFee";

        public static XLWorkbook ExportExcel(this IEnumerable<WaterInstallFeeDTO> items) {
            if(items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "قیمت";
            
            for(int i = 0; i < items.Count(); i++) {
                var item = items.ElementAt(i);
                var row = i + 2;
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.DWaterTypeDisplay;
                sheet.Cell(row, 4).Value = item.WInstallFee;
                sheet.Cell(row, 4).DataType = XLDataType.Number;
            }

            return workbook;
        }
    }
}
