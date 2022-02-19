using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Reports;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentPMDepExcel
    {
        private const string _sheetName = "CostCurrentPMDep";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentPMDepDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "نوع رکورد";
            sheet.Cell(1, 4).Value = "حوزه فعالیت";
            sheet.Cell(1, 5).Value = "مراکز هزینه";
            sheet.Cell(1, 6).Value = "عناوین استهلاک و تعمیر و نگهداری";
            sheet.Cell(1, 7).Value = "هزینه تعمیر و نگهداری دارائی";
            sheet.Cell(1, 8).Value = "ضریب هزینه تعمیر و نگهداری دارائی";
            sheet.Cell(1, 9).Value = "هزینه استهلاک دارایی";
            sheet.Cell(1, 10).Value = "ضریب هزینه استهلاک دارایی";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();

                sheet.Cell(row, 2).Value = item.OrganizationDisplay;

                sheet.Cell(row, 3).Value = item.RecordType.ToDisplay();

                sheet.Cell(row, 4).Value = item.ActivityType.ToDisplay();

                sheet.Cell(row, 5).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 5).DataType = XLDataType.Text;

                sheet.Cell(row, 6).Value = item.CCPMDepTypeDisplay;
                sheet.Cell(row, 6).DataType = XLDataType.Text;

                sheet.Cell(row, 7).Value = item.FinancePMCost;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 8).Value = item.RFinancePMCost_D;
                sheet.Cell(row, 8).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 9).Value = item.FinanceDepCost;
                sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 10).Value = item.RFinanceDepCost_D;
                sheet.Cell(row, 10).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 10);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentPMDepDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات پایه برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 12).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "حوزه فعالیت";
            sheet.Cell(2, 4).Value = "کد فعالیت";
            sheet.Cell(2, 5).Value = "عنوان مرکز هزینه";
            sheet.Cell(2, 6).Value = "کد مرکز";
            sheet.Cell(2, 7).Value = "عناوین استهلاک و تعمیر و نگهداری";
            sheet.Cell(2, 8).Value = "کد عناوین";
            sheet.Cell(2, 9).Value = "هزینه تعمیر و نگهداری دارائی";
            sheet.Cell(2, 10).Value = "ضریب هزینه تعمیر و نگهداری دارائی";
            sheet.Cell(2, 11).Value = "هزینه استهلاک دارایی";
            sheet.Cell(2, 12).Value = "ضریب هزینه استهلاک دارایی";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.ActivityType.ToDisplay();
                sheet.Cell(row, 4).Value = (int)item.ActivityType;
                sheet.Cell(row, 5).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 6).Value = item.CostCenterTypeId;
                sheet.Cell(row, 7).Value = item.CCPMDepTypeDisplay;
                sheet.Cell(row, 8).Value = item.CCPMDepTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 11);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(9).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(11).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            range.Column(11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(12).Style.NumberFormat.Format = ConstantReport.__DecimalFormat;
            range.Column(12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
