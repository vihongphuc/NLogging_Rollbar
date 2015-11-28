using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
#if !DEBUG
    [System.Diagnostics.DebuggerStepThrough]
#endif
    public abstract partial class LoggerBase
    {
        public abstract bool IsTraceEnabled { get; }
        public abstract bool IsDebugEnabled { get; }
        public abstract bool IsInfoEnabled { get; }
        public abstract bool IsWarnEnabled { get; }
        public abstract bool IsErrorEnabled { get; }
        public abstract bool IsFatalEnabled { get; }

        protected abstract void WriteLogCore(LogLevel logLevel, string message);
        protected abstract void WriteLogCore(LogLevel logLevel, string message, Exception ex);

        protected void WriteLog(LogLevel logLevel, string message)
        {
            try
            {
                WriteLogCore(logLevel, message);
            }
            catch (Exception ex)
            {
                HandleWriteError(ex);
            }
        }
        protected void WriteLog(LogLevel logLevel, string message, Exception ex)
        {
            try
            {
                WriteLogCore(logLevel, message, ex);
            }
            catch (Exception wEx)
            {
                HandleWriteError(wEx);
            }
        }

        protected virtual void HandleLoggingError(Exception ex)
        {
            WriteLog(LogLevel.Warn, String.Format("## The message failed to be logged ##"));
        }
        protected virtual void HandleWriteError(Exception ex)
        {
        }

        private void Wrap(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                HandleLoggingError(ex);
            }
        }

        public void Dispose() { Dispose(true); }
        protected virtual void Dispose(bool disposing) { }
    }
}
