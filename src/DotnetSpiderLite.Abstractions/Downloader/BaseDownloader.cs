using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Abstractions.Downloader
{
    public abstract class BaseDownloader : IDownloader
    {
        private IList<IBeforeDownloadHandle> _beforeDownloadHandles = new List<IBeforeDownloadHandle>();
        private IList<IAfterDownloadHandle> _afterDownloadHandles = new List<IAfterDownloadHandle>();


        public ILogger Logger { get; set; }
         
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task DownloadAsync(DownloadContext context)
        {
            // before 
            if (_beforeDownloadHandles != null)
            {
                foreach (var handle in _beforeDownloadHandles)
                {
                    handle.Handle(context);
                }
            }

            // 
            DownloadHandleAsync(context);

            // after 
            if (_afterDownloadHandles != null)
            {
                foreach (var handle in _afterDownloadHandles)
                {
                    handle.Handle(context);
                }
            }


            return Task.CompletedTask;
        }

        public abstract Task DownloadHandleAsync(DownloadContext context);


        public void AddAfterDownloadHandle(IAfterDownloadHandle handle)
        {
            _afterDownloadHandles.Add(handle);
        }

        public void AddBeforeDownloadHandle(IBeforeDownloadHandle handle)
        {
            _beforeDownloadHandles.Add(handle);
        }
    }
}
