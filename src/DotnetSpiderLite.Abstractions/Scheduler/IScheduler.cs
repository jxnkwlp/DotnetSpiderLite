using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Scheduler
{
    public interface IScheduler : IDisposable
    {
        ILogger Logger { get; set; }

        /// <summary>
        ///  放进一个
        /// </summary>
        /// <param name="request"></param>
        void Push(Request request);

        /// <summary>
        ///  拉取一个
        /// </summary>
        /// <returns></returns>
        Request Pull();

    }
}
