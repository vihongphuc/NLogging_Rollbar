using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Logging
{
    public interface IWebLogContext : IDisposable
    {
        DateTime StartTime { get; }
        DateTime EndTime { get; }
        double TotalMilliseconds { get; }

        LogLevel LogLevel { get; }
        HttpStatusCode Status { get; }

        HttpRequest Request { get; }
        HttpResponse Response { get; }

        bool LoggingEnabled { get; }

        Task WriteRequestContent(Stream logStream);
        Task WriteResponseContent(Stream logStream);

        void AddException(Exception exception);
        void AddRequestInfo(string key, string value);
        void AddResponseInfo(string key, string value);
        void SetLogLevel(LogLevel logLevel);

        void EnableRequestContentTracing();
        void EnableResponseContentTracing();
        void EnableLogging();

        void AddDetailItem(LogLevel logLevel, string message, Exception exception);

        void FinalizeContext();
    }
}
