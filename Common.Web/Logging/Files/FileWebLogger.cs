using Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Common.Logging.Files
{
    public class FileWebLogger : IWebLogger
    {
        public static readonly string DefaultMethodPath = "_Default";
        public static readonly string UnknownMethod = "_";
        public static readonly string Extension = "log";

        private readonly string directory;
        private long counter;

        public LogLevel CurrentLogLevel { get; private set; }

        public FileWebLogger(string directory)
        {
            this.directory = directory;
            CurrentLogLevel = WebLoggers.GetLogLevel();
        }

        public IWebLogContext CreateContext(HttpRequest request, HttpResponse response)
        {
            return new FileWebLogContext(request, response);
        }
        public async Task Log(IWebLogContext context)
        {
            var fileContext = (FileWebLogContext)context;
            if (!fileContext.LoggingEnabled)
                return;

            var path = GetDirectoryName(fileContext);

            var targetPath = await Task.Run(delegate
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return Path.Combine(path, GetFileName(fileContext));
            });

            await WriteToFile(fileContext, targetPath);
        }

        private async Task WriteToFile(FileWebLogContext context, string filename)
        {
            using (var fileStream = File.OpenWrite(filename))
            {
                using (var sw = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    sw.WriteLine("{0} {1} {2} {3}", (int)context.Status, context.Status, context.Request.HttpMethod, context.Request.RawUrl);
                    sw.WriteLine("Start    : {0:yyyy-MM-dd HH:mm:ss.fffffff}", context.StartTime);
                    sw.WriteLine("End      : {0:yyyy-MM-dd HH:mm:ss.fffffff}", context.EndTime);
                    sw.WriteLine("Duration : {0}", context.TotalMilliseconds);
                    sw.WriteLine("LogLevel : {0}", context.LogLevel);
                    sw.WriteLine();

                    if (context.DetailItems.Count > 0)
                    {
                        bool first = true;
                        foreach (var detailItem in context.DetailItems)
                        {
                            if (context.LogLevel.CanLog(detailItem.LogLevel))
                            {
                                if (first)
                                {
                                    first = false;
                                    sw.WriteLine("Logging:", context.TotalMilliseconds);
                                }

                                sw.Write("+{0:#,##0.000} ", detailItem.OffsetMilliseconds);
                                sw.Write("{0:HH:mm:ss.fffffff} ", context.StartTime.AddMilliseconds(detailItem.OffsetMilliseconds));
                                sw.Write(detailItem.Message);
                                sw.WriteLine();

                                if (detailItem.Exception != null)
                                {
                                    sw.WriteLine(detailItem.Exception);
                                }
                            }
                        }

                        if (!first)
                        {
                            sw.WriteLine();
                        }
                    }

                    WriteExceptions(context, fileStream, sw);
                    await WriteRequestPart(context, fileStream, sw);
                    await WriteResponsePart(context, fileStream, sw);
                }
            }
        }

        private void WriteExceptions(FileWebLogContext context, FileStream fileStream, StreamWriter sw)
        {
            if (context.Exceptions.Count > 0)
            {
                sw.WriteLine("Exceptions:");
                sw.WriteLine();

                var first = true;
                foreach (var ex in context.Exceptions)
                {
                    if (false == first)
                    {
                        WriteSeparator(sw);
                        sw.WriteLine();
                    }
                    else
                    {
                        first = false;
                    }

                    var sbsEx = ex as ServiceException;
                    if (sbsEx != null)
                    {
                        if (sbsEx.Response != null)
                        {
                            sw.WriteLine($"Http Request: {ServiceException.FormatResponse(sbsEx.Response)}");
                            sw.WriteLine($"{sbsEx.Response}");
                            sw.WriteLine();
                        }

                        if (sbsEx.HttpError != null)
                        {
                            sw.WriteLine("Http Error:");
                            foreach (var kv in sbsEx.HttpError)
                            {
                                sw.WriteLine($"   {kv.Key} = {kv.Value}");
                            }
                            sw.WriteLine();
                        }
                    }

                    sw.WriteLine(ex);
                }
                WriteSeparator(sw);
            }
        }

        private static async Task WriteRequestPart(FileWebLogContext context, FileStream fileStream, StreamWriter sw)
        {
            if (context.RequestInfo.Count > 0)
            {
                sw.WriteLine("Request Information:");
                foreach (var kv in context.RequestInfo)
                {
                    sw.WriteLine("    {0}: {1}", kv.Key, kv.Value);
                }
                sw.WriteLine();
            }

            if (context.Request.Headers.Count > 0)
            {
                sw.WriteLine("Request Headers:");
                foreach (string header in context.Request.Headers)
                {
                    sw.WriteLine("    {0}: {1}", header, context.Request.Headers[header]);
                }
                sw.WriteLine();
            }

            if (context.RequestContentTracingEnabled)
            {
                sw.WriteLine("Request Content:");
                await sw.FlushAsync();

                await context.WriteRequestContent(fileStream);

                WriteSeparator(sw);
            }
        }
        private static async Task WriteResponsePart(FileWebLogContext context, FileStream fileStream, StreamWriter sw)
        {
            if (context.ResponseInfo.Count > 0)
            {
                sw.WriteLine("Response Information:");
                foreach (var kv in context.ResponseInfo)
                {
                    sw.WriteLine("    {0}: {1}", kv.Key, kv.Value);
                }
                sw.WriteLine();
            }

            if (context.Response.Headers.Count > 0)
            {
                sw.WriteLine("Response Headers:");
                foreach (string header in context.Response.Headers)
                {
                    sw.WriteLine("    {0}: {1}", header, context.Response.Headers[header]);
                }
                sw.WriteLine();
            }

            if (context.ResponseContentTracingEnabled)
            {
                sw.WriteLine("Response Content:");
                sw.WriteLine();
                await sw.FlushAsync();

                await context.WriteResponseContent(fileStream);

                WriteSeparator(sw);
            }
        }
        private static void WriteSeparator(StreamWriter sw)
        {
            sw.WriteLine();
            sw.WriteLine("===========================================================");
            sw.WriteLine();
        }

        private string GetFileName(FileWebLogContext context)
        {
            var iCounter = ((ulong)Interlocked.Increment(ref counter) % 1000000);
            return String.Format("{0}.{1}.{2:HHmmssfffffff}.{3:000000}.{4}", context.Request.HttpMethod, (int)context.Response.StatusCode, context.StartTime, iCounter, Extension);
        }
        private string GetDirectoryName(FileWebLogContext context)
        {
            var baseDirPath = Path.Combine(this.directory,
                                           context.StartTime.Year.ToString(),
                                           context.StartTime.Month.ToString(),
                                           context.StartTime.Day.ToString());

            var methodPath = context.GetPath();
            if (String.IsNullOrWhiteSpace(methodPath))
            {
                methodPath = DefaultMethodPath;
                var virtualPath = HostingEnvironment.VirtualPathProvider.GetDirectory(context.Request.Url.LocalPath).VirtualPath;
                if (!String.IsNullOrWhiteSpace(virtualPath))
                {
                    virtualPath = virtualPath.TrimStart('/', '\\')
                                             .Replace("/", "\\");

                    methodPath = Path.Combine(methodPath, virtualPath);
                }
            }

            return Path.Combine(baseDirPath, methodPath);
        }

        public void Dispose() { }
    }
}
