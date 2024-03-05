using System;
//using Serilog.Context;

namespace Datiss.Budget.Web.Logger
{
    internal static class ExceptionLogger
    {
        public static void Log(LogContextData c, Exception ex, string displayMessage) {
//            LogContext.Reset();

//            using (LogContext.PushProperty("Type", ex.GetType().FullName))
//            using (LogContext.PushProperty("HResult", ex.HResult))
//            using (LogContext.PushProperty("Message", ex.Message))
//            using (LogContext.PushProperty("Source", ex.Source))
//            using (LogContext.PushProperty("ConnectionId", c.Connection.Id))
//            using (LogContext.PushProperty("ConnectionIp", c.Connection.RemoteIpv4))
//            using (LogContext.PushProperty("ConnectionPort", c.Connection.RemotePort))
//            using (LogContext.PushProperty("ContentLength", c.Request.ContentLength))
//            using (LogContext.PushProperty("ContentType", c.Request.ContentType))
//            using (LogContext.PushProperty("Cookies", c.Request.Cookies))
//            using (LogContext.PushProperty("Form", c.Request.Form))
//            using (LogContext.PushProperty("Files", c.Request.FormFiles))
//            using (LogContext.PushProperty("Headers", c.Request.Headers))
//            using (LogContext.PushProperty("Host", c.Request.Host))
//            using (LogContext.PushProperty("IsHttps", c.Request.IsHttps))
//            using (LogContext.PushProperty("Method", c.Request.Method))
//            using (LogContext.PushProperty("Path", c.Request.Path))
//            using (LogContext.PushProperty("Protocol", c.Request.Protocol))
//            using (LogContext.PushProperty("Query", c.Request.Query))
//            using (LogContext.PushProperty("Scheme", c.Request.Scheme))
//            using (LogContext.PushProperty("TraceIdentifier", c.TraceIdentifier))
//            using (LogContext.PushProperty("IsAuthenticated", c.User.IsAuthenticated))
//            using (LogContext.PushProperty("UserId", c.User.Id))
///*            using (LogContext.PushProperty("UserName", c.UserName))*/ {
//                Serilog.Log.Error(ex, "{DisplayMessage}", displayMessage);
//            }
        }
    }
}
