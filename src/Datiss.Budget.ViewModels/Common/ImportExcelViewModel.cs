using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.ViewModels
{
    public class ImportExcelViewModel
    {
        public int YearId { get; set; }

        public IFormFile ExcelFile { get; set; }

        public bool ContinueIfAnyOrgMissing { get; set; }
    }
}
