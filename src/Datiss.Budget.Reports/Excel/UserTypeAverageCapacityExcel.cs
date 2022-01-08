using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class UserTypeAverageCapacityExcel
    {
        private const string _sheetName = "UserTypeAverageCapacity";

        public static XLWorkbook ExportExcel(this IEnumerable<UserTypeAverageCapacityDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "متوسط ظرفیت قراردادی آب";
            sheet.Cell(1, 5).Value = "متوسط ظرفیت قراردادی فاضلاب";
            sheet.Cell(1, 6).Value = "متوسط ظرفیت قراردادی آب - درآمد سرمایه ای";
            sheet.Cell(1, 7).Value = "متوسط ظرفیت قراردادی فاضلاب - درآمد سرمایه ای";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.AverageCapacityW;
                sheet.Cell(row, 5).Value = item.AverageCapacityWs;
                sheet.Cell(row, 6).Value = item.AverageCapacityWIncome;
                sheet.Cell(row, 7).Value = item.AverageCapacityWsIncome;
                sheet.Cell(row, 7).DataType = XLDataType.Number;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 7);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<UserTypeAverageCapacityDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 8).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";
            sheet.Cell(2, 5).Value = "متوسط ظرفیت قراردادی آب";
            sheet.Cell(2, 6).Value = "متوسط ظرفیت قراردادی فاضلاب";
            sheet.Cell(2, 7).Value = "متوسط ظرفیت قراردادی آب - درآمد سرمایه ای";
            sheet.Cell(2, 8).Value = "متوسط ظرفیت قراردادی فاضلاب - درآمد سرمایه ای";

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

            var range = sheet.Range(2, 1, row - 1, 8);
            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
