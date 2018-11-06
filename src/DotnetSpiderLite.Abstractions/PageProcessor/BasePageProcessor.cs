using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.Extraction;
using DotnetSpiderLite.Abstractions.Logs;

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
            var requests = ExtractRequest(page);
            if (requests != null)
            {
                foreach (var item in requests)
                {
                    page.AddTargetRequest(item);
                }
            }


            Handle(page);

            return Task.CompletedTask;
        }


        public abstract Task Handle(Page page);

        public virtual IEnumerable<Request> ExtractRequest(Page page)
        {
            return null;
        }
    }
}
