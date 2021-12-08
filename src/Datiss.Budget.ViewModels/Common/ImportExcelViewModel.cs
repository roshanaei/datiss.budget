using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class ImportExcelViewModel
    {

        public IFormFile ExcelFile { get; set; }

        public bool ContinueIfAnyOrgMissing { get; set; }
    }
}
