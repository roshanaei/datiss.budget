using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentRawMaterialExcel
    {
        private const string _sheetName = "CostCurrentRawMaterial";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentRawMaterialDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "نوع فعالیت";
            sheet.Cell(1, 4).Value = "ماده اولیه";
            sheet.Cell(1, 5).Value = "مبلغ سال پایه";
            sheet.Cell(1, 6).Value = "مبلغ سال ماقبل بودجه";
            sheet.Cell(1, 7).Value = "مبلغ پیش بینی";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.ActivityType.ToDisplay();
                sheet.Cell(row, 4).Value = item.RawMaterialTypeDisplay;

                sheet.Cell(row, 5).Value = item.BaseFee;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 6).Value = item.LastYearFee;
                sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 7).Value = item.ForcastFee;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 7);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostCurrentRawMaterialDTO> items, int year)
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
            sheet.Cell(2, 3).Value = "نوع فعالیت";
            sheet.Cell(2, 4).Value = "کد فعالیت";
            sheet.Cell(2, 5).Value = "مواد اولیه";
            sheet.Cell(2, 6).Value = "کد مواد"; 
            sheet.Cell(2, 7).Value = "مبلغ سال پایه";
            sheet.Cell(2, 8).Value = "مبلغ سال ماقبل بودجه";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.ActivityTypeDisplay;
                sheet.Cell(row, 4).Value = (int)item.ActivityType;
                sheet.Cell(row, 5).Value = item.RawMaterialTypeDisplay;
                sheet.Cell(row, 6).Value = item.RawMaterialTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 8);
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            range.Column(7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(8).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }
    }

}