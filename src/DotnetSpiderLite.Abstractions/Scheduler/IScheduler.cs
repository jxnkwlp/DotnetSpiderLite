using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Scheduler
{
    public interface IScheduler : IDisposable
    {
        ILogger Logger { get; set; }

        void Enqueue(Request request);

        Request Dequeue();

    }
}
