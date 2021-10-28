using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Datiss.Budget.Web.Http
{
    public static class HttpExtensions {

        public static async Task<string> ReadBodyAsStringAsync(this HttpRequest request) {

            if (request.ContentLength == null || request.ContentLength == 0 || request.Body == null)
                return null;

            string body = null;

            /* request body is a FrameRequestStream and it doesn�t support seeking.
             * EnableRewind() allows us to set the reader for the request back at the beginning of its stream. */

            request.Body.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(request.Body)) {
                body = await reader.ReadToEndAsync();

                request.Body.Seek(0, SeekOrigin.Begin);
                return body;
            }
        }

        public static async Task<string> ReadBodyAsStringAsync(this HttpResponse response) {

            if (response.HasStarted)
                return "__RESPONSE_STARTED__";

            if (response.ContentLength == null || response.ContentLength == 0 || response.Body == null)
                return null;

            string body = null;

            // We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            // ...and copy it into a string
            using (var reader = new StreamReader(response.Body)) {
                body = await reader.ReadToEndAsync();

                // We need to reset the reader for the response so that the client can read it.
                response.Body.Seek(0, SeekOrigin.Begin);

                // Return the string for the response
                return body;
            }
        }

        public static Dictionary<string, string> ToDictionary(this IHeaderDictionary obj) {
            if (obj == null)
                return null;

            var dic = new Dictionary<string, string>();
            foreach (var key in obj.Keys) {
                if (key != null) {
                    if (obj.TryGetValue(key, out StringValues value))
                        dic.Add(key, value);
                    else
                        dic.Add(key, "__ERROR_READING_VALUE__");
                }
            }

            return dic;
        }

        public static Dictionary<string, string> ToDictionary(this IFormCollection obj) {
            if (obj == null)
                return null;

            var dic = new Dictionary<string, string>();
            foreach (var key in obj.Keys) {
                if (key != null) {
                    if (obj.TryGetValue(key, out StringValues value))
                        dic.Add(key, value);
                    else
                        dic.Add(key, "__ERROR_READING_VALUE__");
                }
            }

            return dic;
        }

        public static Dictionary<string, string> ToDictionary(this IFormFileCollection obj) {
            if (obj == null)
                return null;

            var dic = new Dictionary<string, string>();
            foreach (var file in obj) {
                if (file != null)
                    dic.Add(file.Name, file.FileName);
            }

            return dic;
        }

        public static Dictionary<string, string> ToDictionary(this IQueryCollection obj) {
            if (obj == null)
                return null;

            var dic = new Dictionary<string, string>();
            foreach (var key in obj.Keys) {
                if (key != null) {
                    if (obj.TryGetValue(key, out StringValues value))
                        dic.Add(key, value);
                    else
                        dic.Add(key, "__ERROR_READING_VALUE__");
                }
            }

            return dic;
        }

        public static Dictionary<string, string> ToDictionary(this IRequestCookieCollection obj) {
            if (obj == null)
                return null;

            var dic = new Dictionary<string, string>();
            foreach (var key in obj.Keys) {
                if (key != null) {
                    if (obj.TryGetValue(key, out string value))
                        dic.Add(key, value);
                    else
                        dic.Add(key, "__ERROR_READING_VALUE__");
                }
            }

            return dic;
        }

    }
}
