using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;

namespace Datiss.Budget.Reports.Excel
{

    public static class CostCurrentPersonelExcel
    {
        private const string _sheetName = "CostCurrentPersonel";

        public static XLWorkbook ExportExcel(this IEnumerable<CostCurrentPersonelDTO> items)
        {
            if (items == null || !items.Any())
                return null;

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);

            sheet.RightToLeft = true;

            sheet.Cell(1, 1).Value = "سازمان";
            sheet.Cell(1, 2).Value = "نام";
            sheet.Cell(1, 3).Value = "مرکز هزینه";
            sheet.Cell(1, 4).Value = "کد پرسنلی";
            sheet.Cell(1, 5).Value = "نوع قرارداد";
            sheet.Cell(1, 6).Value = "مزد ثابت";
            sheet.Cell(1, 7).Value = "حق جذب";
            sheet.Cell(1, 8).Value = "فوق العاده منطقه";
            sheet.Cell(1, 9).Value = "مدرک تحصیلی";
            sheet.Cell(1, 10).Value = "بخش مشاغل";
            sheet.Cell(1, 11).Value = "وضعیت استخدام";
            sheet.Cell(1, 12).Value = "جزئیات استخدام";
            sheet.Cell(1, 13).Value = "مدت سابقه";
            sheet.Cell(1, 14).Value = "ساعت اضافه کاری";
            sheet.Cell(1, 15).Value = "مبلغ اضافه کاری";
            sheet.Cell(1, 16).Value = "تعداد روز  تعطیل کاری";
            sheet.Cell(1, 17).Value = "مبلغ تعطیل کاری";
            sheet.Cell(1, 18).Value = "درصد نوبت کاری";
            sheet.Cell(1, 19).Value = "نوبت کاری و کشیک";
            sheet.Cell(1, 20).Value = "تعداد ماموریت";
            sheet.Cell(1, 21).Value = "هزینه ماموریت";
            sheet.Cell(1, 22).Value = "حق سختی کار";
            sheet.Cell(1, 23).Value = "حق ایاب و ذهاب";
            sheet.Cell(1, 24).Value = "حق مسکن";
            sheet.Cell(1, 25).Value = "حق اولاد";
            sheet.Cell(1, 26).Value = "حق خواروبار";
            sheet.Cell(1, 27).Value = "آموزش";
            sheet.Cell(1, 28).Value = "بیمه سهم کارفرما";
            sheet.Cell(1, 29).Value = "بیمه عمر و حادثه تکمیلی";
            sheet.Cell(1, 30).Value = "عیدی سالیانه";
            sheet.Cell(1, 31).Value = "ذخیره مزایای پایان خدمت کارکنان";
            sheet.Cell(1, 32).Value = "مرخصی استفاده نشده";
            sheet.Cell(1, 33).Value = "هزینه های رفاهی";

            var totalCount = items.Count();
            int row = 2;
            for (int i = 0; i < totalCount; i++)
            {
                var item = items.ElementAt(i);
                sheet.Cell(row, 1).Value = item.OrganizationDisplay;
                sheet.Cell(row, 2).Value = item.FirstName + " " + item.LastName;
                sheet.Cell(row, 3).Value = item.CostCenterTypeDisplay;
                sheet.Cell(row, 4).Value = item.PersonelCode;
                sheet.Cell(row, 5).Value = item.ContractTypeDisplay;
                sheet.Cell(row, 6).Value = item.FixSalary;
                sheet.Cell(row, 6).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 7).Value = item.EmployRight;
                sheet.Cell(row, 7).Style.NumberFormat.Format = ConstantReport.__NumberFormat;
                sheet.Cell(row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Cell(row, 8).Value = item.RegionRight;
                sheet.Cell(row, 9).Value = item.GradeTypeDisplay;
                sheet.Cell(row, 10).Value = item.JobDepartmentTypeDisplay;
                sheet.Cell(row, 11).Value = item.JobStatusTypeDisplay;
                sheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 12).Value = item.JobStatusDetailTypeDisplay;
                sheet.Cell(row, 13).Value = item.ExperienceYear + "سال و " + item.ExperienceMonth + " ماه";
                sheet.Cell(row, 14).Value = item.OverTimeValue;
                sheet.Cell(row, 15).Value = item.OverTimeCost;
                sheet.Cell(row, 16).Value = item.HolidayValue;
                sheet.Cell(row, 17).Value = item.HolidayCost;
                sheet.Cell(row, 18).Value = item.ShiftPercent;
                sheet.Cell(row, 19).Value = item.ShiftPCost;
                sheet.Cell(row, 20).Value = item.MissionCount;
                sheet.Cell(row, 21).Value = item.MissionDayCost;
                sheet.Cell(row, 22).Value = item.HardWorkingRt;
                sheet.Cell(row, 23).Value = item.TrafficRt;
                sheet.Cell(row, 24).Value = item.HouseRt;
                sheet.Cell(row, 25).Value = item.ChildRt;
                sheet.Cell(row, 26).Value = item.StuffRt;
                sheet.Cell(row, 27).Value = item.Education;
                sheet.Cell(row, 28).Value = item.InsuranceMaster;
                sheet.Cell(row, 29).Value = item.InsuranceAging;
                sheet.Cell(row, 30).Value = item.HolidayYearly;
                sheet.Cell(row, 31).Value = item.MilitaryServiceCost;
                sheet.Cell(row, 32).Value = item.UnUseHolidayCount;
                sheet.Cell(row, 33).Value = item.WelfareCost;

