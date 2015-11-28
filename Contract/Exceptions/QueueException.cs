using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class QueueException : Exception
    {
        public QueueException()
        {
        }
        public QueueException(string message)
            : base(message)
        {
        }
        protected QueueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        public QueueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
