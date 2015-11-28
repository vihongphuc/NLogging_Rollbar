using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(HttpResponseMessage response, HttpError httpError, string message)
            : base(message ?? FormatResponse(response))
        {
            this.Response = response;
            this.HttpError = httpError;
        }

        public static string FormatResponse(HttpResponseMessage response)
        {
            return String.Format($"URL: {response.RequestMessage.RequestUri.ToString()} - {response.StatusCode}");
        }

        public HttpResponseMessage Response { get; private set; }
        public HttpError HttpError { get; private set; }
    }
}
