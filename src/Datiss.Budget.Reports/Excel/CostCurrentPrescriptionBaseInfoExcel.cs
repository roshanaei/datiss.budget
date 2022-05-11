using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Datiss.Budget.Reports.Excel
{
    public static class CostCurrentPrescriptionBaseInfoExcel
    {
        private const string _sheetName = "CostCurrentPrescriptionBaseInfo";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentPrescriptionBaseInfoDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "سال";
            sheet.Cell(1, 2).Value = "حداقل دستمزد";
            sheet.Cell(1, 3).Value = "حق مسکن";
            sheet.Cell(1, 4).Value = "بن کارگری";
            sheet.Cell(1, 5).Value = "حق جذب";
            sheet.Cell(1, 6).Value = "حق خوار و بار";
            sheet.Cell(1, 7).Value = "حق اولاد";
            sheet.Cell(1, 8).Value = "سختی کار";
            sheet.Cell(1, 9).Value = "فوق العاده منطقه";
            sheet.Cell(1, 10).Value = "بهداشت و درمان";
            sheet.Cell(1, 11).Value = "مزد ثابت نیروی جدید";

            var totalCount = items.Count();
            int row = 2;

            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.Year.ToString();
                sheet.Cell(row, 1).DataType = XLDataType.Text;
                sheet.Cell(row, 2).Value = item.FixSalary;
                sheet.Cell(row, 2).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 3).Value = item.HouseRt;
                sheet.Cell(row, 3).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 4).Value = item.Copun;
                sheet.Cell(row, 4).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 5).Value = item.EmployRight;
                sheet.Cell(row, 5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 6).Value = item.StuffRt;
                sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.ChildRt;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.HardWorkingRt;
                sheet.Cell(row, 8).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 9).Value = item.RegionRight;
                sheet.Cell(row, 9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 10).Value = item.Healths;
                sheet.Cell(row, 10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 11).Value = item.NewFixSalary;
                sheet.Cell(row, 11).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row++;
            }

            var range = sheet.Range(1, 1, row - 1, 11);

            var table = range.CreateTable($"{_sheetName}_Table");
            
            table.Theme = XLTableTheme.TableStyleMedium16;
            
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this int year)
        {

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;
            sheet.Cell(1, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            sheet.Range(1, 1, 1, 10).Merge();

            sheet.Cell(2, 1).Value = "حداقل دستمزد";
            sheet.Cell(2, 2).Value = "حق مسکن";
            sheet.Cell(2, 3).Value = "بن کارگری";
            sheet.Cell(2, 4).Value = "حق جذب";
            sheet.Cell(2, 5).Value = "حق خوار و بار";
            sheet.Cell(2, 6).Value = "حق اولاد";
            sheet.Cell(2, 7).Value = "سختی کار";
            sheet.Cell(2, 8).Value = "فوق العاده منطقه";
            sheet.Cell(2, 9).Value = "بهداشت و درمان";
            sheet.Cell(2, 10).Value = "مزد ثابت نیروی جدید";

            int row = 3;
            for (int i = 0; i < 3; i++)
            {
                row++; //for keeping index in table records
            }

            var range = sheet.Range(2, 1, row - 1, 10);

            range.Column(1).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(2).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(3).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(4).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(5).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(8).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(9).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Column(10).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
            range.Column(10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
