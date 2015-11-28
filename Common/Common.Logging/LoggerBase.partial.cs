using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    partial class LoggerBase : ICommonLogger
    {
		#region Trace
        public void Trace(Func<string> fn)
        {
            if (IsTraceEnabled)
                Wrap(() => WriteLog(LogLevel.Trace, fn()));
        }
        public void Trace<T>(T value)
        {
            if (IsTraceEnabled)
                Wrap(() => WriteLog(LogLevel.Trace, value.ToString()));
        }
        public void Trace(string message)
        {
            if (IsTraceEnabled)
                WriteLog(LogLevel.Trace, message);
        }
        public void Trace(string message, params object[] args)
        {
            Trace(() => String.Format(message, args));
        }
        public void Trace(string message, Exception exception)
        {
            if (IsTraceEnabled)
                WriteLog(LogLevel.Trace, message, exception);
        }
        public void Trace<TArgument>(string message, TArgument argument)
        {
            Trace(() => String.Format(message, argument));
        }
        public void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            Trace(() => String.Format(message, argument1, argument2));
        }
        public void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            Trace(() => String.Format(message, argument1, argument2, argument3));
        }

		public void TraceAll(params object[] args)
		{
            Trace(() => String.Join(",", args.Select(s => s.ToString())));
		}
        #endregion

		#region Debug
        public void Debug(Func<string> fn)
        {
            if (IsDebugEnabled)
                Wrap(() => WriteLog(LogLevel.Debug, fn()));
        }
        public void Debug<T>(T value)
        {
            if (IsDebugEnabled)
                Wrap(() => WriteLog(LogLevel.Debug, value.ToString()));
        }
        public void Debug(string message)
        {
            if (IsDebugEnabled)
                WriteLog(LogLevel.Debug, message);
        }
        public void Debug(string message, params object[] args)
        {
            Debug(() => String.Format(message, args));
        }
        public void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
                WriteLog(LogLevel.Debug, message, exception);
        }
        public void Debug<TArgument>(string message, TArgument argument)
        {
            Debug(() => String.Format(message, argument));
        }
        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            Debug(() => String.Format(message, argument1, argument2));
        }
        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            Debug(() => String.Format(message, argument1, argument2, argument3));
        }

		public void DebugAll(params object[] args)
		{
            Debug(() => String.Join(",", args.Select(s => s.ToString())));
		}
        #endregion

		#region Info
        public void Info(Func<string> fn)
        {
            if (IsInfoEnabled)
                Wrap(() => WriteLog(LogLevel.Info, fn()));
        }
        public void Info<T>(T value)
        {
            if (IsInfoEnabled)
                Wrap(() => WriteLog(LogLevel.Info, value.ToString()));
        }
        public void Info(string message)
        {
            if (IsInfoEnabled)
                WriteLog(LogLevel.Info, message);
        }
        public void Info(string message, params object[] args)
        {
            Info(() => String.Format(message, args));
        }
        public void Info(string message, Exception exception)
        {
            if (IsInfoEnabled)
                WriteLog(LogLevel.Info, message, exception);
        }
        public void Info<TArgument>(string message, TArgument argument)
        {
            Info(() => String.Format(message, argument));
        }
        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            Info(() => String.Format(message, argument1, argument2));
        }
        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            Info(() => String.Format(message, argument1, argument2, argument3));
        }

		public void InfoAll(params object[] args)
		{
            Info(() => String.Join(",", args.Select(s => s.ToString())));
		}
        #endregion

		#region Warn
        public void Warn(Func<string> fn)
        {
            if (IsWarnEnabled)
                Wrap(() => WriteLog(LogLevel.Warn, fn()));
        }
        public void Warn<T>(T value)
        {
            if (IsWarnEnabled)
                Wrap(() => WriteLog(LogLevel.Warn, value.ToString()));
        }
        public void Warn(string message)
        {
            if (IsWarnEnabled)
                WriteLog(LogLevel.Warn, message);
        }
        public void Warn(string message, params object[] args)
        {
            Warn(() => String.Format(message, args));
        }
        public void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
                WriteLog(LogLevel.Warn, message, exception);
        }
        public void Warn<TArgument>(string message, TArgument argument)
        {
            Warn(() => String.Format(message, argument));
        }
        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            Warn(() => String.Format(message, argument1, argument2));
        }
        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            Warn(() => String.Format(message, argument1, argument2, argument3));
        }

		public void WarnAll(params object[] args)
		{
            Warn(() => String.Join(",", args.Select(s => s.ToString())));
		}
        #endregion

		#region Error
        public void Error(Func<string> fn)
        {
            if (IsErrorEnabled)
                Wrap(() => WriteLog(LogLevel.Error, fn()));
        }
        public void Error<T>(T value)
        {
            if (IsErrorEnabled)
                Wrap(() => WriteLog(LogLevel.Error, value.ToString()));
        }
        public void Error(string message)
        {
            if (IsErrorEnabled)
                WriteLog(LogLevel.Error, message);
        }
        public void Error(string message, params object[] args)
        {
            Error(() => String.Format(message, args));
        }
        public void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
                WriteLog(LogLevel.Error, message, exception);
        }
        public void Error<TArgument>(string message, TArgument argument)
        {
            Error(() => String.Format(message, argument));
        }
        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            Error(() => String.Format(message, argument1, argument2));
        }
        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            Error(() => String.Format(message, argument1, argument2, argument3));
        }

		public void ErrorAll(params object[] args)
		{
            Error(() => String.Join(",", args.Select(s => s.ToString())));
		}
        #endregion

		#region Fatal
        public void Fatal(Func<string> fn)
        {
            if (IsFatalEnabled)
                Wrap(() => WriteLog(LogLevel.Fatal, fn()));
        }
        public void Fatal<T>(T value)
        {
            if (IsFatalEnabled)
                Wrap(() => WriteLog(LogLevel.Fatal, value.ToString()));
        }
        public void Fatal(string message)
        {
            if (IsFatalEnabled)
                WriteLog(LogLevel.Fatal, message);
        }
        public void Fatal(string message, params object[] args)
        {
            Fatal(() => String.Format(message, args));
        }
        public void Fatal(string message, Exception exception)
        {
            if (IsFatalEnabled)
                WriteLog(LogLevel.Fatal, message, exception);
        }
        public void Fatal<TArgument>(string message, TArgument argument)
        {
            Fatal(() => String.Format(message, argument));
        }
        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            Fatal(() => String.Format(message, argument1, argument2));
        }
        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            Fatal(() => String.Format(message, argument1, argument2, argument3));
        }

		public void FatalAll(params object[] args)
		{
            Fatal(() => String.Join(",", args.Select(s => s.ToString())));
		}
        #endregion

    }
}
