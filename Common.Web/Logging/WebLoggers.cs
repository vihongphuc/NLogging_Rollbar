using Common.Logging.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Common.Logging
{
    public static class WebLoggers
    {
        public static readonly string FileWebLoggerType = "File";

        private static IWebLogger webLogger;

        public static IWebLogger GetWebLogger()
        {
            lock (typeof(WebLoggers))
            {
                if (webLogger == null)
                {
                    var webLoggerType = ConfigurationManager.Get(WebLoggerConfigurations.Group, WebLoggerConfigurations.Type);
                    if (String.Equals(webLoggerType, FileWebLoggerType, StringComparison.OrdinalIgnoreCase))
                    {
                        webLogger = CreateFileWebLogger();
                    }
                    //else if (String.Equals(webLoggerType, RabbitMqWebLoggerType, StringComparison.OrdinalIgnoreCase))
                    //{
                    //}

                    var regObject = webLogger as IRegisteredObject;
                    if (regObject != null)
                    {
                        HostingEnvironment.RegisterObject(regObject);
                    }
                }

                return webLogger;
            }
        }
        public static LogLevel GetLogLevel()
        {
            var configValue = ConfigurationManager.Get(WebLoggerConfigurations.Group, WebLoggerConfigurations.LogLevel);
            if (!String.IsNullOrWhiteSpace(configValue))
            {
                LogLevel level;
                if (Enum.TryParse<LogLevel>(configValue, out level))
                {
                    return level;
                }
            }

            return LogLevel.Info;
        }

        public static bool GetMvcLogAll()
        {
            return ConfigurationManager.Get(WebLoggerConfigurations.Group, WebLoggerConfigurations.MvcLogAll, false);
        }

        private static FileWebLogger CreateFileWebLogger()
        {
            var directory = ConfigurationManager.Get(WebLoggerConfigurations.Group, WebLoggerConfigurations.Directory);
            if (String.IsNullOrWhiteSpace(directory))
            {
                directory = HostingEnvironment.MapPath(@"~\");
            }
            else if (directory.StartsWith("~"))
            {
                directory = HostingEnvironment.MapPath(directory);
            }

            return new FileWebLogger(directory);
        }
    }
}
