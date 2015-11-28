using Common.Utilities;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Common
{
    [DebuggerStepThrough]
    public static class HttpClientExtensions
    {
        public static async Task<T> RetrieveContent<T>(this Task<HttpResponseMessage> tResp)
        {
            var resp = await tResp.ThrowOnErrors()
                                  .ConfigureAwait(false);

            return await resp.Content.ReadAsAsync<T>()
                             .ConfigureAwait(continueOnCapturedContext: false);
        }
        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string urlFormat, params string[] p)
        {
            return httpClient.GetAsync(String.Format(urlFormat, p));
        }
        public static async Task<HttpResponseMessage> ThrowOnErrors(this Task<HttpResponseMessage> tResp)
        {
            var resp = await tResp.ConfigureAwait(false);
            await resp.ThrowOnErrors().ConfigureAwait(false);

            return resp;
        }
        public static async Task<HttpResponseMessage> ThrowOnErrors(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string message = null;
                HttpError httpError = null;

                if (response.Content.Headers.ContentLength > 0 && response.Content.Headers.ContentType.MediaType != "text/html")
                {
                    httpError = await response.Content.ReadAsAsync<HttpError>().ConfigureAwait(false);
                    if (httpError != null)
                    {
                        message = httpError.Message ?? httpError.ExceptionMessage;
                    }
                    else
                    {
                        var contents = await response.Content.ReadAsAsync<Dictionary<string, string>>().ConfigureAwait(false);
                        if (contents != null)
                        {
                            message = contents.GetOrDefault(MessageKey);
                        }
                    }
                }

                throw new ServiceException(response, httpError, message ?? "An error has occurred");
            }

            return response;
        }

        private static string MessageKey = "Message";
    }
}
