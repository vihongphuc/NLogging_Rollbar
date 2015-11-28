using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Logging
{
    public class WebLogContext : IWebLogContext
    {
        private readonly object syncLock = new object();

        private MemoryCopyStream responseCopyStream;
        private Stopwatch stopWatch;

        private Lazy<List<LogContextDetailItem>> detailItems = new Lazy<List<LogContextDetailItem>>();
        private Lazy<List<Exception>> exceptions = new Lazy<List<Exception>>();
        private Lazy<List<KeyValuePair<string, string>>> requestInfo = new Lazy<List<KeyValuePair<string, string>>>();
        private Lazy<List<KeyValuePair<string, string>>> responseInfo = new Lazy<List<KeyValuePair<string, string>>>();

        public WebLogContext(HttpRequest request, HttpResponse response)
        {
            this.Request = request;
            this.Response = response;

            this.LogLevel = Logging.LogLevel.Debug;
        }

        public string Controller { get; private set; }
        public string Action { get; private set; }

        public LogLevel LogLevel { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public double TotalMilliseconds { get; private set; }
        public bool Ended { get; private set; }

        public HttpStatusCode Status { get; private set; }
        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }

        public bool RequestContentTracingEnabled { get; private set; }
        public bool ResponseContentTracingEnabled { get; private set; }

        public bool LoggingEnabled { get; private set; }

        public IList<LogContextDetailItem> DetailItems
        {
            get
            {
                if (detailItems.IsValueCreated)
                    return detailItems.Value;

                return EmptyList<LogContextDetailItem>.Value;
            }
        }
        public IList<Exception> Exceptions
        {
            get
            {
                if (exceptions.IsValueCreated)
                    return exceptions.Value;

                return EmptyList<Exception>.Value;
            }
        }
        public IList<KeyValuePair<string, string>> RequestInfo
        {
            get
            {
                if (requestInfo.IsValueCreated)
                    return requestInfo.Value;

                return EmptyList<KeyValuePair<string, string>>.Value;
            }
        }
        public IList<KeyValuePair<string, string>> ResponseInfo
        {
            get
            {
                if (responseInfo.IsValueCreated)
                    return responseInfo.Value;

                return EmptyList<KeyValuePair<string, string>>.Value;
            }
        }

        public async Task WriteRequestContent(Stream logStream)
        {
            if (RequestContentTracingEnabled)
            {
                if (Request.InputStream.CanSeek && Request.InputStream.CanRead)
                {
                    Request.InputStream.Seek(0, SeekOrigin.Begin);
                    await Request.InputStream.CopyToAsync(logStream);
                    Request.InputStream.Seek(0, SeekOrigin.Begin);
                }
            }
        }
        public async Task WriteResponseContent(Stream logStream)
        {
            if (ResponseContentTracingEnabled)
            {
                this.responseCopyStream.CopyStream.Seek(0, SeekOrigin.Begin);
                await this.responseCopyStream.CopyStream.CopyToAsync(logStream);
            }
        }

        public virtual void AddDetailItem(LogLevel logLevel, string message, Exception exception)
        {
            lock (syncLock)
            {
                if (Ended)
                    return;

                var offsetTime = stopWatch.ElapsedMilliseconds;

                detailItems.Value.Add(new LogContextDetailItem
                {
                    OffsetMilliseconds = offsetTime,
                    LogLevel = logLevel,
                    Message = message,
                    Exception = exception
                });
            }
        }

        public virtual void AddException(Exception exception)
        {
            if (Ended)
                return;

            exceptions.Value.Add(exception);
        }
        public virtual void AddRequestInfo(string key, string value)
        {
            if (Ended)
                return;

            requestInfo.Value.Add(new KeyValuePair<string, string>(key, value));

            if (String.Equals(key, CommonRequestInfoKeys.Controller, StringComparison.OrdinalIgnoreCase))
            {
                Controller = value;
            }
            else if (String.Equals(key, CommonRequestInfoKeys.Action, StringComparison.OrdinalIgnoreCase))
            {
                Action = value;
            }
        }
        public virtual void AddResponseInfo(string key, string value)
        {
            if (Ended)
                return;

            requestInfo.Value.Add(new KeyValuePair<string, string>(key, value));
        }

        public virtual void EnableRequestContentTracing()
        {
            if (Ended)
                return;

            this.RequestContentTracingEnabled = true;
        }
        public virtual void EnableResponseContentTracing()
        {
            if (Ended || this.ResponseContentTracingEnabled)
                return;

            this.ResponseContentTracingEnabled = true;
            SetupResponseCopyStream();
        }
        private void SetupResponseCopyStream()
        {
            if (LoggingEnabled && this.ResponseContentTracingEnabled)
            {
                responseCopyStream = new MemoryCopyStream(this.Response.Filter);
                this.Response.Filter = responseCopyStream;
            }
        }

        public virtual void EnableLogging()
        {
            if (Ended ||
                LoggingEnabled)
                return;

            LoggingEnabled = true;
            StartTime = DateTime.Now;

            stopWatch = new Stopwatch();
            stopWatch.Start();

            SetupResponseCopyStream();
        }

        public virtual void SetLogLevel(LogLevel logLevel)
        {
            if (Ended)
                return;

            this.LogLevel = logLevel;
        }

        public virtual void FinalizeContext()
        {
            lock (syncLock)
            {
                if (Ended)
                    return;

                if (LoggingEnabled)
                {
                    this.Status = (HttpStatusCode)Response.StatusCode;

                    if (String.IsNullOrWhiteSpace(Controller))
                    {
                        if (this.Response.StatusCode < 100 || this.Response.StatusCode >= 400)
                        {
                            this.SetLogLevel(LogLevel.Error);
                        }
                    }

                    TotalMilliseconds = stopWatch.ElapsedMilliseconds;
                    EndTime = DateTime.Now;

                    stopWatch.Stop();
                }

                Ended = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        ~WebLogContext()
        {
            Dispose(disposing: false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.responseCopyStream != null)
                {
                    this.responseCopyStream.CloseCopyStream();
                }
            }
        }
    }
}
