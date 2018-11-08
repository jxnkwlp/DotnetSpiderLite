using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Pipeline
{
    /// <summary>
    ///  数据管道
    /// </summary>
    public interface IPipeline : IDisposable
    {
        ILogger Logger { get; set; }

        void Process(IList<ResultItems> resultItems);

    }
}
