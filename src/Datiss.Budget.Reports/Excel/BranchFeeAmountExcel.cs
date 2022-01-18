using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;

namespace Datiss.Budget.Reports.Excel
{
    public static class BranchFeeAmountExcel
    {
        private const string _sheetName = "BranchFeeAmount";
        
        public static XLWorkbook ExportExcel(this IEnumerable<BranchFeeAmountDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "سازمان";
            sheet.Cell(1, 3).Value = "ضریب تعدیل شهری";
            sheet.Cell(1, 4).Value = "هزینه لوله گذاری آب";
            sheet.Cell(1, 5).Value = "هزینه لوله گذاری فاضلاب";
            sheet.Cell(1, 6).Value = "حق انشعاب آب هر واحد مسکونی";
            sheet.Cell(1, 7).Value = "ضریب حق انشعاب فاضلاب نسبت به آب";
            sheet.Cell(1, 8).Value = "مبلغ مشارکت آب خانگی (تبصره 3)";
            sheet.Cell(1, 9).Value = "مبلغ مشارکت آب غیر خانگی (تبصره 3)";
            sheet.Cell(1, 10).Value = "مبلغ مشارکت فاضلاب خانگی (تبصره 3)";
            sheet.Cell(1, 11).Value = "مبلغ مشارکت فاضلاب غیر خانگی (تبصره 3)";
            sheet.Cell(1, 12).Value = "مبلغ ثابت ریالی ماده 11 خانگی آب";
            sheet.Cell(1, 13).Value = "مبلغ ثابت ریالی ماده 11 غیر خانگی آب";
            sheet.Cell(1, 14).Value = "مبلغ ثابت ریالی ماده 11 خانگی فاضلاب";
            sheet.Cell(1, 15).Value = "مبلغ ثابت ریالی ماده 11 غیرخانگی فاضلاب";

            var totalCount = items.Count();
            int row = 2;

            for (int i =0; i<totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 2).Value = item.OrganizationDisplay;

                sheet.Cell(row, 3).Value = item.UrbanAdjustmentFactor;
                sheet.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 4).Value = item.WasteRateInWater;
                sheet.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 5).Value = item.WaterBranchingPerHousing;
                sheet.Cell(row, 5).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 6).Value = item.TubingCost;
                sheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 7).Value = item.WaterPartnershipAmountDomestic;
                sheet.Cell(row, 7).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 8).Value = item.WaterPartnershipAmountNDomestic;
                sheet.Cell(row, 8).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 9).Value = item.WastePartnershipAmountDomestic;
                sheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 10).Value = item.WastePartnershipAmountNDomestic;
                sheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 11).Value = item.FixCostNote11H;
                sheet.Cell(row, 11).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 12).Value = item.FixCostNote11NH;
                sheet.Cell(row, 12).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 13).Value = item.FixCostNote11HWs;
                sheet.Cell(row, 13).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 14).Value = item.FixCostNote11NHWs;
                sheet.Cell(row, 14).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                sheet.Cell(row, 15).Value = item.WsTubingCost;
                sheet.Cell(row, 15).Style.NumberFormat.Format = "#,##0";
                sheet.Cell(row, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 15);

            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTamplate(this IEnumerable<BranchFeeAmountDTO> items,int year)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 5).Merge();

            sheet.Cell(2, 1).Value = "عنوان سازمان";
            sheet.Cell(2, 2).Value = "کد سازمان";
            sheet.Cell(2, 3).Value = "ضریب تعدیل شهری";
            sheet.Cell(2, 4).Value = "هزینه لوله گذاری آب";
            sheet.Cell(2, 5).Value = "هزینه لوله گذاری فاضلاب";
            sheet.Cell(2, 6).Value = "حق انشعاب آب هر واحد مسکونی";
            sheet.Cell(2, 7).Value = "ضریب حق انشعاب فاضلاب نسبت به آب";
            sheet.Cell(2, 8).Value = "مبلغ مشارکت آب خانگی (تبصره 3)";
            sheet.Cell(2, 9).Value = "مبلغ مشارکت آب غیر خانگی (تبصره 3)";
            sheet.Cell(2, 10).Value = "مبلغ مشارکت فاضلاب خانگی (تبصره 3)";
            sheet.Cell(2, 11).Value = "مبلغ مشارکت فاضلاب غیر خانگی (تبصره 3)";
            sheet.Cell(2, 12).Value = "مبلغ ثابت ریالی ماده 11 خانگی آب";
            sheet.Cell(2, 13).Value = "مبلغ ثابت ریالی ماده 11 غیر خانگی آب";
            sheet.Cell(2, 14).Value = "مبلغ ثابت ریالی ماده 11 خانگی فاضلاب";
            sheet.Cell(2, 15).Value = "مبلغ ثابت ریالی ماده 11 غیرخانگی فاضلاب";

            var totalCount = items.Count();
            int row = 3;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.OrganizationId;
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 15);
            //Other
            range.Column(3).Style.NumberFormat.Format = "#,##0.00";
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(4).Style.NumberFormat.Format = "#,##0.00";
            range.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(5).Style.NumberFormat.Format = "#,##0";
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(6).Style.NumberFormat.Format = "#,##0";
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(7).Style.NumberFormat.Format = "#,##0";
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(8).Style.NumberFormat.Format = "#,##0";
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(9).Style.NumberFormat.Format = "#,##0";
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            range.Column(10).Style.NumberFormat.Format = "#,##0";
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
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;

        }
    }
}
