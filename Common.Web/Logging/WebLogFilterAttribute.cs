using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;

namespace Common.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class WebLogFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public LogLevel LogLevel { get; set; }
        public bool TraceRequestContent { get; set; }
        public bool TraceResponseContent { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var logContext = WebLoggerModule.GetCurrentLogContext();
            if (logContext == null)
                return;

            if (LogLevel != Logging.LogLevel.Off)
            {
                logContext.SetLogLevel(LogLevel);
            }
            if (TraceRequestContent)
            {
                logContext.EnableRequestContentTracing();
            }
            if (TraceResponseContent)
            {
                logContext.EnableResponseContentTracing();
            }
        }
    }
}
