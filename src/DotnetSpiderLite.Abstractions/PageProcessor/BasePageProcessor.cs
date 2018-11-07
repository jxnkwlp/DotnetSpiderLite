using DotnetSpiderLite.Abstractions.Logs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Abstractions.PageProcessor
{
    public abstract class BasePageProcessor : IPageProcessor
    {
        public ILogger Logger { get; set; }

        public virtual void Dispose()
        {
        }

        public Task Process(Page page)
        {
            HandlePage(page);

            return Task.CompletedTask;
        }


        public abstract Task HandlePage(Page page);

    }
}
