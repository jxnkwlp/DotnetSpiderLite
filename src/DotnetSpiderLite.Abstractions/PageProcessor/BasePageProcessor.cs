using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetSpiderLite.PageProcessor
{
    /// <summary>
    ///  页面处理器抽象类
    /// </summary>
    public abstract class BasePageProcessor : IPageProcessor
    {
        public ILogger Logger { get; set; }

        public virtual void Dispose()
        {
        }

        public void Process(Page page)
        {
            Logger?.Trace("开始处理页面");

            if (!CanProcess(page))
            {
                Logger?.Trace("忽略该处理器");

                return;
            }

            HandlePage(page);

            Logger?.Trace("页面处理完成");
        }


        public abstract void HandlePage(Page page);

        public virtual bool CanProcess(Page page)
        {
            return true;
        }

    }
}
