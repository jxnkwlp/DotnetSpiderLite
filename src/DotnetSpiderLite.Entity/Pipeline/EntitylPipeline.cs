using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpiderLite.Logs;

namespace DotnetSpiderLite.Entity.Pipeline
{
    public class EntitylPipeline<TEntity> : IEntitylPipeline<TEntity> where TEntity : IEntity
    {
        public Action<TEntity> OnProcess { get; set; }

        public virtual ILogger Logger { get; set; }

        public virtual void Dispose()
        {
        }

        public virtual void Process(TEntity instance)
        {
            OnProcess?.Invoke(instance);

        }
    }
}
