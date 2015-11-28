using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Web
{
    public static class UrlHelpers
    {
        public static string TranslateSystemUrl(this UrlHelper urlHelper, string url, string defaultBaseUrl = null)
        {
            if (String.IsNullOrWhiteSpace(url))
                return null;

            url = url.Trim();

            if (url.Contains("://"))
            {
                return url;
            }
            else if (url.StartsWith("//"))
            {
                return String.Format("{0}:{1}", urlHelper.RequestContext.HttpContext.Request.Url.Scheme, url);
            }
            else if (!url.StartsWith("~") && !url.StartsWith("/") && !String.IsNullOrWhiteSpace(defaultBaseUrl))
            {
                defaultBaseUrl = defaultBaseUrl.TrimEnd('/');
                if (defaultBaseUrl.Contains("://"))
                {
                    return defaultBaseUrl + "/" + url;
                }
                else if (defaultBaseUrl.StartsWith("//"))
                {
                    return String.Format("{0}:{1}/{2}", urlHelper.RequestContext.HttpContext.Request.Url.Scheme, defaultBaseUrl, url);
                }

                return urlHelper.Content(defaultBaseUrl + "/" + url);
            }

            return urlHelper.Content(url);
        }

        public static string AbsContent(this UrlHelper urlHelper, string contentPath)
        {
            var path = urlHelper.Content(contentPath);
            var url = new Uri(urlHelper.RequestContext.HttpContext.Request.Url, path);

            return url.AbsoluteUri;
        }
    }
}