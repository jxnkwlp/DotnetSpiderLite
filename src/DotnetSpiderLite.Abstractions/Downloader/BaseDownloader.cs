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

        public async Task<Response> DownloadAsync(Request request)
        {
            // before 
            if (_beforeDownloadHandles != null)
            {
                foreach (var handle in _beforeDownloadHandles)
                {
                    handle.Handle(request);
                }
            }

            // handle 
            var response = await HandleDownloadAsync(request);

            // after 
            if (_afterDownloadHandles != null)
            {
                foreach (var handle in _afterDownloadHandles)
                {
                    handle.Handle(response);
                }
            }

            return response;
        }




        public virtual void AddAfterDownloadHandle(IAfterDownloadHandle handle)
        {
            _afterDownloadHandles.Add(handle);
        }

        public virtual void AddBeforeDownloadHandle(IBeforeDownloadHandle handle)
        {
            _beforeDownloadHandles.Add(handle);
        }

        public abstract Task<Response> HandleDownloadAsync(Request request);
    }
}
