using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public class DetailWebLogger : LoggerBase
    {
        private readonly IWebLogContext webLogContext;

        public override bool IsTraceEnabled
        {
            get { return this.webLogContext.LogLevel.CanLog(LogLevel.Trace); }
        }
        public override bool IsDebugEnabled
        {
            get { return this.webLogContext.LogLevel.CanLog(LogLevel.Debug); }
        }
        public override bool IsInfoEnabled
        {
            get { return this.webLogContext.LogLevel.CanLog(LogLevel.Info); }
        }
        public override bool IsWarnEnabled
        {
            get { return this.webLogContext.LogLevel.CanLog(LogLevel.Warn); }
        }
        public override bool IsErrorEnabled
        {
            get { return this.webLogContext.LogLevel.CanLog(LogLevel.Error); }
        }
        public override bool IsFatalEnabled
        {
            get { return this.webLogContext.LogLevel.CanLog(LogLevel.Fatal); }
        }

        public DetailWebLogger(IWebLogContext webLogContext)
        {
            this.webLogContext = webLogContext;
        }

        protected override void WriteLogCore(LogLevel logLevel, string message)
        {
            this.webLogContext.AddDetailItem(logLevel, message, null);
        }
        protected override void WriteLogCore(LogLevel logLevel, string message, Exception ex)
        {
            this.webLogContext.AddDetailItem(logLevel, message, ex);
        }
    }
}
