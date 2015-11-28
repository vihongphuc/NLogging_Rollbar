using Common.Logging;
using RollbarSharp;
using System.Web;
using System.Web.Mvc;

namespace NLogging
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new WebLogGlobalFilterAttribute() { CanTraceResponse = WebLoggers.GetMvcLogAll() });
            //filters.Add(new SbsHandleErrorAttribute());
        }
        
    }

    public class RollbarExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            (new RollbarClient()).SendException(filterContext.Exception, modelAction: m => m.Context = "error#context");
        }
    }
}
