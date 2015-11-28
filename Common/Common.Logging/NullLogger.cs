using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public class NullLogger : LoggerBase
    {
        protected override void WriteLogCore(LogLevel logLevel, string message)
        {
        }
        protected override void WriteLogCore(LogLevel logLevel, string message, Exception ex)
        {
        }

        public static readonly ICommonLogger Instance = new NullLogger();

        public override bool IsTraceEnabled { get { return false; } }
        public override bool IsDebugEnabled { get { return false; } }
        public override bool IsInfoEnabled { get { return false; } }
        public override bool IsWarnEnabled { get { return false; } }
        public override bool IsErrorEnabled { get { return false; } }
        public override bool IsFatalEnabled { get { return false; } }
    }
}
