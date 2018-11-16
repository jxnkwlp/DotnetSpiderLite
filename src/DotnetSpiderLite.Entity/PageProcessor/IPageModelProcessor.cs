using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Entity.PageProcessor
{
    public interface IPageModelProcessor<T>
    {
        T Process();
    }
}
