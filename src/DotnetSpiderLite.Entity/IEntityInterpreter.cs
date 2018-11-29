using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity
{
    public interface IEntityInterpreter
    {
        EntityDefine Handle<TEntity>(TEntity entity) where TEntity : IEntity;
    }
}
