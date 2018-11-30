using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity.Pipeline
{
    public interface IEntitylPipeline<TEntity> : IDisposable where TEntity : IEntity
    {
        ILogger Logger { get; set; }

        void Process(TEntity instance);

    }
}
