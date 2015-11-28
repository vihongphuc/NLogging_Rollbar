using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Logging
{
    public interface IWebLogger : IDisposable
    {
        LogLevel CurrentLogLevel { get; }

        IWebLogContext CreateContext(HttpRequest request, HttpResponse response);
        Task Log(IWebLogContext context);
    }
}
