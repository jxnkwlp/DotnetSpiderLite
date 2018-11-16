using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity.Pipeline
{
    public interface IModelPipeline<T> : IDisposable
    {
        ILogger Logger { get; set; }

        void Process(T instance);

    }
}
