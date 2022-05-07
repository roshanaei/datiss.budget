using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostForcastFinanceExcel
    {
        private const string _sheetName = "CostForcastFinance";

        public static XLWorkbook ExportExcel(this IEnumerable<CostForcastFinanceDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "مرکز هزینه";
            sheet.Cell(1, 4).Value = "عنوان دارایی";
            sheet.Cell(1, 5).Value = "مانده دارایی در سال پایه بودجه";
            sheet.Cell(1, 6).Value = "دارایی ایجاد شده در نیمه اول سال ماقبل بودجه";
            sheet.Cell(1, 7).Value = "پیش بینی دارایی ایجاد شده در نیمه دوم";
            sheet.Cell(1, 8).Value = "پیش بینی دارایی ایجاد شده در سال بودجه";
            sheet.Cell(1, 9).Value = "کل دارایی ایجاد شده در پایان سال بودجه";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 4).Value = item.FinanceSubjectTypeDisplay;
                sheet.Cell(row, 5).Value = item.RemainingAssets;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.AssetsCreated6_1;
                sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.AssetsCreated6_2;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.ForcastAssets_D;
                sheet.Cell(row, 8).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.TotalAssetsCreated_D;
                sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 9);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
        public static XLWorkbook GetImportTemplate(this IEnumerable<CostForcastFinanceDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 11).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان مرکز هزینه";
            sheet.Cell(2, 4).Value = "کد مرکز هزینه";
            sheet.Cell(2, 5).Value = "عنوان دارایی";
            sheet.Cell(2, 6).Value = "کد دارایی";
            sheet.Cell(2, 7).Value = "مانده دارایی در سال پایه بودجه";
            sheet.Cell(2, 8).Value = "دارایی ایجاد شده در نیمه اول سال ماقبل بودجه";
            sheet.Cell(2, 9).Value = "پیش بینی دارایی ایجاد شده در نیمه دوم";
            sheet.Cell(2, 10).Value = "پیش بینی دارایی ایجاد شده در سال بودجه";
            sheet.Cell(2, 11).Value = "کل دارایی ایجاد شده در پایان سال بودجه";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 4).Value = item.CostCenterTypeId;
                sheet.Cell(row, 5).Value = item.FinanceSubjectTypeDisplay;
                sheet.Cell(row, 6).Value = item.FinanceSubjectTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 11);
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(8).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(11).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }
}