                row++;
            }
            var range = sheet.Range(1, 1, row - 1, 33);
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

        public static XLWorkbook GetImportTemplate(this CostCurrentPersonelImportViewModel model, int year)
        {
            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(_sheetName);
            sheet.RightToLeft = true;
            //
            sheet.Cell(1, 1).Value = "جنسیت";
            sheet.Cell(1, 2).Value = "کد جنسیت";
            sheet.Cell(1, 3).Value = "سازمان";
            sheet.Cell(1, 4).Value = "کد سازمان";
            sheet.Cell(1, 5).Value = "مرکز هزینه";
            sheet.Cell(1, 6).Value = "کد مرکز هزینه";
            sheet.Cell(1, 7).Value = "نوع قرارداد";
            sheet.Cell(1, 8).Value = "کد نوع قرارداد";
            sheet.Cell(1, 9).Value = "مدرک تحصیلی";
            sheet.Cell(1, 10).Value = "کد مدرک تحصیلی";
            sheet.Cell(1, 11).Value = "بخش مشاغل";
            sheet.Cell(1, 12).Value = "کد بخش مشاغل";
            sheet.Cell(1, 13).Value = "وضعیت استخدام";
            sheet.Cell(1, 14).Value = "کد وضعیت استخدام";
            sheet.Cell(1, 15).Value = "جزئیات استخدام";
            sheet.Cell(1, 16).Value = "کد جزئیات استخدام";
            sheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 11).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 12).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 13).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 14).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 13).Style.Fill.BackgroundColor = XLColor.Cream;
            sheet.Cell(1, 14).Style.Fill.BackgroundColor = XLColor.Cream;
            int row = 2;
            sheet.Cell(row, 1).Value = "خانوم";
            sheet.Cell(row, 1).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
            sheet.Cell(row, 2).Value = 0;
            sheet.Cell(row, 2).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
            row++;
            sheet.Cell(row, 1).Value = "آقا";
            sheet.Cell(row, 1).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
            sheet.Cell(row, 2).Value = 1;
            sheet.Cell(row, 2).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
            row = 2;
            foreach (var item in model.OrganizationSource)
            {
                sheet.Cell(row, 3).Value = item.Title;
                sheet.Cell(row, 3).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 4).Value = item.Id;
                sheet.Cell(row, 4).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.CostCenterTypeSource)
            {
                sheet.Cell(row, 5).Value = item.Title;
                sheet.Cell(row, 5).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 6).Value = item.Id;
                sheet.Cell(row, 6).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.ContractTypeSource)
            {
                sheet.Cell(row, 7).Value = item.Title;
                sheet.Cell(row, 7).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 8).Value = item.Id;
                sheet.Cell(row, 8).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.GradeTypeSource)
            {
                sheet.Cell(row, 9).Value = item.Title;
                sheet.Cell(row, 9).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 10).Value = item.Id;
                sheet.Cell(row, 10).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.JobDepartmentTypeSource)
            {
                sheet.Cell(row, 11).Value = item.Title;
                sheet.Cell(row, 11).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 12).Value = item.Id;
                sheet.Cell(row, 12).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }
            row = 2;
            foreach (var item in model.JobStatusTypeSource)
            {
                sheet.Cell(row, 13).Value = item.Title;
                sheet.Cell(row, 13).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                sheet.Cell(row, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                sheet.Cell(row, 14).Value = item.Id;
                sheet.Cell(row, 14).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                row++;
            }
            row = 2;
            foreach (var item in model.JobStatusDetailTypeSource)
            {
                sheet.Cell(row, 15).Value = item.Title;
                sheet.Cell(row, 15).Style.Fill.SetBackgroundColor(XLColor.White);
                sheet.Cell(row, 16).Value = item.Id;
                sheet.Cell(row, 16).Style.Fill.SetBackgroundColor(XLColor.White);
                row++;
            }

            sheet.Range(1, 1, 23, 16);

            sheet.Cell(17, 1).Value = "ورود اطلاعات برای سال مالی : " + year;
            //sheet.Range(23, 1, 24, 15).Merge();

            row = 18;
            sheet.Cell(row, 1).Value = "نام";
            sheet.Cell(row, 2).Value = "نام خانوادگی";
            sheet.Cell(row, 3).Value = "جنسیت";
            sheet.Cell(row, 4).Value = "کد سازمان";
            sheet.Cell(row, 5).Value = "کد مرکز هزینه";
            sheet.Cell(row, 6).Value = "کد پرسنلی";
            sheet.Cell(row, 7).Value = "کد نوع قرارداد";
            sheet.Cell(row, 8).Value = "مزد ثابت";
            sheet.Cell(row, 9).Value = "حق جذب";
            sheet.Cell(row, 10).Value = "فوق العاده منطقه";
            sheet.Cell(row, 11).Value = "کد مدرک تحصیلی";
            sheet.Cell(row, 12).Value = "کد بخش مشاغل";
            sheet.Cell(row, 13).Value = "کد وضعیت استخدام";
            sheet.Cell(row, 14).Value = "کد جزئیات استخدام";
            sheet.Cell(row, 15).Value = "سال سابقه";
            sheet.Cell(row, 16).Value = "ماه سابقه";
            sheet.Cell(row, 17).Value = "ساعت اضافه کاری";
            sheet.Cell(row, 18).Value = "مبلغ اضافه کاری";
            sheet.Cell(row, 19).Value = "تعداد روز  تعطیل کاری";
            sheet.Cell(row, 20).Value = "مبلغ تعطیل کاری";
            sheet.Cell(row, 21).Value = "درصد نوبت کاری";
            sheet.Cell(row, 22).Value = "نوبت کاری و کشیک";
            sheet.Cell(row, 23).Value = "تعداد ماموریت";
            sheet.Cell(row, 24).Value = "هزینه ماموریت";
            sheet.Cell(row, 25).Value = "حق سختی کار";
            sheet.Cell(row, 26).Value = "حق ایاب و ذهاب";
            sheet.Cell(row, 27).Value = "حق مسکن";
            sheet.Cell(row, 28).Value = "حق اولاد";
            sheet.Cell(row, 29).Value = "حق خواروبار";
            sheet.Cell(row, 30).Value = "آموزش";
            sheet.Cell(row, 31).Value = "بیمه سهم کارفرما";
            sheet.Cell(row, 32).Value = "بیمه عمر و حادثه تکمیلی";
            sheet.Cell(row, 33).Value = "عیدی سالیانه";
            sheet.Cell(row, 34).Value = "ذخیره مزایای پایان خدمت کارکنان";
            sheet.Cell(row, 35).Value = "مرخصی استفاده نشده";
            sheet.Cell(row, 36).Value = "هزینه های رفاهی";

            if (!model.Items.Any())
            {
                row = 19;
                sheet.Cell(row, 1).Value = "";
                sheet.Cell(row, 2).Value = "";
                row++;
            }
            else
            {
                var totalCount = model.Items.Count();
                row = 19;
                for (int i = 0; i < totalCount; i++)
                {
                    var item = model.Items.ElementAt(i);
                    sheet.Cell(row, 1).Value = item.FirstName;
                    sheet.Cell(row, 2).Value = item.LastName;
                    row++; //for keeping index in table records
                }
            }
            var range = sheet.Range(18, 1, row - 1, 36);

            //range.Column(4).Style.NumberFormat.Format = "#,##0";
            //range.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //Other
            //range.Column(5).Style.NumberFormat.Format = "#,##0";
            //range.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //
            var table = range.CreateTable($"{_sheetName}_Table");
            table.Theme = XLTableTheme.TableStyleMedium16;
            sheet.Columns().AdjustToContents();

            return workbook;
        }

    }
}
