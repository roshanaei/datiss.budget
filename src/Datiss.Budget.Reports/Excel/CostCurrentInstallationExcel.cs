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
    public static class CostCurrentInstallationExcel
    {
        private const string _sheetName = "CostCurrentInstallation";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentInstalationDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "بخش";
            sheet.Cell(1, 4).Value = "عنوان";
            sheet.Cell(1, 5).Value = "تعداد انشعاب طی دوره";
            sheet.Cell(1, 6).Value = "بهاء واحد";
            sheet.Cell(1, 7).Value = "جمع کل هزینه";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.ActivityType.ToDisplay();
                sheet.Cell(row, 4).Value = item.CCInstalationTypeDisplay;

                sheet.Cell(row, 5).Value = item.NumberUser;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                sheet.Cell(row, 6).Value = item.Cost;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                sheet.Cell(row, 7).Value = item.Income;
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

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentInstalationDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 9).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان بخش";
            sheet.Cell(2, 4).Value = "کد بخش";
            sheet.Cell(2, 5).Value = "عنوان";
            sheet.Cell(2, 6).Value = "کد عنوان";
            sheet.Cell(2, 7).Value = "تعداد انشعاب طی دوره";
            sheet.Cell(2, 8).Value = "بهاء واحد";
            sheet.Cell(2, 9).Value = "جمع کل هزینه";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.ActivityTypeDisplay;
                sheet.Cell(row, 4).Value = (int)item.ActivityType;
                sheet.Cell(row, 5).Value = item.CCInstalationTypeDisplay;
                sheet.Cell(row, 6).Value = item.CCInstalationTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 9);
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(7).Style.NumberFormat.Format = "#,##0";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            
            range.Column(8).Style.NumberFormat.Format = "#,##0";
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            
            range.Column(9).Style.NumberFormat.Format = "#,##0";
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }

}