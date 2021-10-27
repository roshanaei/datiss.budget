using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Datiss.Budget.Web.Logger
{
    public class LogContextData
    {
        public const string KEY = "__Context_Data__";

        public ConnectionData Connection { get; set; }

        public RequestData Request { get; set; }

        public ResponseData Response { get; set; }

        public UserData User { get; set; }

        public bool IsWebSocketRequest { get; set; }

        public string TraceIdentifier { get; set; }

        public double ElapsedMilliseconds { get; set; }

        public Dictionary<string, object> ToDictionary() {
            // TODO: check null properties.
            return new Dictionary<string, object>
            {
                //-- Connection
                { "ConnectionId", Connection.Id },
                { "RemoteIp", Connection.RemoteIpv4 },
                { "RemotePort", Connection.RemotePort },

                //-- Request
                { "ContentLength", Request.ContentLength },
                { "Cookies", Request.Cookies },
                { "Form", Request.Form },
                { "Files", Request.FormFiles },
                { "Headers", Request.Headers },
                { "Host", Request.Host },
                { "IsHttps", Request.IsHttps },
                { "Method", Request.Method },
                { "Path", Request.Path },
                { "Protocol", Request.Protocol },
                { "Query", Request.Query },
                { "RequestBody", Request.Body },
                { "RequestContentType", Request.ContentType },
                { "Scheme", Request.Scheme },

                //-- Response
                { "ResponseBody", Response.Body },
                { "ResponseContentType", Response.ContentType },
                { "StatusCode", Response.StatusCode },

                //-- User
                { "IsAuthenticated", User.IsAuthenticated },
                { "UserId", User.Id },

                //-- Misc.
                { "ElapsedMilliseconds", ElapsedMilliseconds },
                { "TraceIdentifier", TraceIdentifier },
            };
        }

        public override string ToString() {
            // TODO: check null properties.
            return $"{Request.Protocol} {Request.Method} {Request.Path}, {Response.StatusCode} {Response.ContentType} ({ElapsedMilliseconds}ms)";
        }

        public static async Task<LogContextData> CreateAsync(HttpContext context, bool readResponseBody) {

            return new LogContextData {
                Connection = ConnectionData.Create(context.Connection),
                Request = await RequestData.CreateAsync(context.Request),
                Response = await ResponseData.CreateAsync(context.Response, readResponseBody),
                User = UserData.Create(context.User),

                IsWebSocketRequest = context.WebSockets?.IsWebSocketRequest ?? false,
                TraceIdentifier = context.TraceIdentifier,
            };
        }
    }
}
