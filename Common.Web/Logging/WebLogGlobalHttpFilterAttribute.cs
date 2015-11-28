using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Common.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WebLogGlobalHttpFilterAttribute : AuthorizationFilterAttribute, IExceptionFilter
    {
        public string[] FilterNamespaces { get; set; }

        public WebLogGlobalHttpFilterAttribute(params string[] filterNamespaces)
        {
            FilterNamespaces = filterNamespaces.Where(c => !String.IsNullOrWhiteSpace(c)).ToArray();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var logContext = WebLoggerModule.GetCurrentLogContext();
            if (logContext == null ||
                !CheckNamespace(actionContext.ActionDescriptor.ControllerDescriptor.ControllerType))
                return;

            logContext.EnableLogging();

            logContext.AddRequestInfo(CommonRequestInfoKeys.Controller,
                                      actionContext.ControllerContext.ControllerDescriptor.ControllerName);
            logContext.AddRequestInfo(CommonRequestInfoKeys.Action,
                                      actionContext.ActionDescriptor.ActionName);

            bool requestContentEnabled = true;
            var param = actionContext.ActionDescriptor.GetParameters();
            if (param != null && param.Count > 0)
            {
                requestContentEnabled = param.All(p => CanTraceType(p.ParameterType));
            }

            if (requestContentEnabled)
            {
                logContext.EnableRequestContentTracing();
            }

            if (CanTraceType(actionContext.ActionDescriptor.ReturnType))
            {
                logContext.EnableResponseContentTracing();
            }

            base.OnAuthorization(actionContext);
        }
        public void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var logContext = WebLoggerModule.GetCurrentLogContext();
            if (logContext == null ||
                !CheckNamespace(actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType))
                return;

            logContext.SetLogLevel(LogLevel.Error);
            logContext.AddException(actionExecutedContext.Exception);

            //if (actionExecutedContext.ExceptionHandled)
            //    return;

            //(new RollbarClient()).SendException(filterContext.Exception);
        }

        private bool CheckNamespace(Type controllerType)
        {
            if (FilterNamespaces == null || FilterNamespaces.Length == 0)
                return true;

            return FilterNamespaces.Any(s => controllerType.Namespace.Contains(s));
        }
        private bool CanTraceType(Type type)
        {
            return !((typeof(byte[]) == type) ||
                     (typeof(System.IO.Stream).IsAssignableFrom(type)));
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            OnAuthorization(actionContext);

            var source = new TaskCompletionSource<object>(null);
            source.SetResult(null);
            return source.Task;
        }
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException("actionExecutedContext");
            }

            OnException(actionExecutedContext);

            var source = new TaskCompletionSource<object>(null);
            source.SetResult(null);
            return source.Task;
        }
    }
}
