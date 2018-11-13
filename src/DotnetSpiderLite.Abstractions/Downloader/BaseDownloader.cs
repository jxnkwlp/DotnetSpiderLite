using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    public abstract class BaseDownloader : IDownloader
    {
        private IList<IDownloadBeforeHandle> _beforeHandles = new List<IDownloadBeforeHandle>();
        private IList<IDownloadAfterHandle> _afterHandles = new List<IDownloadAfterHandle>();


        public ILogger Logger { get; set; }

        public virtual void Dispose()
        {
        }

        public async Task<Response> DownloadAsync(Request request)
        {
            Logger?.Trace("Start Download...");

            // before 
            if (_beforeHandles?.Count > 0)
            {
                foreach (var handle in _beforeHandles)
                {
                    Logger?.Trace("Handle BeforeDownloads.");

                    handle.Handle(request);
                }
            }

            Logger?.Trace("Handle Download.");
            // handle 
            var response = await HandleDownloadAsync(request);


            // after 
            if (_afterHandles?.Count > 0)
            {
                foreach (var handle in _afterHandles)
                {
                    Logger?.Trace("Handle AfterDownloads.");

                    handle.Handle(response);
                }
            }

            Logger?.Trace("Download completed.");

            return response;
        }

        public abstract Task<Response> HandleDownloadAsync(Request request);
        public abstract IDownloader Clone();

        public void AddDownloadBeforeHandle(IDownloadBeforeHandle handle)
        {
            this._beforeHandles.Add(handle);
        }

        public void AddDownloadAfterHandle(IDownloadAfterHandle handle)
        {
            this._afterHandles.Add(handle);
        }
    }
}
