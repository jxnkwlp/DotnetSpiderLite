using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Downloader
{
    /// <summary>
    ///  抽象类下载器
    /// </summary>
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
            Logger?.Trace($"开始请求 {request.Method} {request.Uri}");

            // before 
            if (_beforeHandles?.Count > 0)
            {
                foreach (var handle in _beforeHandles)
                {
                    Logger?.Trace($"执行前置处理 {handle.GetType()}");

                    handle.Handle(request);
                }
            }

            Logger?.Trace("执行请求");
            // handle 
            var response = await HandleDownloadAsync(request);


            // after 
            if (_afterHandles?.Count > 0)
            {
                foreach (var handle in _afterHandles)
                {
                    Logger?.Trace($"执行后置处理 {handle.GetType()}");

                    handle.Handle(response);
                }
            }

            Logger?.Trace($"请求完成 {request.Method} {request.Uri}");

            return response;
        }

        public abstract Task<Response> HandleDownloadAsync(Request request);

        public abstract IDownloader Clone();

        /// <summary>
        ///  添加前置处理
        /// </summary> 
        public void AddDownloadBeforeHandle(IDownloadBeforeHandle handle)
        {
            handle.Logger = this.Logger;
            this._beforeHandles.Add(handle);
        }

        /// <summary>
        ///  添加后置处理
        /// </summary> 
        public void AddDownloadAfterHandle(IDownloadAfterHandle handle)
        {
            handle.Logger = this.Logger;
            this._afterHandles.Add(handle);
        }
    }
}
