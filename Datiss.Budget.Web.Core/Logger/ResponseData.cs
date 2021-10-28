using Datiss.Budget.Web.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Datiss.Budget.Web.Logger
{
    public class ResponseData
    {
        public string Body { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        //public IDictionary<string, string> Cookies { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public int StatusCode { get; set; }

        public static async Task<ResponseData> CreateAsync(HttpResponse response, bool readBody) {

            var resData = new ResponseData {
                ContentLength = response.ContentLength ?? 0,
                ContentType = response.ContentType,
                Headers = response.Headers.ToDictionary(),
                StatusCode = response.StatusCode
            };

            if (readBody) {
                try {
                    resData.Body = await response.ReadBodyAsStringAsync();
                }
                catch (System.Exception ex) {
                    resData.Body = "__ERROR_READING_BODY__: " + ex.Message;
                }
            }

            return resData;
        }
    }
}
