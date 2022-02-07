using System.IO;
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

    }
}
