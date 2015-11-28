using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public enum LogLevel
    {
        Off,
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public static class LogLevelExtensions
    {
        public static bool CanLog(this LogLevel currentLevel, LogLevel logLevel)
        {
            if (currentLevel == LogLevel.Off)
                return false;

            if (logLevel >= currentLevel)
                return true;

            return false;
        }
    }
}
