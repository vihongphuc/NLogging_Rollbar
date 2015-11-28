using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WebLogHttpFilterAttribute : AuthorizationFilterAttribute
    {
        public LogLevel LogLevel { get; set; }
        public bool TraceRequestContent { get; set; }
        public bool TraceResponseContent { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
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
