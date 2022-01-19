using ClosedXML.Excel;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class NHCoExcel
    {
        private const string _sheetName = "NHCo";

        public static XLWorkbook ExportExcel(this IEnumerable<NHCoDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "نوع فعالیت";
            sheet.Cell(1, 4).Value = "حد ظرفیت برای متغیر اول";
            sheet.Cell(1, 5).Value = "مبلغ ثابت هزینه مشارکت";
            sheet.Cell(1, 6).Value = "مبلغ هزینه مشارکت متغیر اول";
            sheet.Cell(1, 7).Value = "مبلغ هزینه مشارکت متغیر دوم";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.ActivityType.ToDisplay();
                sheet.Cell(row, 4).Value = item.P1Capacity;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.FixCostCo;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.P1CostCo;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.P2CostCo;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 7);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<NHCoDTO> items, int year ,ActivityType activity)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year + " و نوع فعالیت : " + activity.ToDisplay();
            sheet.Range(1, 1, 1, 6).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "حد ظرفیت برای متغیر اول";
            sheet.Cell(2, 4).Value = "مبلغ ثابت هزینه مشارکت";
            sheet.Cell(2, 5).Value = "مبلغ هزینه مشارکت متغیر اول";
            sheet.Cell(2, 6).Value = "مبلغ هزینه مشارکت متغیر دوم";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.P1Capacity;
                sheet.Cell(row, 4).Value = item.FixCostCo;
                sheet.Cell(row, 5).Value = item.P1CostCo;
                sheet.Cell(row, 6).Value = item.P2CostCo;
                row++;
            }

            var range = sheet.Range(2, 1, row - 1, 6);
            //
            range.Column(3).Style.NumberFormat.Format = "#,##0";
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(4).Style.NumberFormat.Format = "#,##0";
            range.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(6).Style.NumberFormat.Format = "#,##0";
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
