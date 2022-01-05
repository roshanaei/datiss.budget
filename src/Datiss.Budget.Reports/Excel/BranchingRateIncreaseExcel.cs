using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class BranchingRateIncreaseExcel
    {
        private const string _sheetName = "BranchingRateIncrease";

        public static XLWorkbook ExportExcel(this IEnumerable<BranchingRateIncreaseDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "ضریب افزایش حق انشعاب آب";
            sheet.Cell(1, 5).Value = "ضریب افزایش حق انشعاب فاضلاب";
            sheet.Cell(1, 6).Value = "ضریب حق انشعاب فاضلاب(درصد)";
            sheet.Cell(1, 7).Value = "مبلغ ثابت یک واحد تجاری";
            sheet.Cell(1, 8).Value = "مبلغ ثابت هر متر مکعب ظرفیت قراردادی";
            sheet.Cell(1, 9).Value = "ضریب افزایش حق نصب آب";
            sheet.Cell(1, 10).Value = "ضریب افزایش حق نصب فاضلاب";
            sheet.Cell(1, 11).Value = "ضریب ثابت تبصره 2 ماده واحده آب";
            sheet.Cell(1, 12).Value = "ضریب ثابت تبصره 2 ماده واحده فاضلاب";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.WaterRateIncrease;
                sheet.Cell(row, 5).Value = item.WasteRateIncrease;
                sheet.Cell(row, 6).Value = item.WastePersentIncrease;
                sheet.Cell(row, 7).Value = item.FixAmountBusiness;
                sheet.Cell(row, 8).Value = item.CapacityFixAmount;
                sheet.Cell(row, 9).Value = item.WaterInstallRateIncrease;
                sheet.Cell(row, 10).Value = item.WsInstalIncrease;
                sheet.Cell(row, 11).Value = item.WaterFixNote2;
                sheet.Cell(row, 12).Value = item.WasteFixNote2;
                sheet.Cell(row, 12).DataType = XLDataType.Number;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 12);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<BranchingRateIncreaseDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 13).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";
            sheet.Cell(2, 5).Value = "ضریب افزایش حق انشعاب آب";
            sheet.Cell(2, 6).Value = "ضریب افزایش حق انشعاب فاضلاب";
            sheet.Cell(2, 7).Value = "ضریب حق انشعاب فاضلاب(درصد)";
            sheet.Cell(2, 8).Value = "مبلغ ثابت یک واحد تجاری";
            sheet.Cell(2, 9).Value = "مبلغ ثابت هر متر مکعب ظرفیت قراردادی";
            sheet.Cell(2, 10).Value = "ضریب افزایش حق نصب آب";
            sheet.Cell(2, 11).Value = "ضریب افزایش حق نصب فاضلاب";
            sheet.Cell(2, 12).Value = "ضریب ثابت تبصره 2 ماده واحده آب";
            sheet.Cell(2, 13).Value = "ضریب ثابت تبصره 2 ماده واحده فاضلاب";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.UserTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 13);
            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
