using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public class NlogLogger : LoggerBase
    {
        private readonly NLog.Logger logger;

        public override bool IsTraceEnabled { get { return logger.IsTraceEnabled; } }
        public override bool IsDebugEnabled { get { return logger.IsDebugEnabled; } }
        public override bool IsInfoEnabled { get { return logger.IsInfoEnabled; } }
        public override bool IsWarnEnabled { get { return logger.IsWarnEnabled; } }
        public override bool IsErrorEnabled { get { return logger.IsErrorEnabled; } }
        public override bool IsFatalEnabled { get { return logger.IsFatalEnabled; } }

        protected override void WriteLogCore(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    this.logger.Trace(message);
                    break;
                case LogLevel.Debug:
                    this.logger.Debug(message);
                    break;
                case LogLevel.Info:
                    this.logger.Info(message);
                    break;
                case LogLevel.Warn:
                    this.logger.Warn(message);
                    break;
                case LogLevel.Error:
                    this.logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    this.logger.Fatal(message);
                    break;
            }
        }
        protected override void WriteLogCore(LogLevel logLevel, string message, Exception ex)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    this.logger.Trace(message, ex);
                    break;
                case LogLevel.Debug:
                    this.logger.Debug(message, ex);
                    break;
                case LogLevel.Info:
                    this.logger.Info(message, ex);
                    break;
                case LogLevel.Warn:
                    this.logger.Warn(message, ex);
                    break;
                case LogLevel.Error:
                    this.logger.Error(message, ex);
                    break;
                case LogLevel.Fatal:
                    this.logger.Fatal(message, ex);
                    break;
            }
        }

        public NlogLogger(string name)
        {
            this.logger = LogManager.GetLogger(name);
        }
    }
}
