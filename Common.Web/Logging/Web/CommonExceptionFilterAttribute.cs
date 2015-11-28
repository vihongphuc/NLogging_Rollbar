using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Common.Web
{
    public class CommonExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ArgumentException)
            {
                actionExecutedContext.Response =
                    actionExecutedContext.Request
                                         .CreateErrorResponse(HttpStatusCode.BadRequest,
                                                              actionExecutedContext.Exception);

                return;
            }

            base.OnException(actionExecutedContext);
        }
    }
}
