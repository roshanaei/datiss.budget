using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Web.Http;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Web.Logger
{
    public class RequestData
    {

        private const int _maxBodyLength = 1024;

        public string Body { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public IDictionary<string, string> Cookies { get; set; }
        public IDictionary<string, string> Form { get; set; }
        public IDictionary<string, string> FormFiles { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Host { get; set; }
        public bool IsHttps { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Protocol { get; set; }
        public IDictionary<string, string> Query { get; set; }
        public string Scheme { get; set; }

        // methods

        public static async Task<RequestData> CreateAsync(HttpRequest request) {
            request.CheckArgumentIsNull(nameof(request));

            var reqData = new RequestData();

            try {
                reqData.Body = await request.ReadBodyAsStringAsync();
            }
            catch (Exception ex) {
                reqData.Body = "__ERROR_READING_BODY__: " + ex.Message;
            }

            reqData.ContentLength = request.ContentLength ?? 0;
            reqData.ContentType = request.ContentType;
            reqData.Cookies = request.Cookies.ToDictionary();

            if (request.HasFormContentType) {
                try {
                    reqData.Form = request.Form.ToDictionary();
                }
                catch (Exception ex) {
                    reqData.Form = new Dictionary<string, string> { { "__ERROR__", ex.Message } };
                }

                try {
                    reqData.FormFiles = request.Form.Files.ToDictionary();
                }
                catch (Exception ex) {
                    reqData.FormFiles = new Dictionary<string, string> { { "__ERROR__", ex.Message } };
                }
            }

            reqData.Headers = request.Headers.ToDictionary();
            reqData.Host = request.Host.Value;
            reqData.IsHttps = request.IsHttps;
            reqData.Method = request.Method.ToUpper();
            reqData.Path = request.Path.Value.ToLower();
            reqData.Protocol = request.Protocol.ToUpper();
            reqData.Query = request.Query.ToDictionary();
            reqData.Scheme = request.Scheme.ToUpper();

            return reqData;
        }
    }
}
