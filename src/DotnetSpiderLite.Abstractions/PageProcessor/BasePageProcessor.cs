using DotnetSpiderLite.Logs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetSpiderLite.PageProcessor
{
    public abstract class BasePageProcessor : IPageProcessor
    {
        public ILogger Logger { get; set; }

        public virtual void Dispose()
        {
        }

        public void Process(Page page)
        {
            Logger?.Trace("Start Process...");

            HandlePage(page);

            Logger?.Trace("End Process");
        }


        public abstract void HandlePage(Page page);

    }
}
