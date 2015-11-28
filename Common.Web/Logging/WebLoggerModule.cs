using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Logging
{
    public class WebLoggerModule : IHttpModule
    {
        public static readonly string WebLoggerContextItem = ".__WebLogger";

        private readonly IWebLogger webLogger;

        public static IWebLogContext GetCurrentLogContext()
        {
            return (IWebLogContext)HttpContext.Current.Items[WebLoggerContextItem];
        }

        public WebLoggerModule()
        {
            webLogger = WebLoggers.GetWebLogger();
        }

        public void Init(HttpApplication context)
        {
            if (webLogger == null)
                return;

            context.BeginRequest += context_BeginRequest;
            context.Error += context_Error;

            EventHandlerTaskAsyncHelper asyncHelper = new EventHandlerTaskAsyncHelper(context_LogRequest);
            context.AddOnLogRequestAsync(asyncHelper.BeginEventHandler, asyncHelper.EndEventHandler);
        }
        private void context_BeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var logContext = this.webLogger.CreateContext(app.Request, app.Response);
            if (logContext != null)
            {
                app.Context.Items.Add(WebLoggerContextItem, logContext);
            }
        }
        private void context_Error(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var logContext = (IWebLogContext)app.Context.Items[WebLoggerContextItem];
            if (logContext != null)
            {
                logContext.SetLogLevel(LogLevel.Error);
                logContext.AddException(app.Server.GetLastError());
            }
        }
        private async Task context_LogRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var logContext = (IWebLogContext)app.Context.Items[WebLoggerContextItem];
            if (logContext != null)
            {
                logContext.FinalizeContext();

                if (logContext.LoggingEnabled && webLogger.CurrentLogLevel.CanLog(logContext.LogLevel))
                {
                    await this.webLogger.Log(logContext);
                }

                logContext.Dispose();

                app.Context.Items.Remove(WebLoggerContextItem);
            }
        }

        public void Dispose() { }
    }
}
