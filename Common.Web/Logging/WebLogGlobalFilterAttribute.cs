using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class WebLogGlobalFilterAttribute : FilterAttribute, IAuthorizationFilter, IExceptionFilter
    {
        public string[] FilterNamespaces { get; private set; }
        public bool CanTraceResponse { get; set; }

        public WebLogGlobalFilterAttribute(params string[] filterNamespaces)
        {
            FilterNamespaces = filterNamespaces.Where(c => !String.IsNullOrWhiteSpace(c)).ToArray();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var logContext = WebLoggerModule.GetCurrentLogContext();
            if (logContext == null ||
                !CheckNamespace(filterContext.ActionDescriptor.ControllerDescriptor.ControllerType))
                return;

            logContext.EnableLogging();

            logContext.AddRequestInfo(CommonRequestInfoKeys.Controller,
                                      filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            logContext.AddRequestInfo(CommonRequestInfoKeys.Action,
                                      filterContext.ActionDescriptor.ActionName);

            bool requestContentEnabled = true;
            var param = filterContext.ActionDescriptor.GetParameters();
            if (param != null && param.Length > 0)
            {
                requestContentEnabled = param.All(p => CanTraceRequestType(p.ParameterType));
            }

            if (requestContentEnabled)
            {
                logContext.EnableRequestContentTracing();
            }

            if (CanTraceResponse)
            {
                var responseContentEnabled = true;
                var refActionDescriptor = filterContext.ActionDescriptor as ReflectedActionDescriptor;
                if (refActionDescriptor != null)
                {
                    responseContentEnabled = CanTraceResponseType(refActionDescriptor.MethodInfo.ReturnType);
                }

                if (responseContentEnabled)
                {
                    logContext.EnableResponseContentTracing();
                }
            }
        }
        public void OnException(ExceptionContext actionExecutedContext)
        {
            var logContext = WebLoggerModule.GetCurrentLogContext();
            if (logContext == null ||
                !CheckNamespace(actionExecutedContext.Controller.GetType()))
                return;

            logContext.SetLogLevel(LogLevel.Error);
            logContext.AddException(actionExecutedContext.Exception);
        }

        private bool CheckNamespace(Type controllerType)
        {
            if (FilterNamespaces == null || FilterNamespaces.Length == 0)
                return true;

            return FilterNamespaces.Any(s => controllerType.Namespace.Contains(s));
        }
        private bool CanTraceRequestType(Type type)
        {
            return !((typeof(byte[]) == type) ||
                     (typeof(System.IO.Stream).IsAssignableFrom(type)));
        }
        private bool CanTraceResponseType(Type type)
        {
            if ((typeof(byte[]) == type) ||
                (typeof(System.IO.Stream).IsAssignableFrom(type)))
            {
                return false;
            }

            return true;
        }
    }
}
