using System.Collections.Generic;

namespace Datiss.Budget.Web.Logger
{
    public class FormFileData
    {
        public string ContentType { get; set; }

        public string FileName { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }
    }
}
