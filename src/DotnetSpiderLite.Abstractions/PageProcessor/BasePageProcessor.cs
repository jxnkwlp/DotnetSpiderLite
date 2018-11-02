using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotnetSpiderLite.Abstractions.Downloader;
using DotnetSpiderLite.Abstractions.Extraction;
using DotnetSpiderLite.Abstractions.Logs;

namespace DotnetSpiderLite.Abstractions.PageProcessor
{
    public abstract class BasePageProcessor : IPageProcessor, IPageExtract
    {
        public ILogger Logger { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Request> Extract(Page page)
        {
            return null;
        }

        public Task Process(Page page)
        {
            var requests = Extract(page);

            if (requests == null)
            {
                foreach (var item in requests)
                {
                }
            }


            Handle(page);

            return Task.CompletedTask;
        }


        public abstract Task Handle(Page page);
    }
}
