using DotnetSpiderLite.Abstractions;
using DotnetSpiderLite.Abstractions.Logs;
using DotnetSpiderLite.Abstractions.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Scheduler
{
    public class SampleQueueScheduler : IScheduler
    {
        ConcurrentQueue<Request> _queue = new ConcurrentQueue<Request>();


        public ILogger Logger { get; set; }

        public Request Pull()
        {
            if (_queue.TryDequeue(out Request item))
                return item;

            return null;
        }

        public void Dispose()
        {
        }

        public void Push(Request request)
        {
            _queue.Enqueue(request);
        }
    }
}
