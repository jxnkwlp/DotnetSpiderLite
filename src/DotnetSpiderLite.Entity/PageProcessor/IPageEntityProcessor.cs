using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity.PageProcessor
{
    public interface IPageEntityProcessor
    {
        TEntity Process<TEntity>(TEntity entity, EntityDefine entityDefine, Page page) where TEntity : IEntity;

    }
}
