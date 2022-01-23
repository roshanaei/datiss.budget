using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Reports.Excel
{
    public static class WWsFeeExcel
    {
        private const string _sheetName = "WWsFee";

        public static XLWorkbook ExportExcel(this IEnumerable<WWsFeeDTO> items) 
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "حوزه فعالیت";
            sheet.Cell(1, 4).Value = "کاربری";
            sheet.Cell(1, 5).Value = "طبقه مصرف";
            sheet.Cell(1, 6).Value = "پارامتر اول تعرفه بهای خدمات ";
            sheet.Cell(1, 7).Value = "پارامتر دوم تعرفه بهای خدمات ";
            sheet.Cell(1, 8).Value = "پارامتر اول خدمات تبصره 3 ";
            sheet.Cell(1, 9).Value = "پارامتر دوم خدمات تبصره 3 ";
            sheet.Cell(1, 10).Value = "پارامتر اول خدمات تبصره 7 ";
            sheet.Cell(1, 11).Value = "پارامتر دوم خدمات تبصره 7 ";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.ActivityTypeDisplay;
                sheet.Cell(row, 3).DataType = XLDataType.Text;
                sheet.Cell(row, 4).Value = item.UserTypeDisplay;
                sheet.Cell(row, 4).DataType = XLDataType.Text;
                sheet.Cell(row, 5).Value = item.UsageLayerDisplay;
                sheet.Cell(row, 5).DataType = XLDataType.Text;
                sheet.Cell(row, 6).Value = item.P1Fee;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.P2Fee;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.P1Note3;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.P1Note7;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.P2Note3;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.P2Note7;
                sheet.Cell(row, 11).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 11);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<WWsFeeDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 14).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان حوزه فعالیت";
            sheet.Cell(2, 4).Value = "کد حوزه فعالیت";
            sheet.Cell(2, 5).Value = "عنوان کاربری";
            sheet.Cell(2, 6).Value = "کد کاربری";
            sheet.Cell(2, 7).Value = "عنوان طبقات مصرف مسکونی";
            sheet.Cell(2, 8).Value = "کد طبقات مصرف مسکونی";
            sheet.Cell(2, 9).Value = "پارامتر اول تعرفه بهای خدمات";
            sheet.Cell(2, 10).Value = "پارامتر دوم تعرفه بهای خدمات";
            sheet.Cell(2, 11).Value = "پارامتر اول خدمات تبصره 3";
            sheet.Cell(2, 12).Value = "پارامتر دوم خدمات تبصره 3";
            sheet.Cell(2, 13).Value = "پارامتر اول خدمات تبصره 7";
            sheet.Cell(2, 14).Value = "پارامتر دوم خدمات تبصره 7";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.ActivityTypeDisplay;
                sheet.Cell(row, 4).Value = (int)item.ActivityType;
                sheet.Cell(row, 5).Value = item.UserTypeDisplay;
                sheet.Cell(row, 6).Value = item.UserTypeId;
                sheet.Cell(row, 7).Value = item.UsageLayerDisplay;
                sheet.Cell(row, 8).Value = item.UsageLayerId;
                row++;
            }

            var range = sheet.Range(2, 1, row - 1, 14);
            range.Column(9).Style.NumberFormat.Format = "#,##0";
            range.Column(10).Style.NumberFormat.Format = "#,##0";
            range.Column(11).Style.NumberFormat.Format = "#,##0";
            range.Column(12).Style.NumberFormat.Format = "#,##0";
            range.Column(13).Style.NumberFormat.Format = "#,##0";
            range.Column(14).Style.NumberFormat.Format = "#,##0";
            range.Column(15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
