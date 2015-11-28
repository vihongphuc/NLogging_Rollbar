using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public interface ICommonLogger : IDisposable
    {
        bool IsTraceEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }

        #region Trace
        void Trace(Func<string> fn);
        void Trace<T>(T value);
        void Trace(string message);
        void Trace(string message, params object[] args);
        void Trace(string message, Exception exception);
        void Trace<TArgument>(string message, TArgument argument);
        void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        #endregion
        #region Debug
        void Debug(Func<string> fn);
        void Debug<T>(T value);
        void Debug(string message);
        void Debug(string message, params object[] args);
        void Debug(string message, Exception exception);
        void Debug<TArgument>(string message, TArgument argument);
        void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        #endregion
        #region Info
        void Info(Func<string> fn);
        void Info<T>(T value);
        void Info(string message);
        void Info(string message, params object[] args);
        void Info(string message, Exception exception);
        void Info<TArgument>(string message, TArgument argument);
        void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        #endregion
        #region Warn
        void Warn(Func<string> fn);
        void Warn<T>(T value);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Warn(string message, Exception exception);
        void Warn<TArgument>(string message, TArgument argument);
        void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        #endregion
        #region Error
        void Error(Func<string> fn);
        void Error<T>(T value);
        void Error(string message);
        void Error(string message, params object[] args);
        void Error(string message, Exception exception);
        void Error<TArgument>(string message, TArgument argument);
        void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        #endregion
        #region Fatal
        void Fatal(Func<string> fn);
        void Fatal<T>(T value);
        void Fatal(string message);
        void Fatal(string message, params object[] args);
        void Fatal(string message, Exception exception);
        void Fatal<TArgument>(string message, TArgument argument);
        void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        #endregion
    }
}
