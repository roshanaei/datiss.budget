using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostForcastWInvestmentReportExcel
    {
        private const string _sheetName = "CostForcastWInvestmentReport";

        public static XLWorkbook ExportExcel (this IEnumerable<CostForcastWInvestmentReportDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "مرکز هزینه";
            sheet.Cell(1, 4).Value = "شرح";

            sheet.Cell(1, 5).Value = "واحد اندازه گیری";
            sheet.Cell(1, 6).Value = "مقدار_منابع داخلی";
            sheet.Cell(1, 7).Value = "مبلغ_منابع داخلی";
            sheet.Cell(1, 8).Value = "مقدار_منابع عمرانی";
            sheet.Cell(1, 9).Value = "مبلغ_منابع عمرانی";

            sheet.Cell(1, 10).Value = "مقدار_مشارکت غیر دولتی";
            sheet.Cell(1, 11).Value = "مبلغ_مشارکت غیر دولتی";

            sheet.Cell(1, 12).Value = "مقدار_جمع";
            sheet.Cell(1, 13).Value = "مبلغ_جمع";

            var totalcount = items.Count();

            int row = 2;
            for (int i = 0; i < totalcount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;
                sheet.Cell(row, 3).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 4).Value = item.SectionTypeDisplay;
                sheet.Cell(row, 5).Value = item.UnitTypeDisplay;
                sheet.Cell(row, 6).Value = item.Amount1;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 7).Value = item.Cost1;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 8).Value = item.Amount2;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 9).Value = item.Cost2;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                sheet.Cell(row, 10).Value = item.Amount3;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 11).Value = item.Cost3;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                sheet.Cell(row, 12).Value = item.Amount4;
                sheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 13).Value = item.Cost4;
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 13);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleLight16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this IEnumerable<CostForcastWInvestmentReportDTO> items, int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 10).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "عنوان مرکز هزینه";
            sheet.Cell(2, 4).Value = "کد مرکز هزینه";
            sheet.Cell(2, 5).Value = "عنوان شرح";
            sheet.Cell(2, 6).Value = "کد شرح";
            sheet.Cell(2, 7).Value = "عنوان واحد اندازه گیری";
            sheet.Cell(2, 8).Value = "کد واحد اندازه گیری";


            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                sheet.Cell(row, 3).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 4).Value = item.CostCenterTypeId;
                sheet.Cell(row, 5).Value = item.SectionTypeDisplay;
                sheet.Cell(row, 6).Value = item.SectionTypeId;
                sheet.Cell(row, 7).Value = item.UnitTypeDisplay;
                sheet.Cell(row, 8).Value = item.UnitTypeId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 8);
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
