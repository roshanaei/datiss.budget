using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class IncomeCurrentWHExcel
    {
        private const string _sheetName = "IncomeCurrentWH";

        public static XLWorkbook ExportExcel(this IEnumerable<IncomeCurrentWHDTO> items)
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
            sheet.Cell(1, 5).Value = "تعداد مشترک";
            sheet.Cell(1, 6).Value = "آحاد مشترک";
            sheet.Cell(1, 7).Value = "قیمت هر طبقه";
            sheet.Cell(1, 8).Value = "متوسط مصرف هر طبقه";
            sheet.Cell(1, 9).Value = "مصرف آب";
            sheet.Cell(1, 10).Value = "درآمد آب بها";
            sheet.Cell(1, 11).Value = "درآمد آبونمان";
            sheet.Cell(1, 12).Value = "درآمد بها فصلی";
            sheet.Cell(1, 13).Value = "درآمد کل آب بها";
            sheet.Cell(1, 14).Value = "قیمت تبصره 3 آب بها خانگی";
            sheet.Cell(1, 15).Value = "درآمد تبصره 3 آب بها";
            sheet.Cell(1, 16).Value = "تفاوت حجم مصرف آب و دفع فاضلاب";
            sheet.Cell(1, 17).Value = "درآمد تبصره 2 آب بها";
            sheet.Cell(1, 18).Value = "حجم دفع فاضلاب";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.UsageLayerDisplay;
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 5).Value = item.NumberUser;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.UnitUser;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.Cost;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.AvgConsumeUser;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.ConsumptionUser;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.Income;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.SubscriptionIncome;
                sheet.Cell(row, 11).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 12).Value = item.SeasonalIncome;
                sheet.Cell(row, 12).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 13).Value = item.TIncome;
                sheet.Cell(row, 13).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 14).Value = item.Note3Price;
                sheet.Cell(row, 14).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 15).Value = item.Note3Income;
                sheet.Cell(row, 15).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 16).Value = item.Diff_ConsWsVolume;
                sheet.Cell(row, 16).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 17).Value = item.Note2Income;
                sheet.Cell(row, 17).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 18).Value = item.WasteVolume;
                sheet.Cell(row, 18).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 18).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 18);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<IncomeCurrentWHDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 20).Merge();
            //sheet.Range(1, 1, 1, 6).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان کاربری";
            sheet.Cell(2, 4).Value = "کد کاربری";
            sheet.Cell(2, 5).Value = "طبقه مصرف";
            sheet.Cell(2, 6).Value = "کد طبقه مصرف";
            sheet.Cell(2, 7).Value = "تعداد مشترک";
            sheet.Cell(2, 8).Value = "آحاد مشترک";
            sheet.Cell(2, 9).Value = "قیمت هر طبقه";
            sheet.Cell(2, 10).Value = "متوسط مصرف هر طبقه";
            sheet.Cell(2, 11).Value = "مصرف آب";
            sheet.Cell(2, 12).Value = "درآمد آب بها";
            sheet.Cell(2, 13).Value = "درآمد آبونمان";
            sheet.Cell(2, 14).Value = "درآمد بها فصلی";
            sheet.Cell(2, 15).Value = "درآمد کل آب بها";
            sheet.Cell(2, 16).Value = "قیمت تبصره 3 آب بها خانگی";
            sheet.Cell(2, 17).Value = "درآمد تبصره 3 آب بها";
            sheet.Cell(2, 18).Value = "تفاوت حجم مصرف آب و دفع فاضلاب";
            sheet.Cell(2, 19).Value = "درآمد تبصره 2 آب بها";
            sheet.Cell(2, 20).Value = "حجم دفع فاضلاب";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).Value = item.UserTypeId;
                sheet.Cell(row, 5).Value = item.UsageLayerDisplay;
                sheet.Cell(row, 6).Value = item.UsageLayerId;
                sheet.Cell(row, 7).Value = item.NumberUser;
                sheet.Cell(row, 8).Value = item.UnitUser;
                sheet.Cell(row, 9).Value = item.Cost;
                sheet.Cell(row, 10).Value = item.AvgConsumeUser;
                sheet.Cell(row, 11).Value = item.ConsumptionUser;
                sheet.Cell(row, 12).Value = item.Income;
                sheet.Cell(row, 13).Value = item.SubscriptionIncome;
                sheet.Cell(row, 14).Value = item.SeasonalIncome;
                sheet.Cell(row, 15).Value = item.TIncome;
                sheet.Cell(row, 16).Value = item.Note3Price;
                sheet.Cell(row, 17).Value = item.Note3Income;
                sheet.Cell(row, 18).Value = item.Diff_ConsWsVolume;
                sheet.Cell(row, 19).Value = item.Note2Income;
                sheet.Cell(row, 20).Value = item.WasteVolume;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 20);
            //var range = sheet.Range(2, 1, row - 1, 6);
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(7).Style.NumberFormat.Format = "#,##0";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(8).Style.NumberFormat.Format = "#,##0";
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(9).Style.NumberFormat.Format = "#,##0";
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = "#,##0.00";
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(11).Style.NumberFormat.Format = "#,##0";
            range.Column(11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(12).Style.NumberFormat.Format = "#,##0";
            range.Column(12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(13).Style.NumberFormat.Format = "#,##0";
            range.Column(13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(14).Style.NumberFormat.Format = "#,##0";
            range.Column(14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(15).Style.NumberFormat.Format = "#,##0";
            range.Column(15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(16).Style.NumberFormat.Format = "#,##0";
            range.Column(16).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(17).Style.NumberFormat.Format = "#,##0";
            range.Column(17).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(18).Style.NumberFormat.Format = "#,##0";
            range.Column(18).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(19).Style.NumberFormat.Format = "#,##0";
            range.Column(19).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(20).Style.NumberFormat.Format = "#,##0";
            range.Column(20).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
