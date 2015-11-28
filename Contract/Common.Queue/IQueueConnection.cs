using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Queue
{
    public interface IQueueConnection : IDisposable
    {
        IQueuePublisher<T> CreatePublisher<T>(string queueName, string exchangeName = null);
        IQueuePublisher<T> CreateFanoutPublisher<T>(string exchangeName);
        IQueuePublisher<T> CreateHeaderPublisher<T>(string exchangeName);
        IQueuePublisher<T> CreateTopicPublisher<T>(string exchangeName, string pattern);
        IDisposable Subscribe<T>(string queueName, QueueProcessor<T> processor);
    }

    public delegate Task<QueueProcessResult> QueueProcessor<T>(T data, IDictionary<string, object> headers);
}
