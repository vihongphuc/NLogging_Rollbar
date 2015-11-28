using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Queue
{
    public static class QueuePublisherExtensions
    {
        public static Task Publish<T>(this IQueuePublisher<T> queuePublisher, T data)
        {
            return queuePublisher.Publish(data, null);
        }
    }
}
