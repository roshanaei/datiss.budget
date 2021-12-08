using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class ImportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public bool AskToImport { get; set; }

        public static ImportResult Succeed(string message) 
            => new ImportResult
            {
                Success = true,
                Message = message
            };

        public static ImportResult Failed(string message)
            => new ImportResult
            {
                Message = message
            };
        
    }
}
