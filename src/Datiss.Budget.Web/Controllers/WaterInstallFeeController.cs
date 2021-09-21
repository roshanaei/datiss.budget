using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Datiss.Budget.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Datiss.Budget.Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace Datiss.Budget.Web.Controllers
{
    [Authorize(Policy = ConstantPolicies.DynamicPermission)]
    [Route("[controller]")]
    public class WaterInstallFeeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IWaterInstallFeeService _waterInstallFeeService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IFinanceYearService _financeYearService;
        private readonly ISecurityTrimmingService _securityTrimmingService;

        public WaterInstallFeeController(
            IWebHostEnvironment environment,
            IWaterInstallFeeService waterInstallFeeService,
            IOrganizationService organizationService,
            IFinanceYearService financeYearService,
            IConstantService constantService,
            ISecurityTrimmingService securityTrimmingService) 
        {
            _env = environment ?? throw new ArgumentNullException(nameof(environment));
            _waterInstallFeeService = waterInstallFeeService ?? throw new ArgumentNullException(nameof(waterInstallFeeService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _financeYearService = financeYearService ?? throw new ArgumentNullException(nameof(financeYearService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _securityTrimmingService = securityTrimmingService ?? throw new ArgumentNullException(nameof(securityTrimmingService));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create(int organizationId, int yearId) {
            var model = new AddWaterInstallFeeViewModel {
                OrganizationId = organizationId,
                YearId = yearId
            };

            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(AddWaterInstallFeeViewModel model) 
        {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {

                return View(model);
            }

            var result = await _waterInstallFeeService.AddAsync(new CreateWaterInstallFeeDTO {
                DWaterTypeId = model.DWaterTypeId,
                OrganizationId = model.OrganizationId,
                WInstllFee = model.WInstllFee,
                YearId = model.YearId,
                DWaterTypeTitle = model.DWaterTypeTitle
            });

            if(! result.IsValid) {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) {
            var entity = await _waterInstallFeeService.GetByIdAsync(id);

            if(entity == null) {
                return RedirectToAction("Index");
            }

            var model = entity.Adapt<UpdateWaterInstallFeeViewModel>();
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            return View(model);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateWaterInstallFeeViewModel model) {
            var dwaterTypeSource = await _constantService.GetByConstantKeyAsync("usertype");
            model.DWaterTypeSource = dwaterTypeSource.Select(x => new SelectListItem {
                Text = x.Title,
                Value = x.Id.ToString()
            });

            if (!ModelState.IsValid) {
                return View(model);
            }

            var result = await _waterInstallFeeService.UpdateAsync(model);

            if(!result.IsValid) {
                model._HasError = true;
                model._ErrorMessage = result.Message;

                return View(model);
            }

            return RedirectToAction("Index", new { page = model._CurrentPage });
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) 
        {
            var access = _securityTrimmingService.CanCurrentUserAccess("", "WaterInstallFee", "Index");

            var filterInput = new WaterInstallFeeFilter {
                OrderBy = "dwatertype",
                PageNumber = page,
                PageSize = 2
            };

            var result = await _waterInstallFeeService.GetListAsync(filterInput);
            
            var model = new WaterInstallFeeIndexViewModel();
            model.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
            model.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync(null));
            model.Model = result;

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaterInstallFeeIndexViewModel viewModel, int page = 1) {
            if (Request.Form["btnFilter"].Count() > 0) {
                var filterInput = viewModel.Filter.Adapt<WaterInstallFeeFilter>();

                var result = await _waterInstallFeeService.GetListAsync(filterInput);

                viewModel.SetFinanceYearFilterSource(await _financeYearService.GetDropDownDataAsync());
                viewModel.SetOrganizationFilterSource(await _organizationService.GetDropDownDataAsync(null));
                viewModel.Model = result;

                return View(viewModel);
            }

            if (Request.Form["btnCreate"].Count() > 0) {
                int yearId = int.Parse(Request.Form["Filter.YearId"].ToString());
                int orgId = int.Parse(Request.Form["Filter.OrganizationId"].ToString());

                return RedirectToAction("Create", new {
                    organizationId = orgId,
                    yearId = yearId
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actions(IFormCollection form) {

            if(Request.Form["btnExcelImport"].Count > 0) {
                //var file = Request.Form["importExcelFile"].FirstOrDefault();\

                var file = form.Files[0];
                try {
                    await _waterInstallFeeService.ImportExcelAsync(file);
                }
                catch(ImportExcelFileFormatInvalidException ex) {
                    ViewData["import_error"] = "فرمت فایل اکسل نمی باشد.";
                }
                catch(ImportExcelFileSizeInvalidException ex) {
                    ViewData["import_error"] = "سایز فایل بیش از حد مجاز است.";
                }
                catch(ImportExcelFileException ex) {
                    ViewData["import_error"] = $"رکورد سطر {ex.ExcelRowIndex} از قبل وجود دارد.";
                }
            }

            if(Request.Form["btnExportExcel"].Count > 0) {
                var result = await _waterInstallFeeService.ExportExcelAsync(new WaterInstallFeeFilter());
                var ms = new MemoryStream();
                result.CopyTo(ms);

                var response = new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment") {
                    FileName = "WaterInstallFee.xlsx"
                };
                response.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                //return File(ms,
                //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                //    "WaterInstallFee.xlsx");

                //return File(response.Content,
                //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadExcelTemplate() {
            var filePath = $"{_env.WebRootPath}\\Excel\\WaterInstallFeeImport.xlsx";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(
                stream, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                "WaterInstallFee.xlsx");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ExportExcel(WaterInstallFeeIndexViewModel viewModel) {
            var filter = viewModel.Filter.Adapt<WaterInstallFeeFilter>();

            //using (var stream = new MemoryStream()) {
            //    await _waterInstallFeeService.ExportExcelAsync(filter, stream);
            //    return File(
            //        stream,
            //        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //        "WaterInstallFee.xlsx");
            //}

            //var stream = new MemoryStream();

            //try {
            //    var result = await _waterInstallFeeService.ExportExcelAsync(filter, stream);
            //    result.CopyTo(stream);
            //}
            //catch(Exception ex) {

            //}

            var result = await _waterInstallFeeService.ExportExcelAsync(filter);
            var ms = new MemoryStream();
            result.CopyTo(ms);

            return File(ms,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WaterInstallFee.xlsx");
        }
    }
}
