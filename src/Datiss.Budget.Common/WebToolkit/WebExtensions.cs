using System.IO;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Common.WebToolkit
{

    public static class WebExtensions
    {

        public static byte[] GetFormFileBytes(this IFormFile file) {
            if (file == null || file.Length <= 0)
                return null;

            using var fileStream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            fileStream.Read(bytes, 0, (int)file.Length);

            return bytes;
        }

        public static bool HasFileExtension(this IFormFile file, string extension) {
            var ext = Path.GetExtension(file.FileName);
            return ext.ToUpper() == extension.ToUpper();
        }

        public static bool IsNotNullOrEmpty(this IFormFile file) 
            => file != null && file.Length > 0;

        public static bool IsNullOrEmpty(this IFormFile file)
            => !IsNotNullOrEmpty(file);

        public static Dictionary<string, string> QueryStringToDictionary(this QueryString query) {
            var str = query.ToString();
            var parsed = HttpUtility.ParseQueryString(str);
            var result = parsed.AllKeys.ToDictionary(_ => _, __=> parsed[__]);

            return result;
        }

    }
}
