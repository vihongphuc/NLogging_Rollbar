using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Queue
{
    public interface IQueuePublisher<T> : IDisposable
    {
        Task Publish(T data, IDictionary<string, object> headers);
    }
}
