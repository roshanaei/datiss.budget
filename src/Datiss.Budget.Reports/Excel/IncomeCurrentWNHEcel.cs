using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class IncomeCurrentWNHExcel
    {
        private const string _sheetName = "IncomeCurrentWNH";

        public static XLWorkbook ExportExcel(this IEnumerable<IncomeCurrentWNHDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "کاربری";
            sheet.Cell(1, 4).Value = "تعداد مشترک";
            sheet.Cell(1, 5).Value = "آحاد مشترک";
            sheet.Cell(1, 6).Value = "متوسط مصرف ماهیانه";
            sheet.Cell(1, 7).Value = "مصرف آب";
            sheet.Cell(1, 8).Value = "ظرفیت قراردادی";
            sheet.Cell(1, 9).Value = "قیمت هر کاربری";
            sheet.Cell(1, 10).Value = "درآمد آب بها";
            sheet.Cell(1, 11).Value = "درآمد آب بها مازاد بر ظرفیت";
            sheet.Cell(1, 12).Value = "درآمد آب بها فصلی";
            sheet.Cell(1, 13).Value = "قیمت آب تبصره 3";
            sheet.Cell(1, 14).Value = "درآمد تبصره 3 آب بها";
            sheet.Cell(1, 15).Value = "درآمد آبونمان";
            sheet.Cell(1, 16).Value = "درآمد کل آب بها";
            sheet.Cell(1, 17).Value = "درآمد تبصره 2 آب بها";
            sheet.Cell(1, 18).Value = "حجم دفع";
            sheet.Cell(1, 19).Value = "تفاوت حجم مصرف آب و دفع فاضلاب";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 3).DataType = XLDataType.Text;
                sheet.Cell(row, 4).Value = item.NumberUser;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.UnitUser;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.AvgConsumeUser;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.ConsumptionUser;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.Capacity;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.Cost;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.Income;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.ExcessIncome;
                sheet.Cell(row, 11).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 12).Value = item.SeasonalIncome;
                sheet.Cell(row, 12).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 13).Value = item.Note3Price;
                sheet.Cell(row, 13).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 14).Value = item.Note3Income;
                sheet.Cell(row, 14).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 15).Value = item.SubscriptionIncome;
                sheet.Cell(row, 15).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 16).Value = item.TotalIncome;
                sheet.Cell(row, 16).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 17).Value = item.Note2Income;
                sheet.Cell(row, 17).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 18).Value = item.WasteVolume;
                sheet.Cell(row, 18).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 18).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 19).Value = item.Diff_ConsWsVolume;
                sheet.Cell(row, 19).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 19).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 19);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<IncomeCurrentWNHDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 4).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";

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

            var range = sheet.Range(2, 1, row - 1, 4);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
